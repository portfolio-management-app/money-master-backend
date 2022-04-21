namespace ApplicationCore.Entity
{
    public class InvestFund : BaseEntity
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}