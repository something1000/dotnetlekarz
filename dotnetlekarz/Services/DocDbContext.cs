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
        public DbSet<User> Users{ get; set; }
        public DbSet<Visit> Visits { get; set; }
        public IQueryable<Visit> VisitsWithUsers => this.Visits.Include(x => x.Doctor).Include(x => x.Visitor);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=docvisit.db");
        }
    }
}
