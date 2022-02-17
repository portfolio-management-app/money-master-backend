namespace ApplicationCore.Entity.Asset
{
    public class BankSavingAsset: InterestAsset
    {
        public bool IsGoingToReinState { get; set; }
        // Reinstate: continue to keep in asset and keep interest rate 
        // Not Reinstate:  withdraw to cash at the end of term 
    }
}