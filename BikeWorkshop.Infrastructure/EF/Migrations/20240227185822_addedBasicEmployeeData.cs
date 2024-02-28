using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BikeWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedBasicEmployeeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "RoleId" },
                values: new object[,]
                {
                    { new Guid("21caf1e5-f03d-4fe0-813f-cfdabfcee001"), "worker@worker.com", "Worker", "Worker", "AQAAAAIAAYagAAAAENf6MeJeKToBlBafVogukJq1Dwhv/ETYpcCT1iDnYGFMji1FLEsCJYlpoZrjdaQF1Q==", 2 },
                    { new Guid("71b87f38-96ad-464e-b1de-a66b06ff4448"), "admin@admin.com", "Admin", "Admin", "AQAAAAIAAYagAAAAELyDAG6Teyf7vh7oGWEGtA/iHhkl1uRgLmsNpF40XFCEwjPBgWUIGfYLShRpZuDCKA==", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("21caf1e5-f03d-4fe0-813f-cfdabfcee001"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("71b87f38-96ad-464e-b1de-a66b06ff4448"));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
