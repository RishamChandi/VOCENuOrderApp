using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReplenishmentSyncLogIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BasePartNumber",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_BasePartNumber",
                table: "ReplenishmentSyncLogs",
                column: "BasePartNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_Color",
                table: "ReplenishmentSyncLogs",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_Status_Timestamp",
                table: "ReplenishmentSyncLogs",
                columns: new[] { "Status", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ReplenishmentSyncLogs_Timestamp",
                table: "ReplenishmentSyncLogs",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_BasePartNumber",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_Color",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_Status_Timestamp",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ReplenishmentSyncLogs_Timestamp",
                table: "ReplenishmentSyncLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BasePartNumber",
                table: "ReplenishmentSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
