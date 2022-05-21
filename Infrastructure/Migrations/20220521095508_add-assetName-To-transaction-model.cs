using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addassetNameTotransactionmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationAssetName",
                table: "SingleAssetTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferentialAssetName",
                table: "SingleAssetTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferentialAssetName",
                table: "InvestFundTransactions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationAssetName",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "ReferentialAssetName",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "ReferentialAssetName",
                table: "InvestFundTransactions");
        }
    }
}
