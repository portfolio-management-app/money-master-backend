using System;
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
        private readonly ExternalPriceFacade _priceFacade;
        private string LackAmountErrorMessage => "Insufficient value left in asset";

        public InvestFundService(IBaseRepository<InvestFund> investFundRepository,
            IBaseRepository<InvestFundTransaction> investFundTransactionRepository,
             ExternalPriceFacade priceFacade)
        {
            _investFundRepository = investFundRepository;
            _investFundTransactionRepository = investFundTransactionRepository;
            _priceFacade = priceFacade;
        }

        public async Task<bool> BuyUsingInvestFund(int portfolioId, PersonalAsset buyingAsset)
        {
            var foundFund = _investFundRepository.GetFirst(i => i.PortfolioId == portfolioId, i => i.Include(fund => fund.Portfolio));

            var fundCurrency = foundFund.Portfolio.InitialCurrency;

            var assetValueInFundCurrency = await buyingAsset.CalculateValueInCurrency(fundCurrency, _priceFacade);
            if (foundFund.CurrentAmount < assetValueInFundCurrency)
            {
                return false;
            }

            foundFund.CurrentAmount -= assetValueInFundCurrency;
            var newOutgoingTransaction = new InvestFundTransaction(buyingAsset.GetAssetType(), buyingAsset.Id,
                assetValueInFundCurrency, fundCurrency, foundFund.Id, false);
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

        public InvestFund GetInvestFundByPortfolio(int portfolioId)
        {
            return _investFundRepository.GetFirst(fund => fund.PortfolioId == portfolioId,
                i => i.Include(inf => inf.Portfolio));
        }

        public async Task<InvestFundTransaction> AddToInvestFund(int portfolioId, PersonalAsset asset, decimal amount,
            string currencyCode, bool isTransferringAll)
        {
            var investFund = GetInvestFundByPortfolio(portfolioId) ?? AddNewInvestFundToPortfolio(portfolioId);
            var assetType = asset.GetAssetType();
            var mandatoryWithdrawAll = new[] { "bankSaving", "realEstate" };
            var fundCurrencyCode = investFund.Portfolio.InitialCurrency;
            decimal withdrawAmount = 0;
            if (isTransferringAll)
            {
                withdrawAmount = await asset.CalculateValueInCurrency(fundCurrencyCode,
                    _priceFacade); 
                investFund.CurrentAmount += withdrawAmount;
                await asset.WithdrawAll();
            }
            else
            {
                if (mandatoryWithdrawAll.Contains(assetType))
                    throw new OperationCanceledException($"Not allowed for partial withdraw: {assetType}");
                if (!await asset.Withdraw(amount, currencyCode, _priceFacade))
                    throw new OperationCanceledException(LackAmountErrorMessage);
                if (investFund.Portfolio.InitialCurrency == currencyCode)
                {
                    investFund.CurrentAmount += amount;
                }
                else
                {
                    var rateObj = await _priceFacade.CurrencyRateRepository.GetRateObject(currencyCode);
                    var realAmountToAdd = rateObj.GetValue(investFund.Portfolio.InitialCurrency) * amount;
                    investFund.CurrentAmount += realAmountToAdd;
                    withdrawAmount = amount;
                }
            }

            _investFundRepository.Update(investFund);
            var newFundTransaction =
                new InvestFundTransaction
                (asset.GetAssetType()
                    , asset.Id,
                    isTransferringAll ? withdrawAmount : amount,
                    currencyCode,
                    investFund.Id,
                    true);
            _investFundTransactionRepository.Insert(newFundTransaction);
            return newFundTransaction;
        }

        public Task<InvestFundTransaction> WithdrawFromInvestFund(int portfolioId, PersonalAsset asset, decimal amount,
            string currencyCode)
        {
            throw new NotImplementedException();
        }
    }
}