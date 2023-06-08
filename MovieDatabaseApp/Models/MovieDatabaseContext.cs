using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MovieDatabaseApp.Models;

namespace MovieDatabaseApp.Models
{
    public partial class MovieDatabaseContext : DbContext
    {
        public MovieDatabaseContext()
        {
        }

        public MovieDatabaseContext(DbContextOptions<MovieDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = $"server={DatabaseConstants.Server};database={DatabaseConstants.Database};user={DatabaseConstants.User};password={DatabaseConstants.Password}";
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("actor");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ActorBirthdate).HasColumnName("actor_birthdate");
                entity.Property(e => e.ActorName)
                    .HasMaxLength(100)
                    .HasColumnName("actor_name");
                entity.Property(e => e.ActorPicture)
                    .HasColumnType("blob")
                    .HasColumnName("actor_picture");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("movie");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.MovieBudget)
                    .HasPrecision(18, 2)
                    .HasColumnName("movie_budget");
                entity.Property(e => e.MovieDuration).HasColumnName("movie_duration");
                entity.Property(e => e.MovieGenre)
                    .HasMaxLength(50)
                    .HasColumnName("movie_genre");
                entity.Property(e => e.MovieName)
                    .HasMaxLength(100)
                    .HasColumnName("movie_name");

                entity.HasMany(d => d.Actors).WithMany(p => p.Movies)
                    .UsingEntity<Dictionary<string, object>>(
                        "Movieactor",
                        r => r.HasOne<Actor>().WithMany()
                            .HasForeignKey("ActorId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("movieactor_ibfk_2"),
                        l => l.HasOne<Movie>().WithMany()
                            .HasForeignKey("MovieId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("movieactor_ibfk_1"),
                        j =>
                        {
                            j.HasKey("MovieId", "ActorId")
                                .HasName("PRIMARY")
                                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                            j.ToTable("movieactor");
                            j.HasIndex(new[] { "ActorId" }, "actor_id");
                            j.IndexerProperty<int>("MovieId").HasColumnName("movie_id");
                            j.IndexerProperty<int>("ActorId").HasColumnName("actor_id");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
