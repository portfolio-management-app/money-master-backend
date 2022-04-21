using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<BankSavingAsset> BankSavingAssets { get; set; }
        public DbSet<CustomInterestAsset> CustomInterestAssets { get; set; }
        public DbSet<CustomInterestAssetInfo> CustomInterestAssetInfos { get; set; }
        public DbSet<CashAsset> CashAssets { get; set; }
        public DbSet<RealEstateAsset> RealEstateAssets { get; set; }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<InvestFund> InvestFunds { get; set; }
        public DbSet<SingleAssetTransaction> SingleAssetTransactions { get; set; }
        public DbSet<InvestFundTransaction> InvestFundTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}