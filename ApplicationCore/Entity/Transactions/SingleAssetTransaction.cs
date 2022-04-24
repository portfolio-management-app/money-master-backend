namespace ApplicationCore.Entity.Transactions
{
    public class SingleAssetTransaction : Transaction
    {
        public SingleAssetTransactionTypes SingleAssetTransactionTypes { get; set; }
        public SingleAssetTransactionDestination SingleAssetTransactionDestination { get; set; }

        public SingleAssetTransaction(string referentialAssetType, int referentialAssetId, decimal amount,
            string currencyCode, SingleAssetTransactionTypes singleAssetTransactionTypes) :
            base(referentialAssetType, referentialAssetId, amount, currencyCode)
        {
            SingleAssetTransactionTypes = singleAssetTransactionTypes;
        }

        public SingleAssetTransaction()
        {
        }
    }
}