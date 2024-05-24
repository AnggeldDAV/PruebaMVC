using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaMVC.Models;

public partial class GrupoCContext : DbContext
{
    public GrupoCContext()
    {
    }

    public GrupoCContext(DbContextOptions<GrupoCContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Albume> Albumes { get; set; }

    public virtual DbSet<Artista> Artistas { get; set; }

    public virtual DbSet<Cancione> Canciones { get; set; }

    public virtual DbSet<CancionesConcierto> CancionesConciertos { get; set; }

    public virtual DbSet<Concierto> Conciertos { get; set; }

    public virtual DbSet<ConciertosGrupo> ConciertosGrupos { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<GruposArtista> GruposArtistas { get; set; }

    public virtual DbSet<Lista> Listas { get; set; }

    public virtual DbSet<ListasCancione> ListasCanciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=musicagrupos.database.windows.net;database=GrupoC;user=as;password=P0t@t0P0t@t0");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Albume>(entity =>
        {
            entity.Property(e => e.Genero)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Grupos).WithMany(p => p.Albumes)
                .HasForeignKey(d => d.GruposId)
                .HasConstraintName("FK_Albumes_Grupos");
        });

        modelBuilder.Entity<Artista>(entity =>
        {
            entity.Property(e => e.Genero)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cancione>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Genero)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Albumes).WithMany(p => p.Canciones)
                .HasForeignKey(d => d.AlbumesId)
                .HasConstraintName("FK_Canciones_Albumes");
        });

        modelBuilder.Entity<CancionesConcierto>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Canciones).WithMany(p => p.CancionesConciertos)
                .HasForeignKey(d => d.CancionesId)
                .HasConstraintName("FK_CancionesConciertos_Canciones");

            entity.HasOne(d => d.Conciertos).WithMany(p => p.CancionesConciertos)
                .HasForeignKey(d => d.ConciertosId)
                .HasConstraintName("FK_CancionesConciertos_Conciertos");
        });

        modelBuilder.Entity<Concierto>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Genero)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Lugar)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ConciertosGrupo>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Conciertos).WithMany(p => p.ConciertosGrupos)
                .HasForeignKey(d => d.ConciertosId)
                .HasConstraintName("FK_ConciertosGrupos_Conciertos");

            entity.HasOne(d => d.Grupos).WithMany(p => p.ConciertosGrupos)
                .HasForeignKey(d => d.GruposId)
                .HasConstraintName("FK_ConciertosGrupos_Grupos");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GruposArtista>(entity =>
        {
            entity.HasOne(d => d.Artistas).WithMany(p => p.GruposArtista)
                .HasForeignKey(d => d.ArtistasId)
                .HasConstraintName("FK_GruposArtistas_Artistas");

            entity.HasOne(d => d.Grupos).WithMany(p => p.GruposArtista)
                .HasForeignKey(d => d.GruposId)
                .HasConstraintName("FK_GruposArtistas_Grupos");
        });

        modelBuilder.Entity<Lista>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Lista)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Listas_Usuarios");
        });

        modelBuilder.Entity<ListasCancione>(entity =>
        {
            entity.HasOne(d => d.Canciones).WithMany(p => p.ListasCanciones)
                .HasForeignKey(d => d.CancionesId)
                .HasConstraintName("FK_ListasCanciones_Canciones");

            entity.HasOne(d => d.Listas).WithMany(p => p.ListasCanciones)
                .HasForeignKey(d => d.ListasId)
                .HasConstraintName("FK_ListasCanciones_Listas");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
