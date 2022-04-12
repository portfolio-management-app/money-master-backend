using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addcashmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAmount",
                table: "CashAssets");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "CashAssets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "CashAssets",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CashAssets");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "CashAssets");

            migrationBuilder.AddColumn<double>(
                name: "CurrentAmount",
                table: "CashAssets",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
