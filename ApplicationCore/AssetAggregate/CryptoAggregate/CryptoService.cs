using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using Mapster;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public class CryptoService : ICryptoService
    {
        private readonly IBaseRepository<Crypto> _cryptoRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        private readonly IInvestFundService _investFundService;

        public CryptoService(IBaseRepository<Crypto> cryptoRepository, ICryptoRateRepository cryptoRateRepository,
            ICurrencyRateRepository currencyRateRepository, IStockPriceRepository stockPriceRepository,
            IInvestFundService investFundService)
        {
            _cryptoRepository = cryptoRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
            _stockPriceRepository = stockPriceRepository;
            _investFundService = investFundService;
        }


        public Crypto GetById(int assetId)
        {
            return _cryptoRepository.GetFirst(c => c.Id == assetId);
        }

        public async Task<Crypto> CreateNewCryptoAsset(int portfolioId, CryptoDto dto)
        {
            var newAsset = dto.Adapt<Crypto>();
            newAsset.PortfolioId = portfolioId;
            _cryptoRepository.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _cryptoRepository.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public List<Crypto> ListByPortfolio(int portfolioId)
        {
            var listCrypto = _cryptoRepository.List(c => c.PortfolioId == portfolioId).ToList();

            return listCrypto.ToList();
        }

        public Crypto SetAssetToDelete(int assetId)
        {
            var found = _cryptoRepository.GetFirst(c => c.Id == assetId);
            if (found is null)
                return null;
            _cryptoRepository.SetToDeleted(found);
            return found;
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cryptoAssets = ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cryptoAssets.Select(crypto =>
                    crypto.CalculateValueInCurrency(currencyCode, _currencyRateRepository, _cryptoRateRepository,
                        _stockPriceRepository));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }
    }
}