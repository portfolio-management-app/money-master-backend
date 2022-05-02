using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset
{
    // Limit the type of transaction allowed here 
    public class AddTransactionRequest
    {
        public int ReferentialAssetId { get; set; }
        [CustomAllowedInputValidation(AllowableValues =
            new[] { "bankSaving", "cash", "crypto", "custom", "realEstate", "stock" })]
        public string ReferentialAssetType { get; set; }

        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsTransferringAll { get; set; }
    }
}