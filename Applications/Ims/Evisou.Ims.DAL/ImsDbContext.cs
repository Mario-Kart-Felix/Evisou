using Evisou.Core.Config;
using Evisou.Core.Log;
using Evisou.Framework.DAL;
using Evisou.Ims.Contract.Model;
using Evisou.Ims.Contract.Model.PayPal;
using System.Data.Entity;


namespace Evisou.Ims.DAL
{
    public class ImsDbContext : DbContextBase
    {
        public ImsDbContext()
            : base(CachedConfigContext.Current.DaoConfig.Ims, new LogDbContext())
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
             Database.SetInitializer<ImsDbContext>(null);
             modelBuilder.Entity<PurchaseProduct>().HasKey(p => new { p.ProductID, p.PurchaseID });
             modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Suppliers)
                .Map(m =>
                {
                    m.ToTable("SupplierProducts");                   
                    m.MapLeftKey("SupplierId");
                    m.MapRightKey("ProductId");
                });

             modelBuilder.Entity<Product>()
              .HasMany(e => e.Images)
              .WithMany(e => e.Products)
              .Map(m =>
              {
                  m.ToTable("ProductImages");
                  m.MapLeftKey("ProductId");
                  m.MapRightKey("ImageId");
              });
             modelBuilder.Entity<PaymentTransactionType>().HasRequired(a => a.PayerInfo).WithRequiredPrincipal(b => b.PayPalPaymentTransaction).WillCascadeOnDelete();
             modelBuilder.Entity<PaymentTransactionType>().HasRequired(a => a.PaymentInfo).WithRequiredPrincipal(b => b.PayPalPaymentTransaction).WillCascadeOnDelete();
             modelBuilder.Entity<PaymentTransactionType>().HasRequired(a => a.PaymentItemInfo).WithRequiredPrincipal(b => b.PayPalPaymentTransaction).WillCascadeOnDelete();
             modelBuilder.Entity<PaymentTransactionType>().HasRequired(a => a.ReceiverInfo).WithRequiredPrincipal(b => b.PayPalPaymentTransaction).WillCascadeOnDelete();

             modelBuilder.Entity<PayerInfoType>().HasRequired(a => a.Address).WithRequiredPrincipal(b => b.PayPalPayerInfo).WillCascadeOnDelete();
             modelBuilder.Entity<PaymentItemInfoType>().HasRequired(a => a.Auction).WithRequiredPrincipal(b => b.PayPalPaymentItemInfo).WillCascadeOnDelete();
             modelBuilder.Entity<PaymentItemInfoType>().HasMany(a => a.PaymentItem).WithRequired(b => b.PayPalPaymentItemInfo).WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseProduct> PurchaseProducts { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        public DbSet<PaypalApi> PaypalApis { get; set; }

        public DbSet<Association> Associations { get; set; }

        // public DbSet<TransactionItem> TransactionItems { get; set; }

        #region new paypal
        public DbSet<PayPalTransaction> PayPalTransactions { get; set; }
        #endregion

        #region Paypal
        public DbSet<PaymentTransactionType> PayPalPaymentTransactions { get; set; }

      /*  public DbSet<UserSelectedOptionType> PayPalUserSelectedOption { get; set; }

        public DbSet<PayerInfoType> PayPalPayerInfo { get; set; }

        public DbSet<PaymentInfoType> PayPalPaymentInfo { get; set; }

        //public DbSet<FMFDetailsType> PayPalFMFDetails { get; set; }

        public DbSet<PaymentItemInfoType> PayPalPaymentItemInfo { get; set; }

        public DbSet<PaymentItemType> PayPalPaymentItems { get; set; }

        public DbSet<InvoiceItemType> PayPalInvoiceItem { get; set; }

        public DbSet<AdditionalFeeType> PayPalAdditionalFee { get; set; }

        public DbSet<DiscountType> PayPalDiscount { get; set; }

        public DbSet<OptionType> PayPalOption { get; set; }

        public DbSet<SubscriptionInfoType> PayPalSubscriptionInfo { get; set; }

        public DbSet<SubscriptionTermsType> PayPalSubscriptionTerms { get; set; }

        public DbSet<BasicAmountType> PayPalBasicAmount { get; set; }

        public DbSet<AuctionInfoType> PayPalAuctionInfo { get; set; }

        public DbSet<ReceiverInfoType> PayPalReceiverInfo { get; set; }

        public DbSet<AddressType> PayPalAddress { get; set; }

        public DbSet<PersonNameType> PayPalPersonName { get; set; }

        public DbSet<SellerDetailsType> PayPalSellerDetails { get; set; }

        public DbSet<TaxIdDetailsType> PayPalTaxIdDetails { get; set; }

        public DbSet<InstrumentDetailsType> PayPalInstrumentDetails { get; set; }*/
        #endregion
    }
}
