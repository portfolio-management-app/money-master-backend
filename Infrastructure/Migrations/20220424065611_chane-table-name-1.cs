using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class chanetablename1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SingleAssetTransactionType",
                table: "SingleAssetTransactions",
                newName: "SingleAssetTransactionTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SingleAssetTransactionTypes",
                table: "SingleAssetTransactions",
                newName: "SingleAssetTransactionType");
        }
    }
}
