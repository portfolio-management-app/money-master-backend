using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class changeNotificationTableVer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThreadHoldAmount",
                table: "Notifications",
                newName: "LowThreadHoldAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "HighThreadHoldAmount",
                table: "Notifications",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighThreadHoldAmount",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "LowThreadHoldAmount",
                table: "Notifications",
                newName: "ThreadHoldAmount");
        }
    }
}
