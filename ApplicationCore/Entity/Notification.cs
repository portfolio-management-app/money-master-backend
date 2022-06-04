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
    public class Notification
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }

        public int PortfolioId { get; set; }

        public string AssetName { get; set; }

        public string AssetType { get; set; }


        public string Currency { get; set; }

        public string CoinCode { get; set; } = null;

        public string StockCode { get; set; } = null;

        public double ThreadHoldAmount { get; set; }


    }
}