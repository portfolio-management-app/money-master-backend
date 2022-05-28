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
            return _cashRepository.GetFirst(c => c.Id == assetId && !c.IsDeleted);
        }

        public async Task<CashAsset> CreateNewCashAsset(int portfolioId, CashDto dto)
        {
            if (dto.IsUsingCash && dto.UsingCashId is not null && !dto.IsUsingInvestFund)
            {
                var cashId = dto.UsingCashId;

                var foundCash = GetById(cashId.Value);
                if (foundCash is null)
                    throw new InvalidOperationException("Cash not found");
                var withdrawResult = await foundCash.Withdraw(dto.Amount, dto.CurrencyCode, _priceFacade);

                if (!withdrawResult)
                    throw new InvalidOperationException("The specified cash does not have sufficient amount");
            }

            var newAsset = dto.Adapt<CashAsset>();
            newAsset.PortfolioId = portfolioId;
            _cashRepository.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _cashRepository.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public CashAsset EditCash(int cashId, EditCashDto dto)
        {
            var foundCash = GetById(cashId);
            if (foundCash is null)
                return null;
            foundCash.Amount = dto.Amount;
            foundCash.CurrencyCode = dto.Currency;
            foundCash.Name = dto.Name;
            foundCash.Description = dto.Description;

            _cashRepository.Update(foundCash);

            return foundCash;
        }

        public async Task<List<CashAsset>> ListByPortfolio(int portfolioId)
        {
            var result = _cashRepository.List(c => c.PortfolioId == portfolioId && !c.IsDeleted).ToList();
            return result;
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
            var cashAssets = await ListByPortfolio(portfolioId);
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