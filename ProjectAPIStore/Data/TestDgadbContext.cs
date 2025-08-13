using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using ProjectAPIStore.Models;

namespace ProjectAPIStore.Data;

public partial class TestDgadbContext : DbContext
{
    public TestDgadbContext()
    {
    }

    public TestDgadbContext(DbContextOptions<TestDgadbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3213E83FE12A6F32");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos).HasMaxLength(50).IsUnicode(false).HasColumnName("apellidos");
            entity.Property(e => e.Correo).HasMaxLength(100).IsUnicode(false).HasColumnName("correo");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime").HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombres).HasMaxLength(50).IsUnicode(false).HasColumnName("nombres");
            entity.Property(e => e.Username).HasMaxLength(100).IsUnicode(false).HasColumnName("username");
        });


        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(p => p.Id).HasName("PK_Productos");
            entity.ToTable("Productos");

            entity.Property(p => p.Id).HasColumnName("Id");
            entity.Property(p => p.Nombre).HasMaxLength(120).IsUnicode(true).HasColumnName("Nombre");
            entity.Property(p => p.Descripcion).HasMaxLength(255).IsUnicode(true).HasColumnName("Descripcion");
            entity.Property(p => p.Precio).HasPrecision(18, 2).HasColumnName("Precio");
            entity.Property(p => p.Stock).HasColumnName("Stock");
        });



        OnModelCreatingPartial(modelBuilder);
    }






















    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


