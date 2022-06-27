namespace PublicAPI.Endpoints.Portfolio
{
    public class CreatePortfolioRequest
    {
        public string Name { get; set; }
        public decimal InitialCash { get; set; }
        
        public string InitialCashName { get; set; }

        public string InitialCashDescription { get; set; }
        public string InitialCurrency { get; set; }
    }
}