using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class MMRTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerMMR",
                schema: "dbo",
                table: "Players");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerMMRId",
                schema: "dbo",
                table: "Players",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PlayerMMR",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MMR = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMMR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMMR_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalSchema: "dbo",
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerMMRId",
                schema: "dbo",
                table: "Players",
                column: "PlayerMMRId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMMR_SeasonId",
                schema: "dbo",
                table: "PlayerMMR",
                column: "SeasonId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerMMR_PlayerMMRId",
                schema: "dbo",
                table: "Players");

            migrationBuilder.DropTable(
                name: "PlayerMMR",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerMMRId",
                schema: "dbo",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerMMRId",
                schema: "dbo",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "PlayerMMR",
                schema: "dbo",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
