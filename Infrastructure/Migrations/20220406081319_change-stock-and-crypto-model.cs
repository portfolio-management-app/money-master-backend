using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class changestockandcryptomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputCurrency",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "InputMoneyAmount",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "RealEstateAssets");

            migrationBuilder.DropColumn(
                name: "InputCurrency",
                table: "Cryptos");

            migrationBuilder.DropColumn(
                name: "InputCurrency",
                table: "CashAssets");

            migrationBuilder.DropColumn(
                name: "InputMoneyAmount",
                table: "CashAssets");

            migrationBuilder.RenameColumn(
                name: "InputMoneyAmount",
                table: "Cryptos",
                newName: "CurrentAmountHolding");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentAmountHolding",
                table: "Cryptos",
                newName: "InputMoneyAmount");

            migrationBuilder.AddColumn<string>(
                name: "InputCurrency",
                table: "Stocks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InputMoneyAmount",
                table: "Stocks",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BuyPrice",
                table: "RealEstateAssets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "InputCurrency",
                table: "Cryptos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InputCurrency",
                table: "CashAssets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InputMoneyAmount",
                table: "CashAssets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
