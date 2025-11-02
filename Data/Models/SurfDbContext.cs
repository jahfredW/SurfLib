using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SurfLib.Data.Models;

public partial class SurfDbContext : DbContext
{
    public SurfDbContext()
    {
    }

    public SurfDbContext(DbContextOptions<SurfDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Maree> Marees { get; set; }

    public virtual DbSet<Prevision> Previsions { get; set; }

    public virtual DbSet<Spot> Spots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Maree>(entity =>
        {
            entity.HasKey(e => e.MareeId).HasName("PK__maree__78B12A65FE2A31BD");
        });

        modelBuilder.Entity<Prevision>(entity =>
        {
            entity.HasKey(e => new { e.SpotId, e.MareeId, e.PrevisionHeure, e.PrevisionDate }).HasName("PK__previsio__77856B2E4A745E31");

            entity.HasOne(d => d.Maree).WithMany(p => p.Previsions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__prevision__maree__3D5E1FD2");

            entity.HasOne(d => d.Spot).WithMany(p => p.Previsions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__prevision__spot___3C69FB99");
        });

        modelBuilder.Entity<Spot>(entity =>
        {
            entity.HasKey(e => e.SpotId).HasName("PK__spot__330AF0F66245A61D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
