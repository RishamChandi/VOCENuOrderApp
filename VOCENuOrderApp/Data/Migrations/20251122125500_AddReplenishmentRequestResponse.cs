using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    public partial class AddReplenishmentRequestResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestJson",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseJson",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestJson",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropColumn(
                name: "ResponseJson",
                table: "ReplenishmentSyncLogs");
        }
    }
}
