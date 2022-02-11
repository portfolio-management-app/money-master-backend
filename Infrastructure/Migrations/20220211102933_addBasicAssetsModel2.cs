using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addBasicAssetsModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets");

            migrationBuilder.CreateIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets");

            migrationBuilder.CreateIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets",
                column: "UserId");
        }
    }
}
