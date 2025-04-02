using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yanyana.BackEnd.Core.Entities;

namespace Yanyana.BackEnd.Data.Context
{
    public class YanDbContext : DbContext
    {
        public YanDbContext(DbContextOptions<YanDbContext> options) : base(options)
        { }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<GoogleLocation> GoogleLocations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CommentPicture> CommentPictures { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<UserPicture> UserPictures { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<PlaceCategory> PlaceCategories { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Role Relationships
            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserRole Relationships
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaceCategory>()
                .HasKey(pc => new { pc.PlaceId, pc.CategoryId });
                modelBuilder.Entity<PlaceCategory>()
                  .HasOne(pc => pc.Place)
                  .WithMany(p => p.PlaceCategories)
                  .HasForeignKey(pc => pc.PlaceId);
                modelBuilder.Entity<PlaceCategory>()
                  .HasOne(pc => pc.Category)
                  .WithMany(c => c.PlaceCategories)
                  .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<Country>()
                  .HasMany(c => c.Cities)
                  .WithOne(ci => ci.Country)
                  .HasForeignKey(ci => ci.CountryId)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Country>()
                  .HasMany(c => c.Cities)
                  .WithOne(ci => ci.Country)
                  .HasForeignKey(ci => ci.CountryId)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<District>()
                   .HasMany(d => d.Streets)
                   .WithOne(s => s.District)
                   .HasForeignKey(s => s.DistrictId)
                   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Place>()
               .HasMany(p => p.Comments)
               .WithOne(c => c.Place)
               .HasForeignKey(c => c.PlaceId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Place>()
              .HasMany(p => p.Rates)
              .WithOne(r => r.Place)
              .HasForeignKey(r => r.PlaceId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
