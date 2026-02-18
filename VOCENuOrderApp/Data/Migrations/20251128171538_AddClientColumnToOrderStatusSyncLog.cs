using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VOCENuOrderApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClientColumnToOrderStatusSyncLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Client",
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
                name: "Client",
                table: "OrderStatusSyncLogs");
        }
    }
}
