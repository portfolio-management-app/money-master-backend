namespace ApplicationCore.Entity.Asset
{
    public class InterestAsset : PersonalAsset
    {
        public decimal InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
    }
}