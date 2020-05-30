using CargaSiscori.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargaSiscori.Context
{
    public class BWContext : DbContext
    {    
        public DbSet<Siscori> Siscori { get; set; }
        public DbSet<LogEntity> LogEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString());
        }
    }
}
