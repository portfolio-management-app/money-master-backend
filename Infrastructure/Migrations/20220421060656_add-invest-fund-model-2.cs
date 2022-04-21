using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addinvestfundmodel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "Cryptos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "Stocks",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "Cryptos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
