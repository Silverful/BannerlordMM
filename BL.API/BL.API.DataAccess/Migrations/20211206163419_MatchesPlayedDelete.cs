using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class MatchesPlayedDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundsPlayed",
                table: "PlayerMatchRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundsPlayed",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
