using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgarCacheFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EdgarCacheFramework.DataAccess
{
    public class  FinancialStatements: DbContext
    {
        public FinancialStatements()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptions)
        {
            
            dbContextOptions.UseSqlite(@"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "FinancialStatementsPulled.db");
        }

        public DbSet<FinancialStatementInstance> FinancialStatementsInstances { get; set; }
        public DbSet<StockPriceInstance> StockPriceInstances { get; set; }

    }
}
