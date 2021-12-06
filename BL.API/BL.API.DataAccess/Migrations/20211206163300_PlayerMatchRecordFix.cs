using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class PlayerMatchRecordFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchRecord_Matches_MatchId",
                table: "PlayerMatchRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchRecord_Players_PlayerId",
                table: "PlayerMatchRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerMatchRecord",
                table: "PlayerMatchRecord");

            migrationBuilder.RenameTable(
                name: "PlayerMatchRecord",
                newName: "PlayerMatchRecords");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerMatchRecord_PlayerId",
                table: "PlayerMatchRecords",
                newName: "IX_PlayerMatchRecords_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerMatchRecord_MatchId",
                table: "PlayerMatchRecords",
                newName: "IX_PlayerMatchRecords_MatchId");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "PlayerMatchRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Faction",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoundsPlayed",
                table: "PlayerMatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerMatchRecords",
                table: "PlayerMatchRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchRecords_Matches_MatchId",
                table: "PlayerMatchRecords",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchRecords_Players_PlayerId",
                table: "PlayerMatchRecords",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchRecords_Matches_MatchId",
                table: "PlayerMatchRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchRecords_Players_PlayerId",
                table: "PlayerMatchRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerMatchRecords",
                table: "PlayerMatchRecords");

            migrationBuilder.DropColumn(
                name: "Faction",
                table: "PlayerMatchRecords");

            migrationBuilder.DropColumn(
                name: "RoundsPlayed",
                table: "PlayerMatchRecords");

            migrationBuilder.RenameTable(
                name: "PlayerMatchRecords",
                newName: "PlayerMatchRecord");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerMatchRecords_PlayerId",
                table: "PlayerMatchRecord",
                newName: "IX_PlayerMatchRecord_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerMatchRecords_MatchId",
                table: "PlayerMatchRecord",
                newName: "IX_PlayerMatchRecord_MatchId");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "PlayerMatchRecord",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerMatchRecord",
                table: "PlayerMatchRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchRecord_Matches_MatchId",
                table: "PlayerMatchRecord",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchRecord_Players_PlayerId",
                table: "PlayerMatchRecord",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
