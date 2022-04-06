namespace ApplicationCore.Entity.Asset
{
    public class RealEstateAsset : PersonalAsset
    {
        public string InputCurrency { get; set; }
        public decimal InputMoneyAmount { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}