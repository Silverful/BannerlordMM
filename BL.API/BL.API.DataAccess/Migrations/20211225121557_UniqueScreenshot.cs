using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class UniqueScreenshot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Matches_ScreenshotLink",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ScreenshotLink",
                schema: "dbo",
                table: "Matches",
                column: "ScreenshotLink",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Matches_ScreenshotLink",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ScreenshotLink",
                schema: "dbo",
                table: "Matches",
                column: "ScreenshotLink");
        }
    }
}
