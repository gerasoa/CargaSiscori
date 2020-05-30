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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Siscori>(e => 
            //{
            //    e.HasNoKey();
            //});

            base.OnModelCreating(modelBuilder); 
        }
    }
}
