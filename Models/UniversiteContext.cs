using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Universite.Models;

public partial class UniversiteContext : DbContext
{
    public UniversiteContext()
    {
    }

    public UniversiteContext(DbContextOptions<UniversiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bolumler> Bolumlers { get; set; }

    public virtual DbSet<Fakulteler> Fakultelers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BET2OM6\\SQLEXPRESS;Database=universite;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bolumler>(entity =>
        {
            entity.HasKey(e => e.BolumId);

            entity.ToTable("Bolumler");

            entity.Property(e => e.BolumId).HasColumnName("bolumId");
            entity.Property(e => e.BolumAd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bolumAd");
            entity.Property(e => e.FakulteId).HasColumnName("fakulteId");

            entity.HasOne(d => d.Fakulte).WithMany(p => p.Bolumlers)
                .HasForeignKey(d => d.FakulteId)
                .HasConstraintName("FK_Bolumler_fakulteler");
        });

        modelBuilder.Entity<Fakulteler>(entity =>
        {
            entity.HasKey(e => e.FakulteId);

            entity.ToTable("fakulteler");

            entity.Property(e => e.FakulteId).HasColumnName("fakulteId");
            entity.Property(e => e.FakulteAd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fakulteAd");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
