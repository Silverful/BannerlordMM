using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class RegionCreatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "dbo",
                table: "Region",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "dbo",
                table: "Region",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");
        }
    }
}
