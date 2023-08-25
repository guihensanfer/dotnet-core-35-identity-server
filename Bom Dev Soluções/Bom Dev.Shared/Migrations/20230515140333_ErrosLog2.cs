using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ErrosLog2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    ErrorLogsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    DateExpiration = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Message = table.Column<string>(maxLength: 255, nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    RequestMethod = table.Column<string>(maxLength: 10, nullable: true),
                    RequestUrl = table.Column<string>(maxLength: 255, nullable: true),
                    UserAgent = table.Column<string>(maxLength: 255, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 45, nullable: true),
                    Language = table.Column<string>(maxLength: 15, nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.ErrorLogsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");
        }
    }
}
