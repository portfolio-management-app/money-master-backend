using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addportfoliototransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SingleAssetTransactionTypes",
                table: "SingleAssetTransactions",
                newName: "SingleAssetTransactionType");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "SingleAssetTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "InvestFundTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SingleAssetTransactions_PortfolioId",
                table: "SingleAssetTransactions",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestFundTransactions_PortfolioId",
                table: "InvestFundTransactions",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestFundTransactions_Portfolios_PortfolioId",
                table: "InvestFundTransactions",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleAssetTransactions_Portfolios_PortfolioId",
                table: "SingleAssetTransactions",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestFundTransactions_Portfolios_PortfolioId",
                table: "InvestFundTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleAssetTransactions_Portfolios_PortfolioId",
                table: "SingleAssetTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SingleAssetTransactions_PortfolioId",
                table: "SingleAssetTransactions");

            migrationBuilder.DropIndex(
                name: "IX_InvestFundTransactions_PortfolioId",
                table: "InvestFundTransactions");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "InvestFundTransactions");

            migrationBuilder.RenameColumn(
                name: "SingleAssetTransactionType",
                table: "SingleAssetTransactions",
                newName: "SingleAssetTransactionTypes");
        }
    }
}
