using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebScrape.Entity;

namespace WebScrape.Dal.Context
{
    public class ScrapingContext : DbContext
    {
        public ScrapingContext(DbContextOptions<ScrapingContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        DbSet<JobBoard> JobBoard { get; set; }
    }
}
