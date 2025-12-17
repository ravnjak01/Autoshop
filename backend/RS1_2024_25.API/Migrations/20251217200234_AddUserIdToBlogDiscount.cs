using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToBlogDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "BlogPosts");

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "Discounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserId",
                table: "DiscountProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserId",
                table: "DiscountCodes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserId",
                table: "DiscountCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogRatings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "BlogPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_CreateUserId",
                table: "Discounts",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProducts_LastModifiedUserId",
                table: "DiscountProducts",
                column: "LastModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_LastModifiedUserId",
                table: "DiscountCodes",
                column: "LastModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategories_LastModifiedUserId",
                table: "DiscountCategories",
                column: "LastModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogRatings_UserId",
                table: "BlogRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_UserId",
                table: "BlogComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_UserId",
                table: "BlogComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_AuthorId",
                table: "BlogPosts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogRatings_AspNetUsers_UserId",
                table: "BlogRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCategories_AspNetUsers_LastModifiedUserId",
                table: "DiscountCategories",
                column: "LastModifiedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_AspNetUsers_LastModifiedUserId",
                table: "DiscountCodes",
                column: "LastModifiedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountProducts_AspNetUsers_LastModifiedUserId",
                table: "DiscountProducts",
                column: "LastModifiedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_AspNetUsers_CreateUserId",
                table: "Discounts",
                column: "CreateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_UserId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogRatings_AspNetUsers_UserId",
                table: "BlogRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCategories_AspNetUsers_LastModifiedUserId",
                table: "DiscountCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_AspNetUsers_LastModifiedUserId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountProducts_AspNetUsers_LastModifiedUserId",
                table: "DiscountProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_AspNetUsers_CreateUserId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_CreateUserId",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_DiscountProducts_LastModifiedUserId",
                table: "DiscountProducts");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_LastModifiedUserId",
                table: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCategories_LastModifiedUserId",
                table: "DiscountCategories");

            migrationBuilder.DropIndex(
                name: "IX_BlogRatings_UserId",
                table: "BlogRatings");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_UserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserId",
                table: "DiscountProducts");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserId",
                table: "DiscountCategories");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BlogRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BlogComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
