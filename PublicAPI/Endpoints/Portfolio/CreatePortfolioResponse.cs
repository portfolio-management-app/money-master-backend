namespace PublicAPI.Endpoints.Portfolio
{
    public class CreatePortfolioResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double InitialCash { get; set; }
        public string InitialCurrency { get; set; }
    }
}