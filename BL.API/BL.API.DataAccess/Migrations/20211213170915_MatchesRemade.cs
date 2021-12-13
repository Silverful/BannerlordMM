using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class MatchesRemade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FactionWon",
                table: "Matches");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(999),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 13, 41, 52, 386, DateTimeKind.Utc).AddTicks(7577));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(2556),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 13, 41, 52, 386, DateTimeKind.Utc).AddTicks(9156));

            migrationBuilder.AddColumn<byte>(
                name: "RoundsWon",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "TeamIndex",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 665, DateTimeKind.Utc).AddTicks(7223),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 13, 41, 52, 379, DateTimeKind.Utc).AddTicks(9793));

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchDate",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "TeamWon",
                table: "Matches",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundsWon",
                table: "PlayerMatchRecords");

            migrationBuilder.DropColumn(
                name: "TeamIndex",
                table: "PlayerMatchRecords");

            migrationBuilder.DropColumn(
                name: "MatchDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamWon",
                table: "Matches");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 13, 41, 52, 386, DateTimeKind.Utc).AddTicks(7577),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(999));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 13, 41, 52, 386, DateTimeKind.Utc).AddTicks(9156),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(2556));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 13, 41, 52, 379, DateTimeKind.Utc).AddTicks(9793),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 665, DateTimeKind.Utc).AddTicks(7223));

            migrationBuilder.AddColumn<int>(
                name: "FactionWon",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
