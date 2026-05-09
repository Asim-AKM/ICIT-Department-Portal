using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class updateAnnouncementEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedBy",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Announcements",
                newName: "Message");

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementTargetAudience",
                table: "Announcements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementType",
                table: "Announcements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementTargetAudience",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "AnnouncementType",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Announcements",
                newName: "Content");

            migrationBuilder.AddColumn<Guid>(
                name: "PostedBy",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
