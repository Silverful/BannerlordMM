using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class ScreenshotLinkIndexAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Matches_ScreenshotLink",
                schema: "dbo",
                table: "Matches",
                column: "ScreenshotLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Matches_ScreenshotLink",
                schema: "dbo",
                table: "Matches");
        }
    }
}
