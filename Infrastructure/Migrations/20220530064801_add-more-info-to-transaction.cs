using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addmoreinfototransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SingleAssetTransactionDestination",
                table: "SingleAssetTransactions");

            migrationBuilder.AddColumn<decimal>(
                name: "DestinationAmount",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DestinationCurrency",
                table: "SingleAssetTransactions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationAmount",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationCurrency",
                table: "SingleAssetTransactions");

            migrationBuilder.AddColumn<int>(
                name: "SingleAssetTransactionDestination",
                table: "SingleAssetTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
