using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class CategoryIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Index",
                table: "Category",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Category");
        }
    }
}
