using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Data.IntegrationTests
{
    public static class DbContextFactory
    {
        public static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=rapidpay-data-integration-test.db")
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.OpenConnection(); // Open connection
            dbContext.Database.EnsureCreated(); // Create the database schema

            return dbContext;
        }
    }
}