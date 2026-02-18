using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReplenishmentLogCompositeIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_BasePart_Timestamp",
                table: "ReplenishmentSyncLogs",
                columns: new[] { "BasePartNumber", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_Color_Timestamp",
                table: "ReplenishmentSyncLogs",
                columns: new[] { "Color", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_BasePart_Timestamp",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_Color_Timestamp",
                table: "ReplenishmentSyncLogs");
        }
    }
}
