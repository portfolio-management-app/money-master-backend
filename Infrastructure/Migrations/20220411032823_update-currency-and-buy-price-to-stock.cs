using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updatecurrencyandbuypricetostock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Stocks");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Stocks",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "BuyPrice",
                table: "Stocks",
                type: "text",
                nullable: true);
        }
    }
}
