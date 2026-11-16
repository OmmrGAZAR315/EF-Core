using EFCore.Models.S3;
using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class Program
{
    public static void Main()
    {
        using (var db = new AppDbContext())
        {
            SeedStudents(db);
            SeedEnrollments(db);
        }

        Header("CREATE: Add an instructor");
        var instructorId = AddInstructor("Dr. Ahmed El-Sayed", "ahmed.elsayed@mail.com",
            "Expert in C# and .NET technologies with 15 years of experience.");

        Header("CREATE: Add a course");
        AddCourse(instructorId ?? 0, 5, "Clean Architecture in .NET", 89.99m, CourseLevel.Advanced);
        AddCourse(instructorId ?? 0, 999, "Course With Missing Category", 10m, CourseLevel.Beginner);
        AddCourse(999, 5, "Course With Missing Instructor", 10m, CourseLevel.Beginner);

        Header("CREATE: Add several students");
        AddStudents();

        var omar = GetStudentId("omar.khaled@mail.com");
        var mariam = GetStudentId("mariam.adel@mail.com");
        var nour = GetStudentId("nour.hany@mail.com");
        var laila = GetStudentId("laila.hassan@mail.com");

        Header("CREATE: Enroll a student");
        EnrollStudent(laila, 4);
        EnrollStudent(omar, 3);
        EnrollStudent(omar, 1);
        EnrollStudent(999, 1);
        EnrollStudent(omar, 999);

        Header("READ: Find a course");
        FindCourse(1, "React from Scratch");
        FindCourse(999, "No Such Course");

        Header("READ: Filter courses by price");
        FilterCoursesByPrice(20m, 60m);
        FilterCoursesByPrice(60m, 20m);

        Header("READ: Sub-categories");
        PrintSubCategories(1);
        PrintSubCategories(999);
        PrintTopLevelCategories();

        Header("READ: Course roster");
        PrintCourseRoster(1);
        PrintCourseRoster(999);

        Header("UPDATE: Change a price");
        ChangeCoursePrice(4, 34.99m);
        ChangeCoursePrice(4, -10m);
        ChangeCoursePrice(999, 10m);

        Header("UPDATE: Update progress");
        UpdateProgress(mariam, 4, 150);
        UpdateProgress(mariam, 4, 100);
        UpdateProgress(omar, 999, 50);

        Header("UPDATE: Publish a course");
        PublishCourse(5);
        PublishCourse(999);

        Header("DELETE: Unenroll");
        Unenroll(nour, 6);

        Header("DELETE: Delete a category");
        DeleteCategoryUnsafe(1);
        DeleteCategorySafe(1);
        DeleteCategorySafe(6);
        DeleteCategorySafe(999);

        Header("DELETE: Delete a course");
        DeleteCourse(2);

        Header("BULK: Raise price by 10%");
        BulkRaisePrice(CourseLevel.Intermediate);

        Header("BULK: Delete dropped enrollments");
        BulkDeleteDroppedEnrollments();
    }

    private static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"===== {title} =====");
    }

    private static int GetStudentId(string email)
    {
        using var db = new AppDbContext();
        return db.Students
            .Where(s => s.Email == email)
            .Select(s => s.Id)
            .FirstOrDefault();
    }

    private static int? AddInstructor(string fullName, string email, string? bio)
    {
        using var db = new AppDbContext();

        var existingId = db.Instructors
            .Where(i => i.Email == email)
            .Select(i => (int?)i.Id)
            .FirstOrDefault();
        if (existingId != null)
        {
            Console.WriteLine($"An instructor with email '{email}' already exists (Id = {existingId}).");
            return existingId;
        }

        var instructor = new Instructor
        {
            FullName = fullName,
            Email = email,
            Bio = bio,
            JoinedAt = DateTime.Today
        };

        db.Instructors.Add(instructor);
        db.SaveChanges();

        Console.WriteLine($"Instructor '{instructor.FullName}' added with Id = {instructor.Id}.");
        return instructor.Id;
    }

    private static void AddCourse(int instructorId, int categoryId, string title, decimal price, CourseLevel level)
    {
        using var db = new AppDbContext();

        if (!db.Instructors.Any(i => i.Id == instructorId))
        {
            Console.WriteLine($"Cannot add '{title}': instructor {instructorId} does not exist.");
            return;
        }

        if (!db.Categories.Any(c => c.Id == categoryId))
        {
            Console.WriteLine($"Cannot add '{title}': category {categoryId} does not exist.");
            return;
        }

        if (db.Courses.Any(c => c.Title == title))
        {
            Console.WriteLine($"A course titled '{title}' already exists.");
            return;
        }

        var course = new Course
        {
            Title = title,
            Price = price,
            Level = level,
            IsPublished = false,
            InstructorId = instructorId,
            CategoryId = categoryId
        };

        db.Courses.Add(course);
        db.SaveChanges();

        Console.WriteLine($"Course '{course.Title}' added with Id = {course.Id}.");
    }

    private static void AddStudents()
    {
        using var db = new AppDbContext();

        var candidates = new List<Student>
        {
            new() { FullName = "Laila Hassan", Email = "laila.hassan@mail.com", DateOfBirth = new DateOnly(2003, 2, 14) },
            new() { FullName = "Tarek Nabil", Email = "tarek.nabil@mail.com", DateOfBirth = new DateOnly(2000, 12, 1) },
            new() { FullName = "Hana Waleed", Email = "hana.waleed@mail.com", DateOfBirth = new DateOnly(2001, 8, 22) },
        };

        var emails = candidates.Select(s => s.Email).ToList();
        var existingEmails = db.Students
            .Where(s => emails.Contains(s.Email))
            .Select(s => s.Email)
            .ToList();

        var newStudents = candidates.Where(s => !existingEmails.Contains(s.Email)).ToList();
        if (newStudents.Count == 0)
        {
            Console.WriteLine("These students already exist, nothing to add.");
            return;
        }

        db.Students.AddRange(newStudents);
        db.SaveChanges();

        foreach (var s in newStudents)
            Console.WriteLine($"Student '{s.FullName}' added with Id = {s.Id}.");
    }

    private static void EnrollStudent(int studentId, int courseId)
    {
        using var db = new AppDbContext();

        if (!db.Students.Any(s => s.Id == studentId))
        {
            Console.WriteLine($"Enrollment refused: student {studentId} does not exist.");
            return;
        }

        var course = db.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"Enrollment refused: course {courseId} does not exist.");
            return;
        }

        if (!course.IsPublished)
        {
            Console.WriteLine($"Enrollment refused: course '{course.Title}' is not published.");
            return;
        }

        if (db.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId))
        {
            Console.WriteLine($"Enrollment refused: student {studentId} is already enrolled in '{course.Title}'.");
            return;
        }

        db.Enrollments.Add(new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.Now,
            Progress = 0,
            Status = EnrollmentStatus.Active
        });
        db.SaveChanges();

        Console.WriteLine($"Student {studentId} enrolled in '{course.Title}'.");
    }

    private static void FindCourse(int id, string title)
    {
        using var db = new AppDbContext();

        var byId = db.Courses.Find(id);
        Console.WriteLine(byId == null
            ? $"Find({id}): not found"
            : $"Find({id}): {byId.Title} | {byId.Level} | {byId.Price:C}");

        var byTitle = db.Courses.FirstOrDefault(c => c.Title == title);
        Console.WriteLine(byTitle == null
            ? $"Title '{title}': not found"
            : $"Title '{title}': Id = {byTitle.Id} | {byTitle.Level} | {byTitle.Price:C}");
    }

    private static void FilterCoursesByPrice(decimal min, decimal max)
    {
        if (min > max)
        {
            Console.WriteLine($"Invalid range: min ({min}) is greater than max ({max}).");
            return;
        }

        using var db = new AppDbContext();

        var courses = db.Courses
            .Where(c => c.Price >= min && c.Price <= max)
            .OrderByDescending(c => c.Price)
            .Select(c => new { c.Title, c.Level, c.Price })
            .ToList();

        Console.WriteLine($"Courses priced between {min:C} and {max:C}:");
        if (courses.Count == 0)
            Console.WriteLine("  (none)");

        foreach (var c in courses)
            Console.WriteLine($"  {c.Title,-30} {c.Level,-13} {c.Price,8:C}");
    }

    private static void PrintSubCategories(int categoryId)
    {
        using var db = new AppDbContext();

        var parent = db.Categories.Find(categoryId);
        if (parent == null)
        {
            Console.WriteLine($"Category {categoryId} not found.");
            return;
        }

        var subCategories = db.Categories
            .Where(c => c.ParentCategoryId == categoryId)
            .OrderBy(c => c.Name)
            .ToList();

        Console.WriteLine($"Sub-categories of '{parent.Name}':");
        if (subCategories.Count == 0)
            Console.WriteLine("  (none)");

        foreach (var c in subCategories)
            Console.WriteLine($"  [{c.Id}] {c.Name}");
    }

    private static void PrintTopLevelCategories()
    {
        using var db = new AppDbContext();

        var topLevel = db.Categories
            .Where(c => c.ParentCategoryId == null)
            .OrderBy(c => c.Name)
            .ToList();

        Console.WriteLine("Top-level categories:");
        foreach (var c in topLevel)
            Console.WriteLine($"  [{c.Id}] {c.Name}");
    }

    private static void PrintCourseRoster(int courseId)
    {
        using var db = new AppDbContext();

        var course = db.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"Course {courseId} not found.");
            return;
        }

        var studentIds = db.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.StudentId)
            .ToList();

        var students = db.Students
            .Where(s => studentIds.Contains(s.Id))
            .OrderBy(s => s.FullName)
            .ToList();

        Console.WriteLine($"'{course.Title}' has {studentIds.Count} enrolled student(s):");
        foreach (var s in students)
            Console.WriteLine($"  - {s.FullName}");
    }

    private static void ChangeCoursePrice(int courseId, decimal newPrice)
    {
        if (newPrice < 0)
        {
            Console.WriteLine($"Invalid price {newPrice}: price cannot be negative.");
            return;
        }

        using var db = new AppDbContext();

        var course = db.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"Course {courseId} not found.");
            return;
        }

        var oldPrice = course.Price;
        course.Price = newPrice;
        db.SaveChanges();

        Console.WriteLine($"'{course.Title}' price changed from {oldPrice:C} to {newPrice:C}.");
    }

    private static void UpdateProgress(int studentId, int courseId, int progress)
    {
        if (progress < 0 || progress > 100)
        {
            Console.WriteLine($"Rejected: progress must be between 0 and 100 (got {progress}).");
            return;
        }

        using var db = new AppDbContext();

        var enrollment = db.Enrollments.Find(studentId, courseId);
        if (enrollment == null)
        {
            Console.WriteLine($"Enrollment (student {studentId}, course {courseId}) not found.");
            return;
        }

        enrollment.Progress = progress;
        if (progress == 100)
            enrollment.Status = EnrollmentStatus.Completed;

        db.SaveChanges();

        Console.WriteLine($"Enrollment (student {studentId}, course {courseId}): progress = {progress}%, status = {enrollment.Status}.");
    }

    private static void PublishCourse(int courseId)
    {
        using var db = new AppDbContext();

        var course = db.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"Course {courseId} not found.");
            return;
        }

        if (course.IsPublished)
        {
            Console.WriteLine($"'{course.Title}' is already published (since {course.PublishedDate:d}).");
            return;
        }

        course.IsPublished = true;
        course.PublishedDate = DateTime.Now;
        db.SaveChanges();

        Console.WriteLine($"'{course.Title}' published on {course.PublishedDate:g}.");
    }

    private static void Unenroll(int studentId, int courseId)
    {
        using var db = new AppDbContext();

        var enrollment = db.Enrollments.Find(studentId, courseId);
        if (enrollment == null)
        {
            Console.WriteLine($"Enrollment (student {studentId}, course {courseId}) not found.");
            return;
        }

        db.Enrollments.Remove(enrollment);
        db.SaveChanges();

        Console.WriteLine($"Student {studentId} unenrolled from course {courseId}.");
    }

    private static void DeleteCategoryUnsafe(int categoryId)
    {
        using var db = new AppDbContext();

        var category = db.Categories.Find(categoryId);
        if (category == null)
        {
            Console.WriteLine($"Category {categoryId} not found.");
            return;
        }

        try
        {
            db.Categories.Remove(category);
            db.SaveChanges();
            Console.WriteLine($"Category '{category.Name}' deleted.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Could not delete '{category.Name}' (unsafe version):");
            Console.WriteLine($"  {ex.Message}");
            Console.WriteLine($"  {ex.InnerException?.Message}");
        }
    }

    private static void DeleteCategorySafe(int categoryId)
    {
        using var db = new AppDbContext();

        var category = db.Categories.Find(categoryId);
        if (category == null)
        {
            Console.WriteLine($"Category {categoryId} not found.");
            return;
        }

        var subCount = db.Categories.Count(c => c.ParentCategoryId == categoryId);
        if (subCount > 0)
        {
            Console.WriteLine($"Cannot delete '{category.Name}': it still has {subCount} sub-category(ies).");
            return;
        }

        var courseCount = db.Courses.Count(c => c.CategoryId == categoryId);
        if (courseCount > 0)
        {
            Console.WriteLine($"Cannot delete '{category.Name}': it still has {courseCount} course(s).");
            return;
        }

        db.Categories.Remove(category);
        db.SaveChanges();

        Console.WriteLine($"Category '{category.Name}' deleted.");
    }

    private static void DeleteCourse(int courseId)
    {
        using var db = new AppDbContext();

        var course = db.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"Course {courseId} not found.");
            return;
        }

        var before = db.Enrollments.Count(e => e.CourseId == courseId);
        Console.WriteLine($"'{course.Title}' has {before} enrollment(s) before delete.");

        db.Courses.Remove(course);
        db.SaveChanges();

        var after = db.Enrollments.Count(e => e.CourseId == courseId);
        Console.WriteLine($"'{course.Title}' deleted. Enrollments after delete: {after}.");
        Console.WriteLine("They were removed because Enrollment -> Course is configured with DeleteBehavior.Cascade,");
        Console.WriteLine("so SQL Server (ON DELETE CASCADE) deletes every enrollment that points to the deleted course.");
    }

    private static void BulkRaisePrice(CourseLevel level)
    {
        using var db = new AppDbContext();

        var affected = db.Courses
            .Where(c => c.IsPublished && c.Level == level)
            .ExecuteUpdate(s => s.SetProperty(c => c.Price, c => c.Price * 1.10m));

        Console.WriteLine($"Raised the price of published {level} courses by 10%. Rows affected: {affected}.");
    }

    private static void BulkDeleteDroppedEnrollments()
    {
        using var db = new AppDbContext();

        var affected = db.Enrollments
            .Where(e => e.Status == EnrollmentStatus.Dropped)
            .ExecuteDelete();

        Console.WriteLine($"Deleted dropped enrollments. Rows affected: {affected}.");
    }

    private static void SeedStudents(AppDbContext db)
    {
        if (db.Students.Any())
        {
            Console.WriteLine("Students already seeded, skipping.");
            return;
        }

        var students = new List<Student>
        {
            new() { FullName = "Omar Khaled", Email = "omar.khaled@mail.com", DateOfBirth = new DateOnly(2001, 4, 12) },
            new() { FullName = "Mariam Adel", Email = "mariam.adel@mail.com", DateOfBirth = new DateOnly(2000, 9, 3) },
            new()
            {
                FullName = "Youssef Samir", Email = "youssef.samir@mail.com", DateOfBirth = new DateOnly(1999, 1, 25)
            },
            new() { FullName = "Nour Hany", Email = "nour.hany@mail.com", DateOfBirth = new DateOnly(2002, 7, 18) },
            new()
            {
                FullName = "Karim Mostafa", Email = "karim.mostafa@mail.com", DateOfBirth = new DateOnly(1998, 11, 30)
            },
        };

        db.Students.AddRange(students);
        db.SaveChanges();

        Console.WriteLine($"Seeded {students.Count} students.");
    }

    private static void SeedEnrollments(AppDbContext db)
    {
        if (db.Enrollments.Any())
        {
            Console.WriteLine("Enrollments already seeded, skipping.");
            return;
        }

        var ids = db.Students.ToDictionary(s => s.Email, s => s.Id);
        if (ids.Count < 5)
        {
            Console.WriteLine("Not enough students to seed enrollments.");
            return;
        }

        int omar = ids["omar.khaled@mail.com"];
        int mariam = ids["mariam.adel@mail.com"];
        int youssef = ids["youssef.samir@mail.com"];
        int nour = ids["nour.hany@mail.com"];
        int karim = ids["karim.mostafa@mail.com"];

        var enrollments = new List<Enrollment>
        {
            new()
            {
                StudentId = omar, CourseId = 1, EnrolledAt = new DateTime(2025, 4, 1), Progress = 40,
                Status = EnrollmentStatus.Active
            },
            new()
            {
                StudentId = mariam, CourseId = 4, EnrolledAt = new DateTime(2025, 5, 10), Progress = 65,
                Status = EnrollmentStatus.Active
            },
            new()
            {
                StudentId = nour, CourseId = 6, EnrolledAt = new DateTime(2025, 6, 2), Progress = 10,
                Status = EnrollmentStatus.Active
            },

            new()
            {
                StudentId = omar, CourseId = 2, EnrolledAt = new DateTime(2025, 2, 15), Progress = 100,
                Status = EnrollmentStatus.Completed, FinalGrade = 92.50m
            },
            new()
            {
                StudentId = youssef, CourseId = 1, EnrolledAt = new DateTime(2025, 3, 20), Progress = 100,
                Status = EnrollmentStatus.Completed, FinalGrade = 85.00m
            },
            new()
            {
                StudentId = karim, CourseId = 2, EnrolledAt = new DateTime(2025, 2, 1), Progress = 100,
                Status = EnrollmentStatus.Completed, FinalGrade = 78.25m
            },

            new()
            {
                StudentId = mariam, CourseId = 2, EnrolledAt = new DateTime(2025, 3, 5), Progress = 20,
                Status = EnrollmentStatus.Dropped
            },
            new()
            {
                StudentId = karim, CourseId = 6, EnrolledAt = new DateTime(2025, 6, 15), Progress = 5,
                Status = EnrollmentStatus.Dropped
            },
        };

        db.Enrollments.AddRange(enrollments);
        db.SaveChanges();

        Console.WriteLine($"Seeded {enrollments.Count} enrollments.");
    }
}
