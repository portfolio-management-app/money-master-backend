using System;
using ApplicationCore.Entity.Transactions;

namespace PublicAPI.Endpoints.Portfolio
{
    public class TransactionResponse
    {
        public string SingleAssetTransactionType { get; set; }
        public int Id { get; set; }
        public int ReferentialAssetId { get; set; }
        public string ReferentialAssetType { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChanged { get; set; }

        public TransactionResponse(SingleAssetTransaction assetTransaction)
        {
            SingleAssetTransactionType = assetTransaction.SingleAssetTransactionTypes switch
            {
                SingleAssetTransactionTypes.NewAsset => "newAsset",
                SingleAssetTransactionTypes.AddValue => "addValue",
                SingleAssetTransactionTypes.WithdrawValue => "withdrawValue",
                SingleAssetTransactionTypes.SellAsset => "sellAsset",
                _ => throw new ArgumentOutOfRangeException()
            };
            Id = assetTransaction.Id;
            ReferentialAssetId = assetTransaction.ReferentialAssetId;
            ReferentialAssetType = assetTransaction.ReferentialAssetType;
            Amount = assetTransaction.Amount;
            CurrencyCode = assetTransaction.CurrencyCode;
            CreatedAt = assetTransaction.CreatedAt;
            LastChanged = assetTransaction.LastChanged;
        }
    }
}