using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class SchemaNLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Players",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PlayerMatchRecords",
                newName: "PlayerMatchRecords",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "NLog",
                newName: "NLog",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matches",
                newSchema: "dbo");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Players",
                schema: "dbo",
                newName: "Players");

            migrationBuilder.RenameTable(
                name: "PlayerMatchRecords",
                schema: "dbo",
                newName: "PlayerMatchRecords");

            migrationBuilder.RenameTable(
                name: "NLog",
                schema: "dbo",
                newName: "NLog");

            migrationBuilder.RenameTable(
                name: "Matches",
                schema: "dbo",
                newName: "Matches");
        }
    }
}
