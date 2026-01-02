using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SuperCircle.Models;

public partial class SuperCircleContext : DbContext
{
    public SuperCircleContext()
    {
    }

    public SuperCircleContext(DbContextOptions<SuperCircleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SuperCircle> SuperCircles { get; set; }
    public virtual DbSet<ProductionOrders> ProductionOrders { get; set; }
    public virtual DbSet<ProductionOrderHeader> ProductionOrderHeaders { get; set; }
    public virtual DbSet<ProcessMaster> ProcessMasters { get; set; }
    public virtual DbSet<ProductionOrderStageMaster> ProductionOrderStageMasters { get; set; }
    public virtual DbSet<InventoryStatusMaster> InventoryStatusMasters { get; set; }
    public virtual DbSet<UserLogin> UserLogins { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SuperCircle>(entity =>
        {
            entity.ToTable("SuperCircle");

            entity.Property(e => e.HeaderItemCode).HasMaxLength(50);
            entity.Property(e => e.HeaderProdName).HasMaxLength(50);
            entity.Property(e => e.HeaderWarehouse).HasMaxLength(50);
            entity.Property(e => e.RowItemCode).HasMaxLength(50);
            entity.Property(e => e.RowWarehouse).HasMaxLength(50);
            entity.Property(e => e.SeriesName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.DocDate).HasColumnName("DocDate");
        });
        modelBuilder.Entity<ProductionOrders>(entity =>
        {
            entity.ToTable("ProductionOrder");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd(); // 🔥 THIS IS CRITICAL

            entity.Property(e => e.HeaderItemCode).HasMaxLength(100);
            entity.Property(e => e.HeaderProdName).HasMaxLength(200);
            entity.Property(e => e.HeaderWarehouse).HasMaxLength(50);
            entity.Property(e => e.RowItemCode).HasMaxLength(100);
            entity.Property(e => e.RowWarehouse).HasMaxLength(50);
            entity.Property(e => e.SeriesName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
        });
        modelBuilder.Entity<ProductionOrderHeader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC071B782A52");

            entity.ToTable("ProductionOrderHeader");

            entity.Property(e => e.BackPlateNo).HasMaxLength(50);
            entity.Property(e => e.CustomerPartNo).HasMaxLength(100);
            entity.Property(e => e.Formulation).HasMaxLength(100);
            entity.Property(e => e.OrderQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PlanningQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RefNo).HasMaxLength(50);
            entity.Property(e => e.RouteCardNo).HasMaxLength(50);
            entity.Property(e => e.HeaderWareHouse).HasColumnName("HeaderWareHouse");

            entity.Property(e => e.headerItem).HasColumnName("HeaderItem"); 
        });
        modelBuilder.Entity<ProcessMaster>(entity =>
        {
            entity.HasKey(e => e.ProcessId).HasName("PK__Process___1B39A956CD558BFB");

            entity.ToTable("Process_Master");

            entity.Property(e => e.ProcessCode).HasMaxLength(50);
            entity.Property(e => e.ProcessName).HasMaxLength(100);
            entity.Property(e => e.isActive).HasColumnName("isActive");
        });
        modelBuilder.Entity<ProductionOrderStageMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC0708A99685");

            entity.ToTable("Production_Order_Stage_Master");

            entity.Property(e => e.DefectReason).HasMaxLength(250);
            entity.Property(e => e.MachineNo).HasMaxLength(50);
            entity.Property(e => e.NotOkQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OkQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProcessOperation).HasMaxLength(100);
            entity.Property(e => e.ProducedQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductionOperatorName).HasMaxLength(100);
            entity.Property(e => e.ProductionOrderNumber).HasMaxLength(50);
            entity.Property(e => e.ProductionOrderStageId).HasMaxLength(50);
            entity.Property(e => e.ProductionSupervisorName).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(250);
            entity.Property(e => e.Shift).HasMaxLength(20);
            entity.Property(e => e.StageType).HasColumnName("StageType");
            entity.Property(e => e.RouteCardNo).HasColumnName("RouteCardNo");
            entity.Property(e => e.QualityQuantity).HasColumnName("QualityQuantity");
        });
        modelBuilder.Entity<InventoryStatusMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC070DFE70BD");

            entity.ToTable("Inventory_Status_Master");

            entity.Property(e => e.ConsumedQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ItemCode).HasMaxLength(50);
            entity.Property(e => e.NotOkQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OkQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProducedQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QualityQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.rawItemCode).HasColumnName("rawItemCode");
        });
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserLogi__1788CC4C83B325BF");

            entity.ToTable("UserLogin");

            entity.HasIndex(e => e.Email, "UQ__UserLogi__A9D10534118807A5").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__UserLogi__C9F2845611505BF9").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
