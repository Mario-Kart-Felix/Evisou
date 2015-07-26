using Evisou.Core.Config;
using Evisou.Core.Log;
using Evisou.Framework.DAL;
using Evisou.OA.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.OA.DAL
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
