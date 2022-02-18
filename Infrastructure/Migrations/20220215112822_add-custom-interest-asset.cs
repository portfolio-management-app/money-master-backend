using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addcustominterestasset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestAssets");

            migrationBuilder.CreateTable(
                name: "BankSavingAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsGoingToReinState = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<double>(type: "double precision", nullable: false),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InterestRate = table.Column<double>(type: "double precision", nullable: false),
                    TermRange = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankSavingAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankSavingAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomInterestAssetInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomInterestAssetInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomInterestAssetInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomInterestAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomInterestAssetInfoId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<double>(type: "double precision", nullable: false),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InterestRate = table.Column<double>(type: "double precision", nullable: false),
                    TermRange = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomInterestAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomInterestAssets_CustomInterestAssetInfos_CustomInteres~",
                        column: x => x.CustomInterestAssetInfoId,
                        principalTable: "CustomInterestAssetInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomInterestAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankSavingAssets_UserId",
                table: "BankSavingAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomInterestAssetInfos_UserId",
                table: "CustomInterestAssetInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomInterestAssets_CustomInterestAssetInfoId",
                table: "CustomInterestAssets",
                column: "CustomInterestAssetInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomInterestAssets_UserId",
                table: "CustomInterestAssets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankSavingAssets");

            migrationBuilder.DropTable(
                name: "CustomInterestAssets");

            migrationBuilder.DropTable(
                name: "CustomInterestAssetInfos");

            migrationBuilder.CreateTable(
                name: "InterestAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InputCurrency = table.Column<string>(type: "text", nullable: true),
                    InputDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InputMoneyAmount = table.Column<double>(type: "double precision", nullable: false),
                    InterestRate = table.Column<double>(type: "double precision", nullable: false),
                    LastChanged = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TermRange = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_InterestAssets_UserId",
                table: "InterestAssets",
                column: "UserId");
        }
    }
}
