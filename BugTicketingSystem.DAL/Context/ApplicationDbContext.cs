using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
               : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bug>()
                .HasMany(b => b.Users)
                .WithMany(u => u.AssignedBugs)
                .UsingEntity(j => j.ToTable("BugUser"));
        }

    }
}
