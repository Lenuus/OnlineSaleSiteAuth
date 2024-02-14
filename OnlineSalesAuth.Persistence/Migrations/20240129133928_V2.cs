using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSalesAuth.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Category_ProductId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_CategoryId",
                table: "ProductCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Category_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Category_CategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Category_ProductId",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
