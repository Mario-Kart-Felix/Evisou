using Evisou.Account.Contract;
using Evisou.Core.Config;
using Evisou.Core.Log;
using Evisou.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Account.DAL
{
    public class AccountContext : DbContextBase
    {
         public AccountContext()
           : base("Server = .; Database =EviousAccount; User ID = sa; Password=123456; Integrated Security=false;MultipleActiveResultSets=true")
            //: base("Data Source=192.169.63.61;Initial Catalog=EviousAccount;Persist Security Info=True;User ID=WebDb;Password=Hi6327582")
        {
            Configuration.LazyLoadingEnabled = false;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AccountDbContext>(null);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .Map(m =>
                {
                    m.ToTable("UserRole");
                    m.MapLeftKey("UserID");
                    m.MapRightKey("RoleID");
                });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LoginInfo> LoginInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<VerifyCode> VerifyCodes { get; set; }
    }

}
