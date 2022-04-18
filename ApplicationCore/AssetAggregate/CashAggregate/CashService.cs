using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public class CashService : ICashService
    {
        private readonly IBaseRepository<CashAsset> _cashRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        public CashService(IBaseRepository<CashAsset> cashRepository, ICryptoRateRepository cryptoRateRepository, ICurrencyRateRepository currencyRateRepository)
        {
            _cashRepository = cashRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
        }

        public CashAsset CreateNewCashAsset(int portfolioId, CashDto dto)
        {
            CashAsset newCashAsset = dto.Adapt<CashAsset>();
            newCashAsset.PortfolioId = portfolioId;

            _cashRepository.Insert(newCashAsset);
            return newCashAsset; 
        }

        public List<CashAsset> GetCashAssetsByPortfolio(int portfolioId)
        {
            return _cashRepository.List(c => c.PortfolioId == portfolioId).ToList(); 
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cashAssets = GetCashAssetsByPortfolio(portfolioId);
            var unifyCurrencyValue = 
                cashAssets
                    .Select
                    ( cash =>
                         cash.CalculateValueInCurrency(currencyCode, _currencyRateRepository,
                            _cryptoRateRepository));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }
    }
}