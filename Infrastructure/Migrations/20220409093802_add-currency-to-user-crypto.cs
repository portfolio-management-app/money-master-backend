using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addcurrencytousercrypto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BuyPrice",
                table: "Cryptos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Cryptos",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Cryptos");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Cryptos");
        }
    }
}
