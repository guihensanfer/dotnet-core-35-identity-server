using Microsoft.EntityFrameworkCore.Migrations;

namespace Bom_Dev.Shared.Migrations
{
    public partial class CategoryUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Category",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Category");
        }
    }
}
