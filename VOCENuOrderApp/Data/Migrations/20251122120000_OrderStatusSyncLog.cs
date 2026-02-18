using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    public partial class OrderStatusSyncLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatusSyncLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NuOrderInternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreviousStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    EndpointUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusSyncLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusSyncLogs_OrderNumber_Timestamp",
                table: "OrderStatusSyncLogs",
                columns: new[] { "OrderNumber", "Timestamp" });
            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusSyncLogs_Success_Timestamp",
                table: "OrderStatusSyncLogs",
                columns: new[] { "Success", "Timestamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStatusSyncLogs");
        }
    }
}
