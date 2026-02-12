using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeIsRefundedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRefunded",
                table: "Enrollments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefunded",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
