using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VOCENuOrderApp.Models;
using VOCENuOrderApp.Models.XORO;
using VOCENuOrderApp.Models.NUORDER;

namespace VOCENuOrderApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // Add Database Models here
        //public DbSet<ShipmentLogModel> ShipmentLogModels { get; set; }
        public DbSet<InventoryLogModel> InventoryLogModels { get; set; }

        public DbSet<ProductSyncLog> ProductSyncLogs { get; set; }

        public DbSet<ReplenishmentSyncLog> ReplenishmentSyncLogs { get; set; }

        public DbSet<SyncCursor> SyncCursors { get; set; }

        public DbSet<OrderStatusSyncLog> OrderStatusSyncLogs { get; set; } // Added sales order status sync logs

        public DbSet<CustomerSyncLog> CustomerSyncLogs { get; set; }

        public DbSet<AppSetting> AppSettings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Indexes to optimize ProductSyncLog queries
            builder.Entity<ProductSyncLog>(entity =>
            {
                // Newest-first support and range queries
                entity.HasIndex(e => e.Timestamp).HasDatabaseName("IX_ProductSyncLogs_Timestamp");

                // Status + Timestamp composite for filtered recent logs
                entity.HasIndex(e => new { e.SyncStatus, e.Timestamp }).HasDatabaseName("IX_ProductSyncLogs_Status_Timestamp");

                // Brand + Timestamp composite for filtered recent logs
                entity.HasIndex(e => new { e.Brand, e.Timestamp }).HasDatabaseName("IX_ProductSyncLogs_Brand_Timestamp");

                // Prefix search support on BasePartNumber and ColorName
                entity.HasIndex(e => e.BasePartNumber).HasDatabaseName("IX_ProductSyncLogs_BasePartNumber");
                entity.HasIndex(e => e.ColorName).HasDatabaseName("IX_ProductSyncLogs_ColorName");
            });

            // Indexes to optimize ReplenishmentSyncLog queries
            builder.Entity<ReplenishmentSyncLog>(entity =>
            {
                entity.HasIndex(e => e.Timestamp).HasDatabaseName("IX_ReplenishmentSyncLogs_Timestamp");
                entity.HasIndex(e => new { e.Status, e.Timestamp }).HasDatabaseName("IX_ReplenishmentSyncLogs_Status_Timestamp");
                entity.HasIndex(e => e.BasePartNumber).HasDatabaseName("IX_ReplenishmentSyncLogs_BasePartNumber");
                entity.HasIndex(e => e.Color).HasDatabaseName("IX_ReplenishmentSyncLogs_Color");
                entity.HasIndex(e => e.InventoryType).HasDatabaseName("IX_ReplenishmentSyncLogs_InventoryType");
                entity.HasIndex(e => e.Size).HasDatabaseName("IX_ReplenishmentSyncLogs_Size");

                entity.HasIndex(e => new { e.BasePartNumber, e.Timestamp }).HasDatabaseName("IX_ReplenishmentSyncLogs_BasePart_Timestamp");
                entity.HasIndex(e => new { e.Color, e.Timestamp }).HasDatabaseName("IX_ReplenishmentSyncLogs_Color_Timestamp");
                entity.HasIndex(e => new { e.InventoryType, e.Timestamp }).HasDatabaseName("IX_ReplenishmentSyncLogs_InvType_Timestamp");
            });
        }
    }
}