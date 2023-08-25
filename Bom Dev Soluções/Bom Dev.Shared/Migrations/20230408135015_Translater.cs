using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Translater : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CacheObject",
                maxLength: 15,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TranslationObject",
                columns: table => new
                {
                    TranslationObjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectGuid = table.Column<Guid>(nullable: false),
                    Language = table.Column<string>(maxLength: 15, nullable: true),
                    Value = table.Column<string>(maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslateObject", x => x.TranslationObjectId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationObject");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CacheObject");
        }
    }
}
