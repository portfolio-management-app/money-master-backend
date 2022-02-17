namespace ApplicationCore.Entity.Asset
{
    public class InterestAsset: PersonalAsset
    {
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
    }
}