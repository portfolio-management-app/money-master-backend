namespace PublicAPI.Endpoints.Portfolio.PersonalAsset
{
    public class BaseCreateRequest
    {
        public bool IsUsingInvestFund { get; set; }
        public bool IsUsingCash { get; set; }
        public int? UsingCashId { get; set; }
    }
}