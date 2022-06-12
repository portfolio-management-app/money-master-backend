using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.InvestFundAggregate
{
    public class InvestFundService : IInvestFundService
    {
        private readonly IBaseRepository<InvestFund> _investFundRepository;
        private readonly IBaseRepository<InvestFundTransaction> _investFundTransactionRepository;
        private readonly IBaseRepository<SingleAssetTransaction> _assetTransactionRepository;
        private readonly ExternalPriceFacade _priceFacade;
        private string LackAmountErrorMessage => "Insufficient value left in asset";

        public InvestFundService(IBaseRepository<InvestFund> investFundRepository,
            IBaseRepository<InvestFundTransaction> investFundTransactionRepository,
            ExternalPriceFacade priceFacade, IBaseRepository<SingleAssetTransaction> assetTransactionRepository)
        {
            _investFundRepository = investFundRepository;
            _investFundTransactionRepository = investFundTransactionRepository;
            _priceFacade = priceFacade;
            _assetTransactionRepository = assetTransactionRepository;
        }

        public async Task<bool> BuyUsingInvestFund(int portfolioId, PersonalAsset buyingAsset)
        {
            var foundFund = _investFundRepository.GetFirst(i => i.PortfolioId == portfolioId,
                i => i.Include(fund => fund.Portfolio));

            var fundCurrency = foundFund.Portfolio.InitialCurrency;

            var assetValueInFundCurrency = await buyingAsset.CalculateValueInCurrency(fundCurrency, _priceFacade);
            if (foundFund.CurrentAmount < assetValueInFundCurrency) return false;

            foundFund.CurrentAmount -= assetValueInFundCurrency;
            var newOutgoingTransaction = new InvestFundTransaction(buyingAsset.GetAssetType(), buyingAsset.Id,
                assetValueInFundCurrency, fundCurrency, foundFund.Id, false)
            {
                ReferentialAssetName = buyingAsset.Name
            };
            _investFundTransactionRepository.Insert(newOutgoingTransaction);

            return true;
        }

        public InvestFund AddNewInvestFundToPortfolio(int portfolioId)
        {
            var newFund = new InvestFund()
            {
                PortfolioId = portfolioId,
                CurrentAmount = 0
            };
            _investFundRepository.Insert(newFund);
            return newFund;
        }

        public async Task<InvestFund> EditCurrency(int portfolioId, string newCurrencyCode)
        {
            var fund = GetInvestFundByPortfolio(portfolioId);
            // exchange
            var newCurrencyValue =
                await _priceFacade.CurrencyRateRepository.GetRateObject(fund.Portfolio.InitialCurrency);
            var rate = newCurrencyValue.GetValue(newCurrencyCode);

            fund.CurrentAmount = rate * fund.CurrentAmount;
            _investFundRepository.Update(fund);
            return fund;
        }

        public async Task WithdrawFromInvestFund(int requestPortfolioId, decimal amount,
            string currencyCode)
        {
            var investFund = GetInvestFundByPortfolio(requestPortfolioId);
            var withdrawAmountInFundCurrency = decimal.Zero;

            var rateObj = await _priceFacade.CurrencyRateRepository.GetRateObject(currencyCode);
            withdrawAmountInFundCurrency = rateObj.GetValue(investFund.Portfolio.InitialCurrency) * amount;
            if (withdrawAmountInFundCurrency > investFund.CurrentAmount)
                throw new OperationCanceledException("Insufficient amount in fund");

            investFund.CurrentAmount -= withdrawAmountInFundCurrency;
            _investFundRepository.Update(investFund);
        }

        public InvestFund GetInvestFundByPortfolio(int portfolioId)
        {
            return _investFundRepository.GetFirst(fund => fund.PortfolioId == portfolioId,
                i => i.Include(inf => inf.Portfolio));
        }

        public List<SingleAssetTransaction> GetInvestFundTransactionByPortfolio(int portfolioId)
        {
            return _assetTransactionRepository
                .List(t =>t.PortfolioId == portfolioId &&
            (t.ReferentialAssetType == "fund" || t.DestinationAssetType == "fund " ))
            .ToList(); 
        }
    }


}