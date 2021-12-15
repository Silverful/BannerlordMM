using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class CalibrationIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(5145),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 372, DateTimeKind.Utc).AddTicks(9325));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(6122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 373, DateTimeKind.Utc).AddTicks(436));

            migrationBuilder.AddColumn<byte>(
                name: "CalibrationIndex",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 613, DateTimeKind.Utc).AddTicks(7142),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 367, DateTimeKind.Utc).AddTicks(6878));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalibrationIndex",
                table: "PlayerMatchRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 372, DateTimeKind.Utc).AddTicks(9325),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(5145));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PlayerMatchRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 373, DateTimeKind.Utc).AddTicks(436),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 619, DateTimeKind.Utc).AddTicks(6122));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 12, 13, 19, 54, 46, 367, DateTimeKind.Utc).AddTicks(6878),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 12, 15, 10, 51, 50, 613, DateTimeKind.Utc).AddTicks(7142));
        }
    }
}
