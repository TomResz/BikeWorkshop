using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedRetrievedDateToSummaryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RetrievedDate",
                table: "Summaries",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetrievedDate",
                table: "Summaries");
        }
    }
}
