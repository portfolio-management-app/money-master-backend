using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationCore.Entity
{
    public class Notification : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int AssetId { get; set; }

        public int PortfolioId { get; set; }

        public string AssetName { get; set; }

        public string AssetType { get; set; }

        public string Currency { get; set; }

        public string CoinCode { get; set; }

        public string StockCode { get; set; }

        public decimal HighThreadHoldAmount { get; set; }

        public decimal LowThreadHoldAmount { get; set; }

        public bool IsHighOn { get; set; } = true;

        public bool IsLowOn { get; set; } = true;
    }
}