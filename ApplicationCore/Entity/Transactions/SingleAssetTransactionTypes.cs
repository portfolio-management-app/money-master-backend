namespace ApplicationCore.Entity.Transactions
{
    public enum SingleAssetTransactionTypes
    {
        NewAsset,
        AddValue, // buy more
        WithdrawValue, // withdraw from asset to cash 
        MoveToFund,
        BuyFromFund,
        BuyFromCash, 
    }
}