using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addoutsideportfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomInterestAssets_CustomInterestAssetInfos_CustomInteres~",
                table: "CustomInterestAssets");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "RealEstateAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CustomInterestAssetInfoId",
                table: "CustomInterestAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "CustomInterestAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "CashAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "BankSavingAssets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateAssets_PortfolioId",
                table: "RealEstateAssets",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomInterestAssets_PortfolioId",
                table: "CustomInterestAssets",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAssets_PortfolioId",
                table: "CashAssets",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_BankSavingAssets_PortfolioId",
                table: "BankSavingAssets",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId",
                table: "Portfolios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankSavingAssets_Portfolios_PortfolioId",
                table: "BankSavingAssets",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashAssets_Portfolios_PortfolioId",
                table: "CashAssets",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomInterestAssets_CustomInterestAssetInfos_CustomInteres~",
                table: "CustomInterestAssets",
                column: "CustomInterestAssetInfoId",
                principalTable: "CustomInterestAssetInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomInterestAssets_Portfolios_PortfolioId",
                table: "CustomInterestAssets",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateAssets_Portfolios_PortfolioId",
                table: "RealEstateAssets",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankSavingAssets_Portfolios_PortfolioId",
                table: "BankSavingAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_CashAssets_Portfolios_PortfolioId",
                table: "CashAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomInterestAssets_CustomInterestAssetInfos_CustomInteres~",
                table: "CustomInterestAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomInterestAssets_Portfolios_PortfolioId",
                table: "CustomInterestAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateAssets_Portfolios_PortfolioId",
                table: "RealEstateAssets");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateAssets_PortfolioId",
                table: "RealEstateAssets");

            migrationBuilder.DropIndex(
                name: "IX_CustomInterestAssets_PortfolioId",
                table: "CustomInterestAssets");

            migrationBuilder.DropIndex(
                name: "IX_CashAssets_PortfolioId",
                table: "CashAssets");

            migrationBuilder.DropIndex(
                name: "IX_BankSavingAssets_PortfolioId",
                table: "BankSavingAssets");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "RealEstateAssets");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "CustomInterestAssets");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "CashAssets");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "BankSavingAssets");

            migrationBuilder.AlterColumn<int>(
                name: "CustomInterestAssetInfoId",
                table: "CustomInterestAssets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomInterestAssets_CustomInterestAssetInfos_CustomInteres~",
                table: "CustomInterestAssets",
                column: "CustomInterestAssetInfoId",
                principalTable: "CustomInterestAssetInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
