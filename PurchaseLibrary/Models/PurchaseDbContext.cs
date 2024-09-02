using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PurchaseLibrary.Models;

public partial class PurchaseDbContext : DbContext
{
    public PurchaseDbContext()
    {
    }

    public PurchaseDbContext(DbContextOptions<PurchaseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(localdb)\\PurchaseServer;database=PurchaseDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3213E83FB6375332");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.MaterialType).HasColumnName("material_type");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.MaterialTypeNavigation).WithMany(p => p.Materials)
                .HasForeignKey(d => d.MaterialType)
                .HasConstraintName("FK__Materials__mater__7B5B524B");
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3213E83FFA9DEEA7");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3213E83F2B9875F8");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK__Purchases__vendo__0C85DE4D");
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3213E83F5691C397");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.PurchaseId).HasColumnName("purchase_id");
            entity.Property(e => e.Qty)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("qty");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("rate");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK__PurchaseI__purch__75A278F5");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vendors__3213E83F38FBAFC6");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
