using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class SeasonsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeasonId",
                schema: "dbo",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Seasons",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnGoing = table.Column<bool>(type: "bit", nullable: false),
                    Started = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Finished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SeasonId",
                schema: "dbo",
                table: "Matches",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Seasons_SeasonId",
                schema: "dbo",
                table: "Matches",
                column: "SeasonId",
                principalSchema: "dbo",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            var guid = Guid.NewGuid();
            var sqlSeason = $"INSERT INTO dbo.Seasons (Id, Title, OnGoing, Started, Finished, Created) VALUES ('{guid}', 'Beta', 1, '2021-11-14', NULL, '2021-12-22')";
            migrationBuilder.Sql(sqlSeason);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Seasons_SeasonId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Seasons",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Matches_SeasonId",
                schema: "dbo",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                schema: "dbo",
                table: "Matches");
        }
    }
}
