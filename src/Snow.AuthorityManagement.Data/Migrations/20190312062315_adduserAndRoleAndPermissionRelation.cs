using Microsoft.EntityFrameworkCore.Migrations;

namespace Snow.AuthorityManagement.Data.Migrations
{
    public partial class adduserAndRoleAndPermissionRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "Permission",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_RoleID",
                table: "Permission",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Role_RoleID",
                table: "Permission",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Role_RoleID",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_RoleID",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Permission");
        }
    }
}
