using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addinvestfundmodel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_InvestFunds_InvestFundId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InvestFundId",
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

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "SingleAssetTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "SingleAssetTransactionType",
                table: "SingleAssetTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SingleAssetTransactions",
                table: "SingleAssetTransactions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InvestFundTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvestFundId = table.Column<int>(type: "integer", nullable: false),
                    IsIngoing = table.Column<bool>(type: "boolean", nullable: false),
                    ReferentialAssetId = table.Column<int>(type: "integer", nullable: false),
                    ReferentialAssetType = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestFundTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestFundTransactions_InvestFunds_InvestFundId",
                        column: x => x.InvestFundId,
                        principalTable: "InvestFunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestFundTransactions_InvestFundId",
                table: "InvestFundTransactions",
                column: "InvestFundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestFundTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SingleAssetTransactions",
                table: "SingleAssetTransactions");

            migrationBuilder.RenameTable(
                name: "SingleAssetTransactions",
                newName: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "SingleAssetTransactionType",
                table: "Transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InvestFundId",
                table: "Transactions",
                column: "InvestFundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_InvestFunds_InvestFundId",
                table: "Transactions",
                column: "InvestFundId",
                principalTable: "InvestFunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
