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
        
        public string ReferentialAssetName { get; set; }
        public int? DestinationAssetId { get; set; } = null;
        public string DestinationAssetType { get; set; } = null;
        public string DestinationAssetName { get; set; } = null; 
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
                SingleAssetTransactionTypes.MoveToFund => "moveToFund", 
                _ => throw new ArgumentOutOfRangeException()
            };
            Id = assetTransaction.Id;
            ReferentialAssetId = assetTransaction.ReferentialAssetId;
            ReferentialAssetType = assetTransaction.ReferentialAssetType;
            ReferentialAssetName = assetTransaction.ReferentialAssetName; 
            Amount = assetTransaction.Amount;
            CurrencyCode = assetTransaction.CurrencyCode;
            CreatedAt = assetTransaction.CreatedAt;
            LastChanged = assetTransaction.LastChanged;
            DestinationAssetId = assetTransaction.DestinationAssetId;
            DestinationAssetType = assetTransaction.DestinationAssetType;
            DestinationAssetName = assetTransaction.DestinationAssetName; 
        }
    }
}