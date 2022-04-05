namespace ApplicationCore.Entity.Asset
{
    public class Crypto: PersonalAsset
    {
        public string CryptoCoinCode { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}