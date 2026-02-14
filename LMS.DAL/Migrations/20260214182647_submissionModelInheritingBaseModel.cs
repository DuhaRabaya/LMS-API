using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class submissionModelInheritingBaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Submissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Submissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Submissions");
        }
    }
}
