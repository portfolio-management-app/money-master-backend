using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.Transactions
{
    public class CreateTransactionCommand
    {
        [FromBody] public decimal Amount { get; set; }

        /// <summary>
        /// for example: 3 bitcoin -> send 3. null by default
        /// </summary>
        [FromBody]
        public decimal? ValueOfReferentialAssetBeforeCreatingTransaction { get; set; }
        [FromBody]
        public decimal? AmountInDestinationAssetUnit { get; set; }

        [FromBody] public string CurrencyCode { get; set; }


        [CustomAllowedInputValidation(AllowableValues =
            new[] { "withdrawToCash", "withdrawToOutside", "addValue", "moveToFund" })]
        [FromBody]
        public string TransactionType { get; set; }

        [FromBody] public int? DestinationAssetId { get; set; } = null;
        [FromBody] public string DestinationAssetType { get; set; } = null;

        [FromBody] public int? ReferentialAssetId { get; set; } = null;
        [FromBody] public string ReferentialAssetType { get; set; } = null;
        [FromBody] public bool IsTransferringAll { get; set; }
        [FromBody] public bool IsUsingFundAsSource { get; set; }
        [FromBody] public decimal? Fee { get; set; }
        [FromBody] public decimal? Tax { get; set; }
    }

    public class CreateTransactionRequest
    {
        [FromBody] public CreateTransactionCommand CreateTransactionCommand { get; set; }

        [FromRoute] public int PortfolioId { get; set; }
    }
}