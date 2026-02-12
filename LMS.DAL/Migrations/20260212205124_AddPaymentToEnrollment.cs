using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefunded",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRefunded",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Enrollments");
        }
    }
}
