using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BikeWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedRefreshtokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("21caf1e5-f03d-4fe0-813f-cfdabfcee001"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("71b87f38-96ad-464e-b1de-a66b06ff4448"));

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "RoleId" },
                values: new object[,]
                {
                    { new Guid("1c07d719-7a0c-4e72-81fb-ec895e80efd6"), "worker@worker.com", "Worker", "Worker", "AQAAAAIAAYagAAAAENnr2Q/FTHf/c4+G8PNq28AtyRIMEGDE8o/HCoXgHDfVScyZ8Mvu/R+YV/VXiioWUQ==", 2 },
                    { new Guid("ed7a659c-9081-44b0-801c-5afb3b100995"), "admin@admin.com", "Admin", "Admin", "AQAAAAIAAYagAAAAENZu1P7EAnpx2Eaf5PbZklpP7/u7/TYrC90Hmqa0TUM/Pw7y5pqoFEAHD3V/OtRHOg==", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_EmployeeId",
                table: "RefreshTokens",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("1c07d719-7a0c-4e72-81fb-ec895e80efd6"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("ed7a659c-9081-44b0-801c-5afb3b100995"));

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "RoleId" },
                values: new object[,]
                {
                    { new Guid("21caf1e5-f03d-4fe0-813f-cfdabfcee001"), "worker@worker.com", "Worker", "Worker", "AQAAAAIAAYagAAAAENf6MeJeKToBlBafVogukJq1Dwhv/ETYpcCT1iDnYGFMji1FLEsCJYlpoZrjdaQF1Q==", 2 },
                    { new Guid("71b87f38-96ad-464e-b1de-a66b06ff4448"), "admin@admin.com", "Admin", "Admin", "AQAAAAIAAYagAAAAELyDAG6Teyf7vh7oGWEGtA/iHhkl1uRgLmsNpF40XFCEwjPBgWUIGfYLShRpZuDCKA==", 1 }
                });
        }
    }
}
