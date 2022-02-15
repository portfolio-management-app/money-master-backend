namespace ApplicationCore.Entity.Asset
{
    public class InterestAsset: PersonalAsset
    {
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day
        
        // Reinstate: continue to keep in asset and keep interest rate 
        // Not Reinstate:  withdraw to cash at the end of term 
    }
}