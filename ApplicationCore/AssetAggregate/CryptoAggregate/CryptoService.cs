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
        private readonly IInvestFundService _investFundService;
        private readonly ExternalPriceFacade _priceFacade;

        public CryptoService(IBaseRepository<Crypto> cryptoRepository, 
            IInvestFundService investFundService, ExternalPriceFacade priceFacade)
        {
            _cryptoRepository = cryptoRepository;
            _investFundService = investFundService;
            _priceFacade = priceFacade;
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

        public async Task<List<Crypto>> ListByPortfolio(int portfolioId)
        {
            var listCrypto =  _cryptoRepository.List(c => c.PortfolioId == portfolioId).ToList();

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
            var cryptoAssets = await ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cryptoAssets.Select(crypto =>
                    crypto.CalculateValueInCurrency(currencyCode,_priceFacade 
                        ));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }
    }
}