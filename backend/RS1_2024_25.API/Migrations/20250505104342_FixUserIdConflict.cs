using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIdConflict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyAuthenticationTokensAll_MyAppUsersAll_MyAppUserId",
                table: "MyAuthenticationTokensAll");

            migrationBuilder.DropTable(
                name: "MyAppUsersAll");

            migrationBuilder.AlterColumn<string>(
                name: "MyAppUserId",
                table: "MyAuthenticationTokensAll",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MyAuthenticationTokensAll",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "MyAuthenticationTokensAll",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDean",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutUntil",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MyAuthenticationTokensAll_AspNetUsers_MyAppUserId",
                table: "MyAuthenticationTokensAll",
                column: "MyAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyAuthenticationTokensAll_AspNetUsers_MyAppUserId",
                table: "MyAuthenticationTokensAll");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MyAuthenticationTokensAll");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "MyAuthenticationTokensAll");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDean",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutUntil",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "MyAppUserId",
                table: "MyAuthenticationTokensAll",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "MyAppUsersAll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsDean = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockoutUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyAppUsersAll", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MyAuthenticationTokensAll_MyAppUsersAll_MyAppUserId",
                table: "MyAuthenticationTokensAll",
                column: "MyAppUserId",
                principalTable: "MyAppUsersAll",
                principalColumn: "Id");
        }
    }
}
