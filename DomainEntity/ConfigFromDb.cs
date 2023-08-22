using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public record ConfigCommon
    {
        public int id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }


    public class ConfigCommonDbContext : DbContext
    {

        public DbSet<ConfigCommon> ConfigCommon { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string DbConnString = Environment.GetEnvironmentVariable("DbConnString", EnvironmentVariableTarget.User)!;
            optionsBuilder.UseSqlServer(DbConnString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigCommon>()
            .ToTable("T_Config");
        }
    }


   
}
