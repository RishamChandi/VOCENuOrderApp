using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSyncLogIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SyncStatus",
                table: "ProductSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ColorName",
                table: "ProductSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "ProductSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BasePartNumber",
                table: "ProductSyncLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSyncLogs_BasePartNumber",
                table: "ProductSyncLogs",
                column: "BasePartNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSyncLogs_Brand_Timestamp",
                table: "ProductSyncLogs",
                columns: new[] { "Brand", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSyncLogs_ColorName",
                table: "ProductSyncLogs",
                column: "ColorName");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSyncLogs_Status_Timestamp",
                table: "ProductSyncLogs",
                columns: new[] { "SyncStatus", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSyncLogs_Timestamp",
                table: "ProductSyncLogs",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductSyncLogs_BasePartNumber",
                table: "ProductSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductSyncLogs_Brand_Timestamp",
                table: "ProductSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductSyncLogs_ColorName",
                table: "ProductSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductSyncLogs_Status_Timestamp",
                table: "ProductSyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProductSyncLogs_Timestamp",
                table: "ProductSyncLogs");

            migrationBuilder.AlterColumn<string>(
                name: "SyncStatus",
                table: "ProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ColorName",
                table: "ProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "ProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BasePartNumber",
                table: "ProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
