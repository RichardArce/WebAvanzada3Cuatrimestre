using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAvanzada3Cuatrimestre.Infrastructure.Entidades;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\richa\\Desktop\\Proyectos de Clase\\WebAvanzada3Cuatrimestre\\WebAvanzada3Cuatrimestre.Infrastructure\\lavacar.db");

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
