using System.Globalization;
using System.Net.Mail;
using EFCore.Models.S4;
using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class Program
{
    public static void Main()
    {
        using var db = new AppDbContext();

        try
        {
            while (true)
            {
                DrawMenu();
                var choice = ReadInt("  Enter choice: ", 0, 5);
                if (choice == 0)
                    break;

                Console.WriteLine();
                try
                {
                    switch (choice)
                    {
                        case 1: AddCustomer(db); break;
                        case 2: OpenAccount(db); break;
                        case 3: UpdateAccountStatus(db); break;
                        case 4: RemoveAccountFromCustomer(db); break;
                        case 5: ListCustomers(db); break;
                    }
                }
                catch (DbUpdateException ex)
                {
                    Error($"Database error: {ex.InnerException?.Message ?? ex.Message}");
                    db.ChangeTracker.Clear();
                }

                Pause();
            }
        }
        catch (EndOfStreamException)
        {
        }

        Console.WriteLine();
        Console.WriteLine("Goodbye!");
    }

    private static void DrawMenu()
    {
        try { Console.Clear(); } catch (IOException) { }

        Console.WriteLine("========================================");
        Console.WriteLine("    National Bank - Management");
        Console.WriteLine("========================================");
        Console.WriteLine("  1) Add a new Customer");
        Console.WriteLine("  2) Open a new Account for a Customer");
        Console.WriteLine("  3) Update Account Status (Active / Closed)");
        Console.WriteLine("  4) Remove an Account from a Customer");
        Console.WriteLine("  5) List all Customers (with accounts)");
        Console.WriteLine("  0) Exit");
        Console.WriteLine("----------------------------------------");
    }

    private static void AddCustomer(AppDbContext db)
    {
        Console.WriteLine("--- Add New Customer ---");

        var fullName = ReadRequired("Full Name      : ", 100);

        var nationalId = ReadValidated("National ID    : ", 20,
            v => v.All(char.IsDigit) ? null : "National ID must contain digits only.");
        if (db.Customers.Any(c => c.NationalId == nationalId))
        {
            Error($"A customer with National ID '{nationalId}' already exists.");
            return;
        }

        var dateOfBirth = ReadDate("Date of Birth  : (yyyy-MM-dd) ");

        var email = ReadValidated("Email          : ", 150,
            v => MailAddress.TryCreate(v, out var a) && a.Address == v ? null : "Please enter a valid email address.");

        var phone = ReadValidated("Phone          : ", 20,
            v => v.All(char.IsDigit) && v.Length is >= 10 and <= 15 ? null : "Phone must be 10-15 digits.");

        var address = ReadRequired("Address        : ", 200);

        var type = ReadEnum<CustomerType>("Customer Type:");

        var customer = new Customer
        {
            FullName = fullName,
            NationalId = nationalId,
            DateOfBirth = dateOfBirth,
            Email = email,
            PhoneNumber = phone,
            Address = address,
            CustomerType = type
        };

        db.Customers.Add(customer);
        db.SaveChanges();

        Console.WriteLine();
        Success($"Customer created successfully. CustomerId = {customer.Id}");
    }

    private static void OpenAccount(AppDbContext db)
    {
        Console.WriteLine("--- Open New Account ---");

        var accountNumber = ReadRequired("Account Number : ", 20);
        if (db.Accounts.Any(a => a.AccountNumber == accountNumber))
        {
            Error($"Account '{accountNumber}' already exists.");
            return;
        }

        var accountType = ReadEnum<AccountType>("Account Type:");
        var branchCode = ReadRequired("Branch Code    : ", 10).ToUpperInvariant();
        var customerId = ReadInt("Customer Id    : ", 1);
        var role = ReadEnum<OwnershipType>("Ownership Role:");

        Info($"Validating branch '{branchCode}' and customer #{customerId}...");

        var branch = db.Branches.Find(branchCode);
        if (branch == null)
        {
            Error($"Branch '{branchCode}' does not exist.");
            return;
        }

        var customer = db.Customers.Find(customerId);
        if (customer == null)
        {
            Error($"Customer #{customerId} does not exist.");
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var account = new Account
        {
            AccountNumber = accountNumber,
            AccountType = accountType,
            CurrentBalance = 0,
            OpeningDate = today,
            BranchCode = branch.Code
        };

        account.CustomerAccounts.Add(new CustomerAccount
        {
            CustomerId = customer.Id,
            OwnershipType = role,
            OwnershipStartDate = today,
            AccountStatus = AccountStatus.Active
        });

        db.Accounts.Add(account);
        db.SaveChanges();

        Success($"Account '{accountNumber}' created and linked to customer {customer.Id} as {role} owner.");
    }

    private static void UpdateAccountStatus(AppDbContext db)
    {
        Console.WriteLine("--- Update Account Status ---");

        var accountNumber = ReadRequired("Account Number : ", 20);
        var customerId = ReadInt("Customer Id    : ", 1);

        var link = db.CustomerAccounts.Find(customerId, accountNumber);
        if (link == null)
        {
            Error($"Customer #{customerId} is not an owner of account '{accountNumber}'.");
            return;
        }

        Info($"Current status: {link.AccountStatus}");
        var newStatus = ReadEnum<AccountStatus>("New Status:");

        if (newStatus == link.AccountStatus)
        {
            Info($"Status is already {newStatus}. Nothing changed.");
            return;
        }

        link.AccountStatus = newStatus;
        db.SaveChanges();

        Success($"Status updated to {newStatus}.");
    }

    private static void RemoveAccountFromCustomer(AppDbContext db)
    {
        Console.WriteLine("--- Remove Account From Customer ---");

        var accountNumber = ReadRequired("Account Number : ", 20);
        var customerId = ReadInt("Customer Id    : ", 1);

        var link = db.CustomerAccounts.Find(customerId, accountNumber);
        if (link == null)
        {
            Error($"Customer #{customerId} is not an owner of account '{accountNumber}'.");
            return;
        }

        var otherOwners = db.CustomerAccounts
            .Count(ca => ca.AccountNumber == accountNumber && ca.CustomerId != customerId);

        db.CustomerAccounts.Remove(link);

        if (otherOwners == 0)
        {
            var account = db.Accounts.Find(accountNumber);
            if (account != null)
                db.Accounts.Remove(account);
        }

        db.SaveChanges();

        Console.WriteLine();
        Success("  Ownership link deleted.");
        if (otherOwners == 0)
            Info($"      That was the last owner - account '{accountNumber}' was also removed.");
        else
            Info($"      Account '{accountNumber}' still has {otherOwners} other owner(s).");
    }

    private static void ListCustomers(AppDbContext db)
    {
        Console.WriteLine("--- All Customers ---");
        Console.WriteLine();

        var customers = db.Customers
            .AsNoTracking()
            .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                    .ThenInclude(a => a.Branch)
            .OrderBy(c => c.Id)
            .ToList();

        if (customers.Count == 0)
        {
            Console.WriteLine("  (no customers)");
            return;
        }

        foreach (var c in customers)
        {
            Console.WriteLine($"  #{c.Id} {c.FullName} ({c.CustomerType})");

            if (c.CustomerAccounts.Count == 0)
            {
                Console.WriteLine("      (no accounts)");
                continue;
            }

            foreach (var ca in c.CustomerAccounts.OrderBy(ca => ca.AccountNumber))
            {
                var a = ca.Account;
                Console.WriteLine(
                    $"      {a.AccountNumber,-10} {a.AccountType,-9} Balance: {a.CurrentBalance.ToString("N2", CultureInfo.InvariantCulture),12}   " +
                    $"{ca.OwnershipType,-9} {ca.AccountStatus,-7} @ {a.Branch.Name}");
            }
        }
    }

    private static string ReadLine()
        => Console.ReadLine() ?? throw new EndOfStreamException();

    private static string ReadRequired(string prompt, int maxLength)
        => ReadValidated(prompt, maxLength, _ => null);

    private static string ReadValidated(string prompt, int maxLength, Func<string, string?> validate)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadLine().Trim();

            if (input.Length == 0)
            {
                Error("This field is required.");
                continue;
            }

            if (input.Length > maxLength)
            {
                Error($"Must be at most {maxLength} characters.");
                continue;
            }

            var error = validate(input);
            if (error != null)
            {
                Error(error);
                continue;
            }

            return input;
        }
    }

    private static int ReadInt(string prompt, int min, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadLine().Trim();

            if (!int.TryParse(input, out var value))
            {
                Error("Please enter a whole number.");
                continue;
            }

            if (value < min || value > max)
            {
                Error(max == int.MaxValue
                    ? $"Please enter a number of at least {min}."
                    : $"Please enter a number between {min} and {max}.");
                continue;
            }

            return value;
        }
    }

    private static DateOnly ReadDate(string prompt)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        while (true)
        {
            Console.Write(prompt);
            var input = ReadLine().Trim();

            if (!DateOnly.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var date))
            {
                Error("Please use the format yyyy-MM-dd (e.g. 2000-01-25).");
                continue;
            }

            if (date > today || date.Year < 1900)
            {
                Error("Date must be between 1900-01-01 and today.");
                continue;
            }

            return date;
        }
    }

    private static T ReadEnum<T>(string title) where T : struct, Enum
    {
        var values = Enum.GetValues<T>();

        Console.WriteLine(title);
        for (var i = 0; i < values.Length; i++)
            Console.WriteLine($"    {i + 1}) {values[i]}");

        var choice = ReadInt("  Choice: ", 1, values.Length);
        return values[choice - 1];
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press any key to return to the menu...");
        try
        {
            Console.ReadKey(intercept: true);
        }
        catch (InvalidOperationException)
        {
            ReadLine();
        }
    }

    private static void Success(string message) => WriteColored(message, ConsoleColor.Green);
    private static void Info(string message) => WriteColored(message, ConsoleColor.Cyan);
    private static void Error(string message) => WriteColored(message, ConsoleColor.Red);

    private static void WriteColored(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
