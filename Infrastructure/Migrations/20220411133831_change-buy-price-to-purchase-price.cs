using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class changebuypricetopurchaseprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuyPrice",
                table: "Cryptos",
                newName: "PurchasePrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "Cryptos",
                newName: "BuyPrice");
        }
    }
}
