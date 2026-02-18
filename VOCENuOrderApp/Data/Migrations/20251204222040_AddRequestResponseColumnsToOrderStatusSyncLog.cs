using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestResponseColumnsToOrderStatusSyncLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestJson",
                table: "OrderStatusSyncLogs",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestUrl",
                table: "OrderStatusSyncLogs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponseBody",
                table: "OrderStatusSyncLogs",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponseStatusCode",
                table: "OrderStatusSyncLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestJson",
                table: "OrderStatusSyncLogs");

            migrationBuilder.DropColumn(
                name: "RequestUrl",
                table: "OrderStatusSyncLogs");

            migrationBuilder.DropColumn(
                name: "ResponseBody",
                table: "OrderStatusSyncLogs");

            migrationBuilder.DropColumn(
                name: "ResponseStatusCode",
                table: "OrderStatusSyncLogs");
        }
    }
}
