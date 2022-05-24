using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addvalueinassetunittotransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountInDestinationAssetUnit",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInDestinationAssetUnit",
                table: "InvestFundTransactions",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountInDestinationAssetUnit",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "AmountInDestinationAssetUnit",
                table: "InvestFundTransactions");
        }
    }
}
