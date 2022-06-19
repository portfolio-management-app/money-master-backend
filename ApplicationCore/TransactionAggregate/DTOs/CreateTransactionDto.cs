namespace ApplicationCore.TransactionAggregate.DTOs
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }

        /// <summary>
        /// for example: 3 bitcoin -> send 3. null by default
        /// </summary>
        public decimal? AmountInDestinationAssetUnit { get; set; }

        public decimal? AmountOfReferentialAssetBeforeCreatingTransaction { get; set; }
        public string CurrencyCode { get; set; }
        public int? DestinationAssetId { get; set; } = null;
        public string DestinationAssetType { get; set; } = null;
        public int? ReferentialAssetId { get; set; } = null;
        public string ReferentialAssetType { get; set; } = null;
        public bool IsTransferringAll { get; set; }
        public bool IsUsingFundAsSource { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }
}