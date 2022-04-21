using System;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.InvestFundAggregate
{
    public class InvestFundService: IInvestFundService
    {

        private readonly IBaseRepository<InvestFund> _investFundRepository;
        private readonly IBaseRepository<InvestFundTransaction> _investFundTransactionRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private string LackAmountErrorMessage => "Insufficient value left in asset";

        public InvestFundService(IBaseRepository<InvestFund> investFundRepository, IBaseRepository<InvestFundTransaction> investFundTransactionRepository, ICurrencyRateRepository currencyRateRepository)
        {
            _investFundRepository = investFundRepository;
            _investFundTransactionRepository = investFundTransactionRepository;
            _currencyRateRepository = currencyRateRepository;
        }

        private InvestFund AddNewInvestFundToPortfolio(int portfolioId)
        {
            var newFund = new InvestFund()
            {
                PortfolioId = portfolioId,
                CurrentAmount = 0,
            };
            _investFundRepository.Insert(newFund);
            return newFund;
        }

        public InvestFund GetInvestFundByPortfolio(int portfolioId)
        {
            return _investFundRepository.GetFirst(fund => fund.PortfolioId == portfolioId, 
                i => i.Include(inf => inf.Portfolio));
        }

        public async Task<InvestFundTransaction> AddToInvestFund(int portfolioId, PersonalAsset asset, decimal amount, string currencyCode)
        {
            var investFund = GetInvestFundByPortfolio(portfolioId) ?? AddNewInvestFundToPortfolio(portfolioId);

            if (!await asset.Withdraw(amount, currencyCode, _currencyRateRepository))
            {
                throw new OperationCanceledException(LackAmountErrorMessage);
            };
            if(investFund.Portfolio.InitialCurrency == currencyCode)
                investFund.CurrentAmount += amount;
            else
            {
                var rateObj = await _currencyRateRepository.GetRateObject(currencyCode);
                var realAmountToAdd = rateObj.GetValue(investFund.Portfolio.InitialCurrency) * amount;
                investFund.CurrentAmount += realAmountToAdd;
            }
            _investFundRepository.Update(investFund);

            var newFundTransaction =
                new InvestFundTransaction(asset.GetAssetType(), asset.Id, amount, currencyCode, investFund.Id, true);
            _investFundTransactionRepository.Insert(newFundTransaction);
            return newFundTransaction;
        }

        public Task<InvestFundTransaction> WithdrawFromInvestFund(int portfolioId, PersonalAsset asset, decimal amount, string currencyCode)
        {
            throw new NotImplementedException();
        }
    }
}