using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSalesAuth.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "Campaigns");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedPrice",
                table: "ProductCampaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "ProductCampaigns");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedPrice",
                table: "Campaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
