using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class TypesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 11, 4, 0, 91, DateTimeKind.Utc).AddTicks(8346),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(5145));

            migrationBuilder.AlterColumn<short>(
                name: "Kills",
                table: "PlayerMatchRecords",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 11, 4, 0, 91, DateTimeKind.Utc).AddTicks(9527),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(6122));

            migrationBuilder.AlterColumn<short>(
                name: "Assists",
                table: "PlayerMatchRecords",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 11, 4, 0, 85, DateTimeKind.Utc).AddTicks(7328),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 613, DateTimeKind.Utc).AddTicks(7142));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(5145),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 11, 4, 0, 91, DateTimeKind.Utc).AddTicks(8346));

            migrationBuilder.AlterColumn<byte>(
                name: "Kills",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(6122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 11, 4, 0, 91, DateTimeKind.Utc).AddTicks(9527));

            migrationBuilder.AlterColumn<byte>(
                name: "Assists",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 613, DateTimeKind.Utc).AddTicks(7142),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 11, 4, 0, 85, DateTimeKind.Utc).AddTicks(7328));
        }
    }
}
