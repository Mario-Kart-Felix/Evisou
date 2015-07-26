using Evisou.Crm.Contract;
using Evisou.Core.Config;
using Evisou.Core.Log;
using Evisou.Framework.DAL;
using System.Data.Entity;

namespace Evisou.Crm.DAL
{
    public class CrmDbContext : DbContextBase
    {
        public CrmDbContext()
            : base(CachedConfigContext.Current.DaoConfig.Crm, new LogDbContext())
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CrmDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<VisitRecord> VisitRecords { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Area> Areas { get; set; }
    }
}
