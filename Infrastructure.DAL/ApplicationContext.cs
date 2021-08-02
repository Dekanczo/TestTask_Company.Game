using Infrastructure.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System.IO;
using Bogus;
using System;

using Microsoft.Extensions.Logging;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DAL
{

    public partial class ApplicationContext : IdentityDbContext<User, Role, Guid>
    {
        public static readonly ILoggerFactory MainLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<CharacterWeaponRelation> CharacterWeaponRelations { get; set; }
        public virtual DbSet<Gamer> Gamers { get; set; }
        public virtual DbSet<GamerCharacterRelation> GamerCharacterRelations { get; set; }
        public virtual DbSet<GamerWeaponRelation> GamerWeaponRelations { get; set; }
        public virtual DbSet<Weapon> Weapons { get; set; }
        public override DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Infrastructure.ProjectsConfigurations/sharedsettings.json", optional: true)
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "./sharedsettings.json", optional: true)
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "./appsettings.json", optional: true)
                    .Build();

                var connectionString = configuration.GetConnectionString(
                    configuration.GetValue<string>("CurrentEnvironment"));

                optionsBuilder
                    .UseLoggerFactory(MainLoggerFactory)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                    .UseSqlServer(connectionString);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Character>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<CharacterWeaponRelation>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.WeaponId })
                    .HasName("PK_AdvertisingProfileRelations");

                entity.HasIndex(e => e.CharacterId, "IX_CharacterWeaponRelations_CharacterId");

                entity.HasIndex(e => e.WeaponId, "IX_CharacterWeaponRelations_WeaponId");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterWeaponRelations)
                    .HasForeignKey(d => d.CharacterId);

                entity.HasOne(d => d.Weapon)
                    .WithMany(p => p.CharacterWeaponRelations)
                    .HasForeignKey(d => d.WeaponId);
            });

            modelBuilder.Entity<Gamer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<GamerCharacterRelation>(entity =>
            {
                entity.HasKey(e => new { e.GamerId, e.CharacterId });

                entity.HasIndex(e => e.CharacterId, "IX_GamerCharacterRelations_CharacterId");

                entity.HasIndex(e => e.GamerId, "IX_GamerCharacterRelations_GamerId");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.GamerCharacterRelations)
                    .HasForeignKey(d => d.CharacterId);

                entity.HasOne(d => d.Gamer)
                    .WithMany(p => p.GamerCharacterRelations)
                    .HasForeignKey(d => d.GamerId)
                    .HasConstraintName("FK_GamerCharacterRelations_Gamer_GamerId");
            });

            modelBuilder.Entity<GamerWeaponRelation>(entity =>
            {
                entity.HasKey(e => new { e.GamerId, e.WeaponId });

                entity.HasIndex(e => e.GamerId, "IX_GamerWeaponRelations_GamerId");

                entity.HasIndex(e => e.WeaponId, "IX_GamerWeaponRelations_WeaponId");

                entity.HasOne(d => d.Gamer)
                    .WithMany(p => p.GamerWeaponRelations)
                    .HasForeignKey(d => d.GamerId)
                    .HasConstraintName("FK_GamerWeaponRelations_Gamer_GamerId");

                entity.HasOne(d => d.Weapon)
                    .WithMany(p => p.GamerWeaponRelations)
                    .HasForeignKey(d => d.WeaponId);
            });

            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<User>()
                .HasMany<Gamer>(c => c.Gamers)
                .WithOne(p => p.User);

            OnModelCreatingPartial(modelBuilder);
        }



        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<ApplicationContext>
    {

        public static readonly ILoggerFactory MainLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public ApplicationContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Infrastructure.ProjectsConfigurations/sharedsettings.json", optional: true)
                .AddJsonFile(@Directory.GetCurrentDirectory() + "./sharedsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationContext>();

            var connectionString = configuration.GetConnectionString(
                configuration.GetValue<string>("CurrentEnvironment"));
            builder
                .UseLoggerFactory(MainLoggerFactory)
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();

            return new ApplicationContext(builder.Options);
        }
    }
}