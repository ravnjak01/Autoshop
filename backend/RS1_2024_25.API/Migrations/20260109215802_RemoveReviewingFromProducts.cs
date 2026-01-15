using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReviewingFromProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgGrade",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NumberOfReviews",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvgGrade",
                table: "Products",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReviews",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
