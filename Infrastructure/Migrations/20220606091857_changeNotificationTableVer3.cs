using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class changeNotificationTableVer3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOn",
                table: "Notifications",
                newName: "IsLowOn");

            migrationBuilder.AddColumn<bool>(
                name: "IsHighOn",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHighOn",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "IsLowOn",
                table: "Notifications",
                newName: "IsOn");
        }
    }
}
