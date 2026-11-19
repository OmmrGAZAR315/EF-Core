using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Session4Bank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Branches_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OpeningDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
                    table.CheckConstraint("CK_Accounts_CurrentBalance", "[CurrentBalance] >= 0");
                    table.ForeignKey(
                        name: "FK_Accounts_Branches_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "Branches",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAccounts",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    OwnershipType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OwnershipStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccounts", x => new { x.CustomerId, x.AccountNumber });
                    table.ForeignKey(
                        name: "FK_CustomerAccounts_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionNumber);
                    table.CheckConstraint("CK_Transactions_Amount", "[Amount] > 0");
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "CustomerType", "DateOfBirth", "Email", "FullName", "NationalId", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "10 Nasr St, Cairo", "Individual", new DateOnly(1990, 1, 1), "ahmed.ali@mail.com", "Ahmed Ali", "29001011234567", "01011111111" },
                    { 2, "5 Port St, Alexandria", "Business", new DateOnly(2010, 5, 20), "info@niletrading.com", "Nile Trading LLC", "20100520001", "034440000" },
                    { 3, "3 Haram St, Giza", "Individual", new DateOnly(1995, 5, 15), "salma.youssef@mail.com", "Salma Youssef", "29505151234568", "01022222222" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "Email", "FullName", "HireDate", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "hossam.fathy@nationalbank.com", "Hossam Fathy", new DateOnly(2015, 3, 1), "01000000001" },
                    { 2, "dina.mahmoud@nationalbank.com", "Dina Mahmoud", new DateOnly(2017, 6, 15), "01000000002" },
                    { 3, "karim.saad@nationalbank.com", "Karim Saad", new DateOnly(2019, 9, 10), "01000000003" }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Code", "Address", "ManagerId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { "ALX-01", "22 Corniche Rd, Alexandria", 2, "Alexandria Branch", "034567890" },
                    { "CAI-01", "15 Tahrir Square, Cairo", 1, "Cairo Main Branch", "0223456789" },
                    { "GIZ-01", "8 Pyramids St, Giza", 3, "Giza Branch", "0237654321" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountNumber", "AccountType", "BranchCode", "CurrentBalance", "OpeningDate" },
                values: new object[,]
                {
                    { "2001-CUR", "Current", "CAI-01", 15400m, new DateOnly(2020, 2, 10) },
                    { "3000-BUS", "Business", "ALX-01", 120000m, new DateOnly(2019, 7, 1) },
                    { "4001-SAV", "Savings", "GIZ-01", 52000m, new DateOnly(2021, 11, 5) }
                });

            migrationBuilder.InsertData(
                table: "CustomerAccounts",
                columns: new[] { "AccountNumber", "CustomerId", "AccountStatus", "OwnershipStartDate", "OwnershipType" },
                values: new object[,]
                {
                    { "2001-CUR", 1, "Active", new DateOnly(2020, 2, 10), "Primary" },
                    { "4001-SAV", 1, "Active", new DateOnly(2022, 1, 15), "CoHolder" },
                    { "3000-BUS", 2, "Active", new DateOnly(2019, 7, 1), "Primary" },
                    { "4001-SAV", 3, "Active", new DateOnly(2021, 11, 5), "Primary" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionNumber", "AccountNumber", "Amount", "Note", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { 1, "2001-CUR", 20000m, "Initial deposit", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deposit" },
                    { 2, "2001-CUR", 4600m, "Rent payment", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Withdrawal" },
                    { 3, "3000-BUS", 150000m, "Capital injection", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deposit" },
                    { 4, "3000-BUS", 30000m, "Supplier payment", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Transfer" },
                    { 5, "4001-SAV", 50000m, "Opening deposit", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deposit" },
                    { 6, "4001-SAV", 2000m, "Monthly savings", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deposit" },
                    { 7, "4001-SAV", 2000m, null, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deposit" },
                    { 8, "4001-SAV", 2000m, "ATM withdrawal", new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Withdrawal" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BranchCode",
                table: "Accounts",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ManagerId",
                table: "Branches",
                column: "ManagerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_AccountNumber",
                table: "CustomerAccounts",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_NationalId",
                table: "Customers",
                column: "NationalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Email",
                table: "Managers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountNumber",
                table: "Transactions",
                column: "AccountNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAccounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Managers");
        }
    }
}
