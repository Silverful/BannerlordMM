using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class MatchesUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 372, DateTimeKind.Utc).AddTicks(9325),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(999));

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "MVPs",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Kills",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Deaths",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 373, DateTimeKind.Utc).AddTicks(436),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(2556));

            migrationBuilder.AlterColumn<byte>(
                name: "Assists",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "RoundsPlayed",
                table: "Matches",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 367, DateTimeKind.Utc).AddTicks(6878),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 665, DateTimeKind.Utc).AddTicks(7223));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(999),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 372, DateTimeKind.Utc).AddTicks(9325));

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MVPs",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Kills",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Deaths",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 674, DateTimeKind.Utc).AddTicks(2556),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 373, DateTimeKind.Utc).AddTicks(436));

            migrationBuilder.AlterColumn<int>(
                name: "Assists",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoundsPlayed",
                table: "Matches",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 17, 9, 14, 665, DateTimeKind.Utc).AddTicks(7223),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 367, DateTimeKind.Utc).AddTicks(6878));
        }
    }
}
