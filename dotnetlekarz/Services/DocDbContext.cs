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
        public DbSet<UserModel> Users{ get; set; }
        public DbSet<VisitModel> Visits { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=docvisit.db");
        }
    }
}
