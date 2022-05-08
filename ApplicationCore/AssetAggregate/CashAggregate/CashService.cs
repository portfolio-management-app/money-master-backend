using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using Mapster;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public class CashService : ICashService
    {
        private readonly IBaseRepository<CashAsset> _cashRepository;
        private readonly IInvestFundService _investFundService;
        private readonly ExternalPriceFacade _priceFacade;

        public CashService(IBaseRepository<CashAsset> cashRepository, 
             IInvestFundService investFundService, ExternalPriceFacade priceFacade)
        {
            _cashRepository = cashRepository;
            _investFundService = investFundService;
            _priceFacade = priceFacade;
        }

        public CashAsset GetById(int assetId)
        {
            return _cashRepository.GetFirst(c => c.Id == assetId);
        }

        public async Task<CashAsset> CreateNewCashAsset(int portfolioId, CashDto dto)
        {
            var newAsset = dto.Adapt<CashAsset>();
            newAsset.PortfolioId = portfolioId;

            _cashRepository.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _cashRepository.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public List<CashAsset> ListByPortfolio(int portfolioId)
        {
            return _cashRepository.List(c => c.PortfolioId == portfolioId).ToList();
        }

        public CashAsset SetAssetToDelete(int assetId)
        {
            var found = _cashRepository.GetFirst(c => c.Id == assetId);
            if (found is null)
                return null;
            _cashRepository.SetToDeleted(found); 
            return found; 
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cashAssets = ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cashAssets
                    .Select
                    (cash =>
                        cash.CalculateValueInCurrency(currencyCode, _priceFacade
                        ));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }
    }
}