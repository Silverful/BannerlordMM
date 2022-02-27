using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class RegionsAddedToConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMMR_Region_RegionId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                schema: "dbo",
                table: "PlayerMMR",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                schema: "dbo",
                table: "Configurations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_RegionId",
                schema: "dbo",
                table: "Configurations",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configurations_Region_RegionId",
                schema: "dbo",
                table: "Configurations",
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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configurations_Region_RegionId",
                schema: "dbo",
                table: "Configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMMR_Region_RegionId",
                schema: "dbo",
                table: "PlayerMMR");

            migrationBuilder.DropIndex(
                name: "IX_Configurations_RegionId",
                schema: "dbo",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "dbo",
                table: "Configurations");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                schema: "dbo",
                table: "PlayerMMR",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMMR_Region_RegionId",
                schema: "dbo",
                table: "PlayerMMR",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
