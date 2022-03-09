namespace ApplicationCore.Entity.Asset
{
    public class RealEstateAsset : PersonalAsset
    {
        public string Description { get; set; }
        public double CurrentPrice { get; set; }
    }
}