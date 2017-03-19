using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace foodynet_api_anais.Migrations
{
    public partial class userRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Admin",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "User");
        }
    }
}
