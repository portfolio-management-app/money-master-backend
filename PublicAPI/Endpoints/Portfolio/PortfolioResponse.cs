namespace PublicAPI.Endpoints.Portfolio
{
    public class PortfolioResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double InitialCash { get; set; }
        public string InitialCurrency { get; set; }
        public bool IsDeleted { get; set;  }
    }
}