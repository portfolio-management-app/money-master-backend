using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class changetransactionshape : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AmountOfReferentialAssetBeforeCreatingTransaction",
                table: "SingleAssetTransactions",
                newName: "ValueOfReferentialAssetBeforeCreatingTransaction");

            migrationBuilder.RenameColumn(
                name: "AmountOfReferentialAssetBeforeCreatingTransaction",
                table: "InvestFundTransactions",
                newName: "ValueOfReferentialAssetBeforeCreatingTransaction");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOfDestinationAfterCreatingTransactionInSpecificUnit",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOfSourceAssetAfterCreatingTransactionInSpecificUnit",
                table: "SingleAssetTransactions",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfDestinationAfterCreatingTransactionInSpecificUnit",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "AmountOfSourceAssetAfterCreatingTransactionInSpecificUnit",
                table: "SingleAssetTransactions");

            migrationBuilder.RenameColumn(
                name: "ValueOfReferentialAssetBeforeCreatingTransaction",
                table: "SingleAssetTransactions",
                newName: "AmountOfReferentialAssetBeforeCreatingTransaction");

            migrationBuilder.RenameColumn(
                name: "ValueOfReferentialAssetBeforeCreatingTransaction",
                table: "InvestFundTransactions",
                newName: "AmountOfReferentialAssetBeforeCreatingTransaction");
        }
    }
}
