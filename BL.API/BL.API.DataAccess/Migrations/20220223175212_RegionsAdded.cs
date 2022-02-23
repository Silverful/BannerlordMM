using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class RegionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                schema: "dbo",
                table: "Seasons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                schema: "dbo",
                table: "PlayerMMR",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                schema: "dbo",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Region",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_RegionId",
                schema: "dbo",
                table: "Seasons",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMMR_RegionId",
                schema: "dbo",
                table: "PlayerMMR",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RegionId",
                schema: "dbo",
                table: "Matches",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Region_RegionId",
                schema: "dbo",
                table: "Matches",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMMR_Region_RegionId",
                schema: "dbo",
                table: "PlayerMMR",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Region_RegionId",
                schema: "dbo",
                table: "Seasons",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Region_RegionId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMMR_Region_RegionId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Region_RegionId",
                schema: "dbo",
                table: "Seasons");

            migrationBuilder.DropTable(
                name: "Region",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_RegionId",
                schema: "dbo",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_PlayerMMR_RegionId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.DropIndex(
                name: "IX_Matches_RegionId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "dbo",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "dbo",
                table: "Matches");
        }
    }
}
