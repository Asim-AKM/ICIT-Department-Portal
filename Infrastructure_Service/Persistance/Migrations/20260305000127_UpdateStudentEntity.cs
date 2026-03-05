using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentEmail",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentEmail",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "Students");
        }
    }
}
