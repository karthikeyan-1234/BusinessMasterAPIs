using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace VendorLibrary.Models;

public partial class VendorDbContext : DbContext
{
    IConfiguration configuration;


    public VendorDbContext(DbContextOptions<VendorDbContext> options,IConfiguration configuration)
        : base(options)
    {
        this.configuration = configuration;
    }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(configuration.GetConnectionString("ServerConn"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vendors__3213E83FD412CCEE");

            entity.Property(e => e.Id).HasColumnName("id");
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
