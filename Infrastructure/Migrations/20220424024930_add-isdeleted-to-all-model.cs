using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addisdeletedtoallmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stocks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SingleAssetTransactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RealEstateAssets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Portfolios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InvestFundTransactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InvestFunds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomInterestAssets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomInterestAssetInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cryptos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CashAssets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BankSavingAssets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RealEstateAssets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InvestFundTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InvestFunds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomInterestAssets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomInterestAssetInfos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cryptos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CashAssets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BankSavingAssets");
        }
    }
}
