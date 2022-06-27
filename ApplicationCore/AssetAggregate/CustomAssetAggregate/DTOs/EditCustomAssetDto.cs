namespace ApplicationCore.AssetAggregate.CustomAssetAggregate.DTOs
{
    public class EditCustomAssetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
    }
}