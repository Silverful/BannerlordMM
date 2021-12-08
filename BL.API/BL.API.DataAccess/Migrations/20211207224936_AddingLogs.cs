using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class AddingLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Logged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logger = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Callsite = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NLog", x => x.ID);
                });

            var sp = @"CREATE PROCEDURE [dbo].[NLog_AddEntry_p] (
                          @machineName nvarchar(200),
                          @logged datetime,
                          @level varchar(5),
                          @message nvarchar(max),
                          @logger nvarchar(300),
                          @properties nvarchar(max),
                          @callsite nvarchar(300),
                          @exception nvarchar(max)
                        ) AS
                        BEGIN
                          INSERT INTO [dbo].[NLog] (
                            [MachineName],
                            [Logged],
                            [Level],
                            [Message],
                            [Logger],
                            [Properties],
                            [Callsite],
                            [Exception]
                          ) VALUES (
                            @machineName,
                            @logged,
                            @level,
                            @message,
                            @logger,
                            @properties,
                            @callsite,
                            @exception
                          );
                        END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NLog");
        }
    }
}
