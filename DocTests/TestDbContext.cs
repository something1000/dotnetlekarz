using dotnetlekarz.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocTests
{
    class TestDbContext
    {
        public static DocDbContext GetDocDbContext()
        {
            var options = new DbContextOptionsBuilder<DocDbContext>()
                .UseInMemoryDatabase("in_mem_Test")
                .Options;
            DocDbContext db = new DocDbContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }
    }
}
