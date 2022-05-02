using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class adddestinationtotransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationAssetId",
                table: "SingleAssetTransactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationAssetType",
                table: "SingleAssetTransactions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationAssetId",
                table: "SingleAssetTransactions");

            migrationBuilder.DropColumn(
                name: "DestinationAssetType",
                table: "SingleAssetTransactions");
        }
    }
}
