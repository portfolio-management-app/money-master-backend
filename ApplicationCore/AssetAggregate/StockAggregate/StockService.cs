using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using Mapster;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public class StockService : IStockService
    {
        private readonly IBaseRepository<Stock> _stockRepository;
        private readonly IInvestFundService _investFundService;
        private readonly ExternalPriceFacade _priceFacade;
        private readonly ICashService _cashService;

        public StockService(IBaseRepository<Stock> stockRepository,
            IInvestFundService investFundService, ExternalPriceFacade priceFacade, ICashService cashService)
        {
            _stockRepository = stockRepository;
            _investFundService = investFundService;
            _priceFacade = priceFacade;
            _cashService = cashService;
        }

        public async Task<Stock> CreateNewStockAsset(int portfolioId, StockDto dto)
        {
            if (dto.IsUsingCash && dto.UsingCashId is not null && !dto.IsUsingInvestFund)
            {
                var cashId = dto.UsingCashId;

                var foundCash = _cashService.GetById(cashId.Value);
                if (foundCash is null)
                    throw new InvalidOperationException("Cash not found");
                var withdrawResult = await foundCash.Withdraw(dto.PurchasePrice * dto.CurrentAmountHolding,
                    dto.InputCurrency, _priceFacade);

                if (!withdrawResult)
                    throw new InvalidOperationException("The specified cash does not have sufficient amount");
            }

            var newAsset = dto.Adapt<Stock>();
            newAsset.PortfolioId = portfolioId;
            _stockRepository.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _stockRepository.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var assets = await ListByPortfolio(portfolioId);
            var unifyCurrencyValue = assets.Select
            (stock =>
                stock.CalculateValueInCurrency(currencyCode, _priceFacade
                ));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }

        public Stock GetById(int assetId)
        {
            return _stockRepository.GetFirst(s => s.Id == assetId);
        }

        public async Task<List<Stock>> ListByPortfolio(int portfolioId)
        {
            var stocks = _stockRepository.List(s => s.PortfolioId == portfolioId).ToList();
            foreach (var stock in stocks) stock.CurrentPrice = await stock.GetCurrentPricePerUnit(_priceFacade);
            return stocks;
        }

        public Stock SetAssetToDelete(int assetId)
        {
            var found = _stockRepository.GetFirst(c => c.Id == assetId);
            if (found is null)
                return null;
            _stockRepository.SetToDeleted(found);
            return found;
        }
    }
}