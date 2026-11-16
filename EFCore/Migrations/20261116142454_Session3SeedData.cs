using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Session3SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, "Software development courses", "Programming", null },
                    { 2, "Visual and product design", "Design", null }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Bio", "Email", "FullName", "JoinedAt" },
                values: new object[,]
                {
                    { 1, "Senior .NET developer, 10 years of backend work.", "ahmed.fathy@learn.com", "Ahmed Fathy", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Full-stack engineer and React trainer.", "sara.mahmoud@learn.com", "Sara Mahmoud", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, null, "hassan.ali@learn.com", "Hassan Ali", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 3, "Server-side development", "Backend", 1 },
                    { 4, "Client-side development", "Frontend", 1 },
                    { 6, "User interface & experience", "UI/UX", 2 },
                    { 5, null, "ASP.NET", 3 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 2, 3, "Start programming with C#.", 1, true, "Beginner", 19.99m, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# Fundamentals" },
                    { 4, 4, "Components, hooks and state.", 2, true, "Beginner", 29.99m, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "React from Scratch" },
                    { 5, 4, "Flexbox and Grid in depth.", 3, false, "Intermediate", 24.99m, null, "Modern CSS Layouts" },
                    { 6, 6, "Design usable interfaces.", 3, true, "Advanced", 59.99m, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "UI/UX Design Principles" },
                    { 1, 5, "Build REST APIs with ASP.NET Core.", 1, true, "Intermediate", 49.99m, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core Web API" },
                    { 3, 5, null, 2, false, "Advanced", 79.99m, null, "Advanced EF Core" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
