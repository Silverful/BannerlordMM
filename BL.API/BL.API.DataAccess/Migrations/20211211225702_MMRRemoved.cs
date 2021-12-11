using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class MMRRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerMMR_PlayerMMRId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "PlayerMMR");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerMMRId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerMMRId",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "PlayerMMR",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerMMR",
                table: "Players");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerMMRId",
                table: "Players",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PlayerMMR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MMR = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMMR", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerMMRId",
                table: "Players",
                column: "PlayerMMRId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerMMR_PlayerMMRId",
                table: "Players",
                column: "PlayerMMRId",
                principalTable: "PlayerMMR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
