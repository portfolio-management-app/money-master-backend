using System;
using ApplicationCore.Entity;

namespace ApplicationCore.AssetAggregate.CryptoAggregate.DTOs
{
    public class CryptoDto
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; } = DateTime.Now;
        public decimal BuyPrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string Description { get; set; }
        public string CryptoCoinCode { get; set; }
    }
}