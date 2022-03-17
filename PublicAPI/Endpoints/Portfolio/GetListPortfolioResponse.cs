namespace PublicAPI.Endpoints.Portfolio
{
    public class GetListPortfolioResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double InitialCash { get; set; }
        public string InitialCurrency { get; set; }
        public decimal Sum { get; set; }
    }
}