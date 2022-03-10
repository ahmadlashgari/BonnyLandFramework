using BL.Framework.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace BL.Test.Infrastructure.Persistence
{
    public class TestDbContext : DbContextBase
    {
        private readonly ILogger<TestDbContext> _logger;

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
            ConfigurationAssembly = GetType().Assembly;
        }

        public TestDbContext(DbContextOptions<TestDbContext> options, ILogger<TestDbContext> logger) : base(options, logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ConfigurationAssembly = GetType().Assembly;
        }

        //public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedData(builder);
        }

        private ModelBuilder SeedData(ModelBuilder builder)
        {
            //builder.Entity<Service>().HasData(
            //    new Service { Id = 1, Title = "??????? ????", CreatedOn = DateTime.Now }, 
            //    new Service { Id = 2, Title = "????? ? ??? ?????", CreatedOn = DateTime.Now }, 
            //    new Service { Id = 3, Title = "??????", CreatedOn = DateTime.Now }, 
            //    new Service { Id = 4, Title = "???????", CreatedOn = DateTime.Now }, 
            //    new Service { Id = 5, Title = "?????", CreatedOn = DateTime.Now }, 
            //    new Service { Id = 6, Title = "????? ????", CreatedOn = DateTime.Now }, 
            //    new Service { Id = 7, Title = "??????", CreatedOn = DateTime.Now }); 

            return builder;
        }
    }
}
