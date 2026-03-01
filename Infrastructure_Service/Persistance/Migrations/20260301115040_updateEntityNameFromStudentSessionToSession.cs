using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class updateEntityNameFromStudentSessionToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_StudentSessions_SessionId",
                table: "Semesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSessions",
                table: "StudentSessions");

            migrationBuilder.RenameTable(
                name: "StudentSessions",
                newName: "Sessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_Sessions_SessionId",
                table: "Semesters",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_Sessions_SessionId",
                table: "Semesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "Sessions",
                newName: "StudentSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSessions",
                table: "StudentSessions",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_StudentSessions_SessionId",
                table: "Semesters",
                column: "SessionId",
                principalTable: "StudentSessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
