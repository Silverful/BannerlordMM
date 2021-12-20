using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class NullableTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchRecords_Players_PlayerId",
                schema: "dbo",
                table: "PlayerMatchRecords");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "MMRChange",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Faction",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "CalibrationIndex",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchRecords_Players_PlayerId",
                schema: "dbo",
                table: "PlayerMatchRecords",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchRecords_Players_PlayerId",
                schema: "dbo",
                table: "PlayerMatchRecords");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MMRChange",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Faction",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "CalibrationIndex",
                schema: "dbo",
                table: "PlayerMatchRecords",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchRecords_Players_PlayerId",
                schema: "dbo",
                table: "PlayerMatchRecords",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
