using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using MyPortfolioWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyPortfolioWebAPI.Data
{
    public class MyPortfolioContext:DbContext
    {

        public MyPortfolioContext(DbContextOptions<MyPortfolioContext> options) : base(options)
        {

        }
        public DbSet<Emails> Emails { get; set; } = null!;
        public DbSet<Projects> Projects { get; set; } = null!;
    }
}
