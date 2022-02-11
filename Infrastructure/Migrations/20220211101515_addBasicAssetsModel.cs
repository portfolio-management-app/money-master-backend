using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addBasicAssetsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentAmount = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<double>(type: "double precision", nullable: false),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InterestRate = table.Column<double>(type: "double precision", nullable: false),
                    TermRange = table.Column<int>(type: "integer", nullable: false),
                    IsGoingToReinstate = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<double>(type: "double precision", nullable: false),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealEstateAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CurrentPrice = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<double>(type: "double precision", nullable: false),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealEstateAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashAssets_UserId",
                table: "CashAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestAssets_UserId",
                table: "InterestAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateAssets_UserId",
                table: "RealEstateAssets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashAssets");

            migrationBuilder.DropTable(
                name: "InterestAssets");

            migrationBuilder.DropTable(
                name: "RealEstateAssets");
        }
    }
}
