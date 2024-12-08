using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MD3Marcis.Migrations
{
    /// <inheritdoc />
    public partial class Test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Gender", "Name", "StudentIdNumber", "Surname" },
                values: new object[,]
                {
                    { 1, 1, "Elza", 3001, "Ziediņa" },
                    { 2, 0, "Artis", 3002, "Balodis" },
                    { 3, 1, "Rūta", 3003, "Lāce" },
                    { 4, 0, "Edgars", 3004, "Bērziņš" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "ContractDate", "Gender", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Laura", "Kļaviņa" },
                    { 2, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Mārtiņš", "Rozītis" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 1, "Programmēšana", 1 },
                    { 2, "Datu bāzes", 2 }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "CourseId", "Deadline", "Description" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python" },
                    { 2, 2, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SQL skripti" }
                });

            migrationBuilder.InsertData(
                table: "Submissions",
                columns: new[] { "Id", "AssignmentId", "Score", "StudentId", "SubmissionTime" },
                values: new object[,]
                {
                    { 3, 1, 90, 1, new DateTime(2024, 3, 1, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, 75, 3, new DateTime(2024, 3, 1, 12, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 2, 85, 2, new DateTime(2024, 4, 15, 11, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 2, 80, 4, new DateTime(2024, 4, 15, 14, 20, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Submissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
