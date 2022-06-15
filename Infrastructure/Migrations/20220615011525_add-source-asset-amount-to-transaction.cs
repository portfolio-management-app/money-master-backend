using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addsourceassetamounttotransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountOfReferentialAssetBeforeCreatingTransaction",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOfReferentialAssetBeforeCreatingTransaction",
                table: "InvestFundTransactions",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfReferentialAssetBeforeCreatingTransaction",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "AmountOfReferentialAssetBeforeCreatingTransaction",
                table: "InvestFundTransactions");
        }
    }
}
