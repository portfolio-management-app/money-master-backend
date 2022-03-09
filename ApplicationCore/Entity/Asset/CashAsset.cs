namespace ApplicationCore.Entity.Asset
{
    public class CashAsset : PersonalAsset
    {
        public double CurrentAmount { get; set; } = default;
    }
}