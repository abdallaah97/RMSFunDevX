using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 21, 15, 59, 31, 15, DateTimeKind.Utc).AddTicks(2182), "Issues related to physical components of computers and devices.", "Hardware" },
                    { 2, new DateTime(2026, 5, 21, 15, 59, 31, 15, DateTimeKind.Utc).AddTicks(2857), "Issues related to applications, operating systems, and software functionality.", "Software" },
                    { 3, new DateTime(2026, 5, 21, 15, 59, 31, 15, DateTimeKind.Utc).AddTicks(2860), "Issues related to connectivity, internet access, and network performance.", "Network" },
                    { 4, new DateTime(2026, 5, 21, 15, 59, 31, 15, DateTimeKind.Utc).AddTicks(2862), "Issues related to cybersecurity threats, data breaches, and security vulnerabilities.", "Security" },
                    { 5, new DateTime(2026, 5, 21, 15, 59, 31, 15, DateTimeKind.Utc).AddTicks(2865), "Miscellaneous issues that do not fit into the above categories.", "Other" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

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
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
