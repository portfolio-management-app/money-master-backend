using ApplicationCore.PortfolioAggregate;


namespace ApplicationCore.Entity
{
    public class Portfolio : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public decimal InitialCash { get; set; }
        public string InitialCurrency { get; set; }


        public Portfolio(int userId, string name, decimal initialCash, string initialCurrency)
        {
            Name = name;
            UserId = userId;
            InitialCash = initialCash;
            InitialCurrency = initialCurrency;
        }

        public decimal CalculateSum(IPortfolioService portfolioService)
        {
            return decimal.Zero;
        }
    }
}