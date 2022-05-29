using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class CreateCashCommand: BaseCreateRequest
    {
        [Required] public string CurrencyCode { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public string Name { get; set; }
        public DateTime InputDay { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }

    public class CreateCashRequest
    {
        [FromBody] public CreateCashCommand CreateCashCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}