using dotnetlekarz.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetlekarz.Services
{
    public class DocDbContext : DbContext
    {

        public DocDbContext(DbContextOptions<DocDbContext> options)
        : base(options)
        { }

        public DbSet<User> Users{ get; set; }
        public DbSet<Visit> Visits { get; set; }
        public IQueryable<Visit> VisitsWithUsers => this.Visits.Include(x => x.Doctor).Include(x => x.Visitor);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>()
            .HasIndex(p => new { p.DoctorId, p.DateTime }).IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Login }).IsUnique();
        }

       
    }
}
