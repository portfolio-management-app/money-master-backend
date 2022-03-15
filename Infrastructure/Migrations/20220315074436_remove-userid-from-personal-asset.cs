using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class removeuseridfrompersonalasset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankSavingAssets_Users_UserId",
                table: "BankSavingAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_CashAssets_Users_UserId",
                table: "CashAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomInterestAssets_Users_UserId",
                table: "CustomInterestAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateAssets_Users_UserId",
                table: "RealEstateAssets");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateAssets_UserId",
                table: "RealEstateAssets");

            migrationBuilder.DropIndex(
                name: "IX_CustomInterestAssets_UserId",
                table: "CustomInterestAssets");

            migrationBuilder.DropIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets");

            migrationBuilder.DropIndex(
                name: "IX_BankSavingAssets_UserId",
                table: "BankSavingAssets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RealEstateAssets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomInterestAssets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CashAssets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankSavingAssets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RealEstateAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomInterestAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CashAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BankSavingAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateAssets_UserId",
                table: "RealEstateAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomInterestAssets_UserId",
                table: "CustomInterestAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankSavingAssets_UserId",
                table: "BankSavingAssets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankSavingAssets_Users_UserId",
                table: "BankSavingAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashAssets_Users_UserId",
                table: "CashAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomInterestAssets_Users_UserId",
                table: "CustomInterestAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateAssets_Users_UserId",
                table: "RealEstateAssets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
