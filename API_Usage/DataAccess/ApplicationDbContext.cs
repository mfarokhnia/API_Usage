﻿using Microsoft.EntityFrameworkCore;
using API_Usage.Models;

namespace API_Usage.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Equity> Equities { get; set; }
        public DbSet<DailyEquity> DailyEquities {get;set;}
        //public DbSet<Financial> Financials { get; set; }
        //public DbSet<Quote> Quotes { get; set; }
        public DbSet<LargestTrade> Quotes { get; set; }

    }
}