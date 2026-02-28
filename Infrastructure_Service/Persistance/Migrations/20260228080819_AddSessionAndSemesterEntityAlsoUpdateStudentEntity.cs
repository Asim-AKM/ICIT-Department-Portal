using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionAndSemesterEntityAlsoUpdateStudentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SemesterId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StudentSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndYear = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSessions", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentSessionSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                    table.ForeignKey(
                        name: "FK_Semesters_StudentSessions_StudentSessionSessionId",
                        column: x => x.StudentSessionSessionId,
                        principalTable: "StudentSessions",
                        principalColumn: "SessionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SemesterId",
                table: "Students",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_StudentSessionSessionId",
                table: "Semesters",
                column: "StudentSessionSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Semesters_SemesterId",
                table: "Students",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Semesters_SemesterId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "StudentSessions");

            migrationBuilder.DropIndex(
                name: "IX_Students_SemesterId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Students");
        }
    }
}
