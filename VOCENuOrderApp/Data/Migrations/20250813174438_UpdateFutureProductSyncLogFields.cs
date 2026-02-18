using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFutureProductSyncLogFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseJson",
                table: "FutureProductSyncLogs",
                newName: "XoroResponseJson");

            migrationBuilder.RenameColumn(
                name: "RequestJson",
                table: "FutureProductSyncLogs",
                newName: "XoroRequestJson");

            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "FutureProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NuOrderId",
                table: "FutureProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NuOrderRequestJson",
                table: "FutureProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NuOrderResponseJson",
                table: "FutureProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "FutureProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Upc",
                table: "FutureProductSyncLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "FutureProductSyncLogs");

            migrationBuilder.DropColumn(
                name: "NuOrderId",
                table: "FutureProductSyncLogs");

            migrationBuilder.DropColumn(
                name: "NuOrderRequestJson",
                table: "FutureProductSyncLogs");

            migrationBuilder.DropColumn(
                name: "NuOrderResponseJson",
                table: "FutureProductSyncLogs");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "FutureProductSyncLogs");

            migrationBuilder.DropColumn(
                name: "Upc",
                table: "FutureProductSyncLogs");

            migrationBuilder.RenameColumn(
                name: "XoroResponseJson",
                table: "FutureProductSyncLogs",
                newName: "ResponseJson");

            migrationBuilder.RenameColumn(
                name: "XoroRequestJson",
                table: "FutureProductSyncLogs",
                newName: "RequestJson");
        }
    }
}
