using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserManagementEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RollNumber",
                table: "Students",
                newName: "RollNo");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Students",
                newName: "RegistrationNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RollNo",
                table: "Students",
                newName: "RollNumber");

            migrationBuilder.RenameColumn(
                name: "RegistrationNo",
                table: "Students",
                newName: "RegistrationNumber");
        }
    }
}
