using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    public partial class AddIndexesReplenishmentSyncLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_Timestamp",
                table: "ReplenishmentSyncLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_BasePart_Color_Status",
                table: "ReplenishmentSyncLogs",
                columns: new[] { "BasePartNumber", "Color", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_Timestamp",
                table: "ReplenishmentSyncLogs");
            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_BasePart_Color_Status",
                table: "ReplenishmentSyncLogs");
        }
    }
}
