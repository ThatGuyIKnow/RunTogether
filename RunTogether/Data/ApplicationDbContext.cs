using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunTogether.Areas.Identity;

namespace RunTogether.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Run> Runs { get; set; }
        public DbSet<RunRoute> RunRoutes { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<StartPoint> StartPoints { get; set; }
        public DbSet<EndPoint> EndPoints { get; set; }
        public DbSet<ThroughPoint> ThroughPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            //modelBuilder.Entity<Post>()
            //    .HasOne(p => p.Blog)
            //    .WithMany(b => b.Posts);

            modelBuilder.Entity<Stage>()
                .HasOne(s => s.RunRoute)
                .WithMany(r => r.Stages);

            modelBuilder.Entity<Stage>()
                .HasOne(s => s.StartPoint)
                .WithOne(sp => sp.Stage);

            modelBuilder.Entity<Stage>()
                .HasOne(s => s.EndPoint)
                .WithOne(sp => sp.Stage);

            modelBuilder.Entity<Stage>()
                .HasMany(s => s.ThroughPoints)
                .WithOne(t => t.Stage);

            modelBuilder.Entity<StartPoint>()
                .HasOne(sp => sp.Stage)
                .WithOne(s => s.StartPoint);
            
            modelBuilder.Entity<EndPoint>()
                .HasOne(e => e.Stage)
                .WithOne(s => s.EndPoint);
            
            modelBuilder.Entity<ThroughPoint>()
                .HasOne(t => t.Stage)
                .WithMany(s => s.ThroughPoints);

            modelBuilder.Entity<RunRoute>()
                .HasMany(rr => rr.Stages)
                .WithOne(s => s.RunRoute);

            modelBuilder.Entity<RunRoute>()
                .HasOne(rr => rr.Run)
                .WithOne(r => r.Route);

            modelBuilder.Entity<Run>()
                .HasOne(r => r.Route)
                .WithOne(rr => rr.Run);

            modelBuilder.Entity<Run>()
                .HasMany(r => r.Runners)
                .WithOne(rr => rr.Run);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext() : base()
        {
        }
    }
}
