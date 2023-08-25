using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class CategoryParentToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentCategoryIdFk",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_ParentCategoryIdFk",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ParentCategoryIdFk",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryIdFk",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryIdFk",
                table: "Category",
                column: "ParentCategoryIdFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentCategoryIdFk",
                table: "Category",
                column: "ParentCategoryIdFk",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
