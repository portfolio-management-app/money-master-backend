using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    public class EditPortfolioCommand
    {
        public string NewName { get; set; }
        public string NewCurrency { get; set; }
    }

    public class EditPortfolioRequest
    {
        [FromBody] public EditPortfolioCommand EditPortfolioCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}