using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class FollowUpMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerMMR_PlayerMMRId",
                schema: "dbo",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerMMRId",
                schema: "dbo",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerMMRId",
                schema: "dbo",
                table: "Players");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMMR_PlayerId",
                schema: "dbo",
                table: "PlayerMMR",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMMR_Players_PlayerId",
                schema: "dbo",
                table: "PlayerMMR",
                column: "PlayerId",
                principalSchema: "dbo",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMMR_Players_PlayerId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.DropIndex(
                name: "IX_PlayerMMR_PlayerId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerMMRId",
                schema: "dbo",
                table: "Players",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerMMRId",
                schema: "dbo",
                table: "Players",
                column: "PlayerMMRId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerMMR_PlayerMMRId",
                schema: "dbo",
                table: "Players",
                column: "PlayerMMRId",
                principalSchema: "dbo",
                principalTable: "PlayerMMR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
