using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snow.AuthorityManagement.Data.Migrations
{
    public partial class AddMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PermissionName = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Route = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    ParentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menu_Menu_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ParentID",
                table: "Menu",
                column: "ParentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");
        }
    }
}
