using Evious.Core.Config;
using Evious.Core.Log;
using Evious.Framework.DAL;
using Evious.OA.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.OA.DAL
{
    public class OADbContext : DbContextBase
    {
        public OADbContext()
            : base(CachedConfigContext.Current.DaoConfig.OA, new LogDbContext())
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<OADbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Branch> Branchs { get; set; }
    }
}
