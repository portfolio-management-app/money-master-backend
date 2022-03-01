namespace PublicAPI.Endpoints.Portfolio
{
    public class CreatePortfolioRequest
    {
        public string Name { get; set; }
        public double InitialCash { get; set; }
        public string InitialCurrency { get; set; }
    }
}