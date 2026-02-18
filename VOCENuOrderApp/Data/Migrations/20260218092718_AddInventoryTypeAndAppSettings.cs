using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryTypeAndAppSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InventoryType",
                table: "ReplenishmentSyncLogs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Key);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_InventoryType",
                table: "ReplenishmentSyncLogs",
                column: "InventoryType");

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_InvType_Timestamp",
                table: "ReplenishmentSyncLogs",
                columns: new[] { "InventoryType", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_Size",
                table: "ReplenishmentSyncLogs",
                column: "Size");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_InventoryType",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_InvType_Timestamp",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_Size",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropColumn(
                name: "InventoryType",
                table: "ReplenishmentSyncLogs");
        }
    }
}
