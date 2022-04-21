using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addinvestfundmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceAssetId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TargetAssetId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ValueByUsd",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Transactions",
                newName: "ReferentialAssetType");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "ReferentialAssetId");

            migrationBuilder.RenameColumn(
                name: "TargetType",
                table: "Transactions",
                newName: "CurrencyCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Transactions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InvestFundId",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIngoing",
                table: "Transactions",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SingleAssetTransactionType",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvestFunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestFunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestFunds_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InvestFundId",
                table: "Transactions",
                column: "InvestFundId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestFunds_PortfolioId",
                table: "InvestFunds",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_InvestFunds_InvestFundId",
                table: "Transactions",
                column: "InvestFundId",
                principalTable: "InvestFunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_InvestFunds_InvestFundId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "InvestFunds");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InvestFundId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InvestFundId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsIngoing",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SingleAssetTransactionType",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ReferentialAssetType",
                table: "Transactions",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "ReferentialAssetId",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Transactions",
                newName: "TargetType");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceAssetId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetAssetId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValueByUsd",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
