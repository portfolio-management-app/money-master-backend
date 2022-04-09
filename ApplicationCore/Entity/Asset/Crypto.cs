namespace ApplicationCore.Entity.Asset
{
    public class Crypto: PersonalAsset
    {
        
        public decimal BuyPrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string CryptoCoinCode { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}