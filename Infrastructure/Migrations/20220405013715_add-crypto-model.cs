using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addcryptomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cryptos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CryptoCoinCode = table.Column<string>(type: "text", nullable: true),
                    CurrentPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cryptos_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cryptos_PortfolioId",
                table: "Cryptos",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cryptos");
        }
    }
}
