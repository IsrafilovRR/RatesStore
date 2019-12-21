using RatesStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.EF
{
    public class RatesDbContext : DbContext
    {
        public DbSet<Rate> Rates { get; set; }
        public DbSet<RateRelation> RatesRelations { get; set; }
        public DbSet<RateRequestHistory> RatesRequestHistory { get; set; }
        public DbSet<Log> Logs { get; set; }

        public RatesDbContext()
            : base("DBConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RateRelation>().Property(rate => rate.Cost).HasPrecision(18, 4);
            base.OnModelCreating(modelBuilder);
        }
    }
}
