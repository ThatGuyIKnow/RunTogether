using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RunTogether.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
