using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSemeterAndSessionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Semesters_SemesterId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_SemesterId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StartYear",
                table: "Sessions",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndYear",
                table: "Sessions",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Semesters",
                newName: "Order");

            migrationBuilder.AddColumn<int>(
                name: "AcademicYear",
                table: "Semesters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Semesters");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Sessions",
                newName: "StartYear");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Sessions",
                newName: "EndYear");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Semesters",
                newName: "Year");

            migrationBuilder.AddColumn<Guid>(
                name: "SemesterId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SemesterId",
                table: "Students",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Semesters_SemesterId",
                table: "Students",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId");
        }
    }
}
