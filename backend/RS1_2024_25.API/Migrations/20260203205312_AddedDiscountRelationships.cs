using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiscountRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategories_Discounts_DiscountId",
                table: "DiscountCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProducts_Discounts_DiscountId",
                table: "DiscountProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategories_Discounts_DiscountId",
                table: "DiscountCategories",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProducts_Discounts_DiscountId",
                table: "DiscountProducts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategories_Discounts_DiscountId",
                table: "DiscountCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProducts_Discounts_DiscountId",
                table: "DiscountProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategories_Discounts_DiscountId",
                table: "DiscountCategories",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProducts_Discounts_DiscountId",
                table: "DiscountProducts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
