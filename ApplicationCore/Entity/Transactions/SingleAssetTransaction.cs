namespace ApplicationCore.Entity.Transactions
{
    public class SingleAssetTransaction : Transaction
    {
        public SingleAssetTransactionTypes SingleAssetTransactionTypes { get; set; }
        public SingleAssetTransactionDestination SingleAssetTransactionDestination { get; set; }
        public int? DestinationAssetId { get; set; } = null;
        public string DestinationAssetType { get; set; } = null;
        public string DestinationAssetName { get; set; } = null; 

        public SingleAssetTransaction(string referentialAssetType, int referentialAssetId, decimal amount,
            string currencyCode, SingleAssetTransactionTypes singleAssetTransactionTypes, int? destinationAssetId,string destinationAssetType) :
            base(referentialAssetType, referentialAssetId, amount, currencyCode)
        {
            SingleAssetTransactionTypes = singleAssetTransactionTypes;
            DestinationAssetId = destinationAssetId;
            DestinationAssetType = destinationAssetType;
        }

        public SingleAssetTransaction(){

        }
    }
}