using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestIdInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestSessionId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestSessionId",
                table: "Carts");
        }
    }
}
