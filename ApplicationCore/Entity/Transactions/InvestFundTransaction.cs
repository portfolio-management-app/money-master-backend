
namespace ApplicationCore.Entity.Transactions
{
    public class InvestFundTransaction: Transaction
    {
        public int InvestFundId { get; set; }
        public InvestFund  InvestFund { get; set; }
        public bool IsIngoing { get; set; }

        public InvestFundTransaction( string referentialAssetType, int referentialAssetId, decimal amount, 
                                                 string currencyCode, int investFundId, bool isIngoing)
            : base(referentialAssetType, referentialAssetId, amount, currencyCode)
        {
            InvestFundId = investFundId;
            IsIngoing = isIngoing;
        }
        
        public InvestFundTransaction(){}
    }
}