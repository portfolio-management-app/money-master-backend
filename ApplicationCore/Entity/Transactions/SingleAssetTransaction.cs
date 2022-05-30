namespace ApplicationCore.Entity.Transactions
{
    public class SingleAssetTransaction : Transaction
    {
        public SingleAssetTransactionTypes SingleAssetTransactionTypes { get; set; }
        public int? DestinationAssetId { get; set; } = null;
        public string DestinationAssetType { get; set; } = null;
        public string DestinationAssetName { get; set; } = null; 
        
        public decimal DestinationAmount { get; set; }
        public string DestinationCurrency { get; set; }

        public SingleAssetTransaction(string referentialAssetType, int referentialAssetId, decimal amount,
            string currencyCode, SingleAssetTransactionTypes singleAssetTransactionTypes, int? destinationAssetId,string destinationAssetType, string destinationAssetName
            ,decimal destinationAmount,string destinationCurrency) :
            base(referentialAssetType, referentialAssetId, amount, currencyCode)
        {
            SingleAssetTransactionTypes = singleAssetTransactionTypes;
            DestinationAssetId = destinationAssetId;
            DestinationAssetType = destinationAssetType;
            DestinationAssetName = destinationAssetName;
            DestinationAmount = destinationAmount;
            DestinationCurrency = destinationCurrency; 
        }

        public SingleAssetTransaction(){

        }
    }
}