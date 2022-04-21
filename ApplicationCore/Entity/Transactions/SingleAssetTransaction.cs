namespace ApplicationCore.Entity.Transactions
{
    public class SingleAssetTransaction: Transaction
    {
        public SingleAssetTransactionType SingleAssetTransactionType { get; set; }

        public SingleAssetTransaction( string referentialAssetType, int referentialAssetId, decimal amount, 
            string currencyCode,SingleAssetTransactionType singleAssetTransactionType): 
            base(referentialAssetType,referentialAssetId,amount,currencyCode)
        {
            SingleAssetTransactionType = singleAssetTransactionType;
        }
        public SingleAssetTransaction(){}
    }
}