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
        private readonly IStockPriceRepository _stockPriceRepository;

        public CashService(IBaseRepository<CashAsset> cashRepository, ICryptoRateRepository cryptoRateRepository,
            ICurrencyRateRepository currencyRateRepository, IStockPriceRepository stockPriceRepository)
        {
            _cashRepository = cashRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
            _stockPriceRepository = stockPriceRepository;
        }

        public CashAsset GetById(int assetId)
        {
            return _cashRepository.GetFirst(c => c.Id == assetId);
        }

        public CashAsset CreateNewCashAsset(int portfolioId, CashDto dto)
        {
            var newCashAsset = dto.Adapt<CashAsset>();
            newCashAsset.PortfolioId = portfolioId;

            _cashRepository.Insert(newCashAsset);
            return newCashAsset;
        }

        public List<CashAsset> ListByPortfolio(int portfolioId)
        {
            return _cashRepository.List(c => c.PortfolioId == portfolioId).ToList();
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cashAssets = ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cashAssets
                    .Select
                    (cash =>
                        cash.CalculateValueInCurrency(currencyCode, _currencyRateRepository,
                            _cryptoRateRepository, _stockPriceRepository));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }
    }
}