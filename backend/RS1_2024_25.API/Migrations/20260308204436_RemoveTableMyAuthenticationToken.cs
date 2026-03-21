using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTableMyAuthenticationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyAuthenticationTokensAll");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "MyAuthenticationTokensAll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyAuthenticationTokensAll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyAuthenticationTokensAll_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyAuthenticationTokensAll_UserId",
                table: "MyAuthenticationTokensAll",
                column: "UserId");
        }
    }
}
