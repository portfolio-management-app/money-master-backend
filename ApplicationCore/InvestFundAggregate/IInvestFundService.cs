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
        List<InvestFundTransaction> GetInvestFundTransactionByPortfolio(int portfolioId); 

        Task<InvestFundTransaction> AddToInvestFund(int portfolioId, PersonalAsset asset, decimal amount,
            string currencyCode, bool isTransferringAll);

        Task<InvestFundTransaction> WithdrawFromInvestFund(int portfolioId, CashAsset asset, decimal amount,
            string currencyCode);

        Task<bool> BuyUsingInvestFund(int portfolioId, PersonalAsset buyingAsset); 

        InvestFund AddNewInvestFundToPortfolio(int portfolioId);

        Task<InvestFund> EditCurrency(int portfolioId, string newCurrencyCode); 


    }
}