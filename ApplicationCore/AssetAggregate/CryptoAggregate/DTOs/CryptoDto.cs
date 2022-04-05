using System;
using ApplicationCore.Entity;

namespace ApplicationCore.AssetAggregate.CryptoAggregate.DTOs
{
    public class CryptoDto
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; } = DateTime.Now;
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public string Description { get; set; }
        public string CryptoCoinCode { get; set; }
    }
}