using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;
using ApplicationCore.TransactionAggregate;


namespace ApplicationCore.InvestFundAggregate

{
    public class InvestFundAsset : PersonalAsset
    {


        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            throw new NotImplementedException();
        }

        public override string GetAssetType()
        {
            return "fund";
        }

        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> AddValue(decimal amountInAssetUnit)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> WithdrawAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor, int period)
        {
            throw new NotImplementedException();
        }

        public override decimal GetAssetSpecificAmount()
        {
            throw new NotImplementedException();
        }

        public override string GetCurrency()
        {
            throw new NotImplementedException();
        }
    }



    public class InvestFundService : IInvestFundService
    {
        private readonly IBaseRepository<InvestFund> _investFundRepository;
        private readonly IBaseRepository<SingleAssetTransaction> _assetTransactionRepository;
        private readonly ExternalPriceFacade _priceFacade;


        private string LackAmountErrorMessage => "Insufficient value left in asset";

        public InvestFundService(IBaseRepository<InvestFund> investFundRepository,
            ExternalPriceFacade priceFacade, IBaseRepository<SingleAssetTransaction> assetTransactionRepository)
        {
            _investFundRepository = investFundRepository;
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
            _investFundRepository.Update(foundFund);
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

        public List<SingleAssetTransaction> GetInvestFundTransactionByPortfolio(int portfolioId, int? pageNumber, int? pageSize,
            DateTime? startDate, DateTime? endDate, string transactionType)
        {

            var fakeAsset = new InvestFundAsset();
            fakeAsset.PortfolioId = portfolioId;
            fakeAsset.Name = "fund";
            var listTransaction = _assetTransactionRepository.List(
              new TransactionWithPagingAndTimeSpec(fakeAsset, pageNumber, pageSize, startDate, endDate, transactionType)
          );
            return listTransaction.ToList();
        }
    }
}