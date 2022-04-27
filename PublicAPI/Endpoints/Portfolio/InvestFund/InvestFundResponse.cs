namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class InvestFundResponse
    {
        public decimal CurrentAmount { get; set; }
        public bool IsDeleted { get; set; }
        public int PortfolioId { get; set; }
    }
}