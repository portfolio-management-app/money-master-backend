namespace ApplicationCore.Entity.Asset
{
    public class RealEstateAsset : PersonalAsset
    {
        public decimal BuyPrice { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}