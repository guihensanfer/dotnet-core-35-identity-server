using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Translater2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslateObject",
                table: "TranslateObject");

            migrationBuilder.RenameTable(
                name: "TranslateObject",
                newName: "TranslationObject");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslationObject",
                table: "TranslationObject",
                column: "TranslationObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslationObject",
                table: "TranslationObject");

            migrationBuilder.RenameTable(
                name: "TranslationObject",
                newName: "TranslateObject");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslateObject",
                table: "TranslateObject",
                column: "TranslationObjectId");
        }
    }
}
