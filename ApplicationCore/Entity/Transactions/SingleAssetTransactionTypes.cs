namespace ApplicationCore.Entity.Transactions
{
    public enum SingleAssetTransactionTypes
    {
        
        AddValue = 1, // buy more
        WithdrawToCash = 2, // withdraw from asset to cash 
        MoveToFund = 4,
        BuyFromFund = 8,
        BuyFromCash = 16, 
        BuyFromOutside =32 
    }
}