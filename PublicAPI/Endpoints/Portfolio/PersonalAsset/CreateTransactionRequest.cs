using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset
{
    public class CreateTransactionCommand
    {
        
        [FromBody]public decimal Amount { get; set; } 
        /// <summary>
        /// for example: 3 bitcoin -> send 3. null by default
        /// </summary>
        [FromBody ]public decimal? AmountInDestinationAssetUnit { get; set; }
        [FromBody]public string CurrencyCode { get; set; } 
        [FromBody]public string TransactionType { get; set; }
        [FromBody]public int? DestinationAssetId { get; set; } = null;
        [FromBody]public string DestinationAssetType { get; set; } = null;
        [FromBody]public bool IsTransferringAll { get; set; }
    }
    public class CreateTransactionRequest
    {
        [FromBody] public CreateTransactionCommand CreateTransactionCommand { get; set; }
        
        [FromRoute] public int PortfolioId { get; set; }
        [FromRoute] public int AssetId { get; set; }
    }
}