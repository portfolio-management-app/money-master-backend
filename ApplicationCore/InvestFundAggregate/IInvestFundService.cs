using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;

namespace ApplicationCore.InvestFundAggregate
{
    public interface IInvestFundService
    {
        InvestFund GetInvestFundByPortfolio(int portfolioId);
        List<SingleAssetTransaction> GetInvestFundTransactionByPortfolio(int portfolioId);


        Task<bool> BuyUsingInvestFund(int portfolioId, PersonalAsset buyingAsset);

        InvestFund AddNewInvestFundToPortfolio(int portfolioId);

        Task<InvestFund> EditCurrency(int portfolioId, string newCurrencyCode);


        Task WithdrawFromInvestFund(int requestPortfolioId, decimal amount, string currencyCode);
    }
}