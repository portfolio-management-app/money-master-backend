using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addfeeandtaxintransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "InvestFundTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "InvestFundTransactions",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "InvestFundTransactions");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "InvestFundTransactions");
        }
    }
}
