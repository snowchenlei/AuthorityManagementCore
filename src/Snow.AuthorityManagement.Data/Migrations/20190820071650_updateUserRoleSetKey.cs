using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snow.AuthorityManagement.Data.Migrations
{
    public partial class updateUserRoleSetKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "AddTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AddTime",
                table: "Role");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Permission",
                newName: "ID");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "UserRole",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserID",
                table: "UserRole",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_UserID",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Permission",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddTime",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddTime",
                table: "Role",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                columns: new[] { "UserID", "RoleID" });
        }
    }
}
