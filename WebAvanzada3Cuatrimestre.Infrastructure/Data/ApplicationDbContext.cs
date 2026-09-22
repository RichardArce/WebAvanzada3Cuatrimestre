using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAvanzada3Cuatrimestre.Infrastructure;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carro> Carros { get; set; }

    public virtual DbSet<Duenno> Duennos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carro>(entity =>
        {
            entity.ToTable("Carro");

            entity.HasIndex(e => e.Placa, "IX_Carro_Placa").IsUnique();

            entity.HasIndex(e => e.Fkduenno, "idx_carro_fkduenno");

            entity.HasIndex(e => e.Placa, "idx_carro_placa");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.Fkduenno).HasColumnName("FKDuenno");

            entity.HasOne(d => d.FkduennoNavigation).WithMany(p => p.Carros).HasForeignKey(d => d.Fkduenno);
        });

        modelBuilder.Entity<Duenno>(entity =>
        {
            entity.ToTable("Duenno");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
