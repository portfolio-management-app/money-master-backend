using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class EditCashCommand
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
    public class EditCashRequest
    {
        [FromRoute] public int PortfolioId { get; set; }
        [FromRoute] public int CashId { get; set; }
        [FromBody] public EditCashCommand EditCashCommand { get; set; }
    }
}