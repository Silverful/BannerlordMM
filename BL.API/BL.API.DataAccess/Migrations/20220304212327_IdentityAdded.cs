using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BL.API.DataAccess.Migrations
{
    public partial class IdentityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                schema: "dbo",
                table: "IdentityRoleClaim<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserClaim<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserClaim<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserLogin<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserLogin<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                schema: "dbo",
                table: "IdentityUserRole<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserRole<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserToken<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserToken<string>");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_TempId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_TempId1",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_TempId2",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_TempId3",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_IdentityRole_TempId",
                schema: "dbo",
                table: "IdentityRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_IdentityRole_TempId1",
                schema: "dbo",
                table: "IdentityRole");

            migrationBuilder.DropColumn(
                name: "TempId",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TempId1",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TempId2",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TempId",
                schema: "dbo",
                table: "IdentityRole");

            migrationBuilder.DropColumn(
                name: "TempId1",
                schema: "dbo",
                table: "IdentityRole");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "dbo",
                newName: "AspNetUsers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken<string>",
                schema: "dbo",
                newName: "AspNetUserTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole<string>",
                schema: "dbo",
                newName: "AspNetUserRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin<string>",
                schema: "dbo",
                newName: "AspNetUserLogins",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<string>",
                schema: "dbo",
                newName: "AspNetUserClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim<string>",
                schema: "dbo",
                newName: "AspNetRoleClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                schema: "dbo",
                newName: "AspNetRoles",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "TempId3",
                schema: "dbo",
                table: "AspNetUsers",
                newName: "AccessFailedCount");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "dbo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                schema: "dbo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                schema: "dbo",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "dbo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                schema: "dbo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginProvider",
                schema: "dbo",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                schema: "dbo",
                table: "AspNetUserTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginProvider",
                schema: "dbo",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderKey",
                schema: "dbo",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderDisplayName",
                schema: "dbo",
                table: "AspNetUserLogins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "AspNetUserClaims",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                schema: "dbo",
                table: "AspNetUserClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                schema: "dbo",
                table: "AspNetUserClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "AspNetRoleClaims",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                schema: "dbo",
                table: "AspNetRoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                schema: "dbo",
                table: "AspNetRoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                schema: "dbo",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "dbo",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                schema: "dbo",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "dbo",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "dbo",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "dbo",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                schema: "dbo",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "dbo",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "dbo",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "dbo",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "dbo",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                schema: "dbo",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                schema: "dbo",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                schema: "dbo",
                table: "AspNetRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "LoginProvider",
                schema: "dbo",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "dbo",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LoginProvider",
                schema: "dbo",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "ProviderKey",
                schema: "dbo",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "ProviderDisplayName",
                schema: "dbo",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "ClaimType",
                schema: "dbo",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                schema: "dbo",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "ClaimType",
                schema: "dbo",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                schema: "dbo",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                newName: "IdentityUserToken<string>",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "dbo",
                newName: "User",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                newName: "IdentityUserRole<string>",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                newName: "IdentityUserLogin<string>",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                newName: "IdentityUserClaim<string>",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "dbo",
                newName: "IdentityRole",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                newName: "IdentityRoleClaim<string>",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                schema: "dbo",
                table: "User",
                newName: "TempId3");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "IdentityUserToken<string>",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempId1",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempId2",
                schema: "dbo",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                schema: "dbo",
                table: "IdentityUserRole<string>",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "IdentityUserRole<string>",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "IdentityUserLogin<string>",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "IdentityUserClaim<string>",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                schema: "dbo",
                table: "IdentityRole",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempId1",
                schema: "dbo",
                table: "IdentityRole",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                schema: "dbo",
                table: "IdentityRoleClaim<string>",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_TempId",
                schema: "dbo",
                table: "User",
                column: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_TempId1",
                schema: "dbo",
                table: "User",
                column: "TempId1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_TempId2",
                schema: "dbo",
                table: "User",
                column: "TempId2");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_TempId3",
                schema: "dbo",
                table: "User",
                column: "TempId3");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_IdentityRole_TempId",
                schema: "dbo",
                table: "IdentityRole",
                column: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_IdentityRole_TempId1",
                schema: "dbo",
                table: "IdentityRole",
                column: "TempId1");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                schema: "dbo",
                table: "IdentityRoleClaim<string>",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "IdentityRole",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserClaim<string>",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserLogin<string>",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                schema: "dbo",
                table: "IdentityUserRole<string>",
                column: "RoleId",
                principalSchema: "dbo",
                principalTable: "IdentityRole",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserRole<string>",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "TempId2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserToken<string>_User_UserId",
                schema: "dbo",
                table: "IdentityUserToken<string>",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "TempId3",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
