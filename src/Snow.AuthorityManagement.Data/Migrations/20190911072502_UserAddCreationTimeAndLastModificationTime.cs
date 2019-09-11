﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snow.AuthorityManagement.Data.Migrations
{
    public partial class UserAddCreationTimeAndLastModificationTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "User");
        }
    }
}
