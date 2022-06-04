using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using ApplicationCore.ParallelAsync;
using Mapster;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public class CryptoService : ICryptoService
    {
        private readonly IBaseRepository<Crypto> _cryptoRepository;
        private readonly IInvestFundService _investFundService;
        private readonly ICashService _cashService;
        private readonly ExternalPriceFacade _priceFacade;

        public CryptoService(IBaseRepository<Crypto> cryptoRepository,
            IInvestFundService investFundService, ExternalPriceFacade priceFacade, ICashService cashService)
        {
            _cryptoRepository = cryptoRepository;
            _investFundService = investFundService;
            _priceFacade = priceFacade;
            _cashService = cashService;
        }


        public Crypto GetById(int assetId)
        {
            return _cryptoRepository.GetFirst(c => c.Id == assetId);
        }

        public async Task<Crypto> CreateNewCryptoAsset(int portfolioId, CryptoDto dto)
        {
            if (dto.IsUsingCash && dto.UsingCashId is not null && !dto.IsUsingInvestFund)
            {
                var cashId = dto.UsingCashId;

                var foundCash = _cashService.GetById(cashId.Value);
                if (foundCash is null)
                    throw new InvalidOperationException("Cash not found");
                var withdrawResult = await foundCash.Withdraw(dto.PurchasePrice * dto.CurrentAmountHolding, dto.CurrencyCode, _priceFacade);

                if (!withdrawResult)
                    throw new InvalidOperationException("The specified cash does not have sufficient amount");
            }
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
            var listCrypto = _cryptoRepository.List(c => c.PortfolioId == portfolioId).ToList();

            await AsyncMethod.ParallelForEachAsync(listCrypto, async (crypto) =>
            {
                try
                {
                    crypto.CurrentPrice = await _priceFacade.CryptoRateRepository.GetCurrentPriceInCurrency(crypto.CryptoCoinCode, crypto.CurrencyCode);
                }
                catch (Exception ex)
                {
                    throw new OperationCanceledException($"Error when get current crypto price {ex.Message}");
                }
            });


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
                    crypto.CalculateValueInCurrency(currencyCode, _priceFacade
                        ));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }

        public Crypto EditCrypto(int cryptoId, EditCryptoDto dto)
        {
            var cryptoAsset = GetById(cryptoId);
            if (cryptoAsset is null)
                return null;
            cryptoAsset.CurrentAmountHolding = dto.CurrentAmountHolding;
            cryptoAsset.Description = dto.Description;
            cryptoAsset.Name = dto.Name;

            _cryptoRepository.Update(cryptoAsset);

            return cryptoAsset;
        }
    }
}