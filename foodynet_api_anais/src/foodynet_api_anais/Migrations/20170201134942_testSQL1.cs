using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace foodynet_api_anais.Migrations
{
    public partial class testSQL1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Ingredient = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Time = table.Column<int>(nullable: false),
                    cooking = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipe");
        }
    }
}
