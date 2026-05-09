using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Service.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class NotificationEntityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Clerks");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Announcements",
                newName: "PostedBy");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "AnnouncementId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBroadcast",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SenderUserId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetRole",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetUserId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Clerks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendMailNotification",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsBroadcast",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SenderUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetRole",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TargetUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Clerks");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "SendMailNotification",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "PostedBy",
                table: "Announcements",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Clerks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
