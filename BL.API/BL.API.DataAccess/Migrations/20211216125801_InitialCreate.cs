using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreenshotLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoundsPlayed = table.Column<byte>(type: "tinyint", nullable: false),
                    TeamWon = table.Column<byte>(type: "tinyint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 12, 16, 12, 58, 1, 7, DateTimeKind.Utc).AddTicks(9353))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Logged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logger = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Callsite = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Clan = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    MainClass = table.Column<int>(type: "int", nullable: false),
                    SecondaryClass = table.Column<int>(type: "int", nullable: false),
                    DiscordId = table.Column<int>(type: "int", nullable: false),
                    PlayerMMR = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 12, 16, 12, 58, 1, 13, DateTimeKind.Utc).AddTicks(887))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMatchRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamIndex = table.Column<byte>(type: "tinyint", nullable: false),
                    RoundsWon = table.Column<byte>(type: "tinyint", nullable: false),
                    Faction = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<short>(type: "smallint", nullable: true),
                    Assists = table.Column<short>(type: "smallint", nullable: true),
                    Deaths = table.Column<byte>(type: "tinyint", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    MVPs = table.Column<byte>(type: "tinyint", nullable: true),
                    MMRChange = table.Column<int>(type: "int", nullable: false),
                    CalibrationIndex = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 12, 16, 12, 58, 1, 13, DateTimeKind.Utc).AddTicks(1977))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatchRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMatchRecords_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMatchRecords_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchRecords_MatchId",
                table: "PlayerMatchRecords",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchRecords_PlayerId",
                table: "PlayerMatchRecords",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NLog");

            migrationBuilder.DropTable(
                name: "PlayerMatchRecords");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
