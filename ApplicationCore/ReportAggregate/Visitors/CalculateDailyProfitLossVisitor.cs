using System;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.Entity.Asset;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.TransactionAggregate;

namespace ApplicationCore.ReportAggregate.Visitors
{
    public class CalculateDailyProfitLossVisitor : CalculatePeriodProfitLossVisitor
    {
        private readonly ICryptoService _cryptoService;
        private readonly IAssetTransactionService _assetTransactionService;
        private readonly ExternalPriceFacade _priceFacade;

        public CalculateDailyProfitLossVisitor(ICryptoService cryptoService,
            IAssetTransactionService assetTransactionService, ExternalPriceFacade priceFacade)
        {
            _cryptoService = cryptoService;
            _assetTransactionService = assetTransactionService;
            _priceFacade = priceFacade;
        }

        public override async Task<ProfitLossBasis> VisitCrypto(Crypto asset)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var buyAndSellDifference =
                _assetTransactionService.CalculateSubTransactionProfitLoss(listTransaction, asset.CurrencyCode);
            var currentAssetValue = await asset.CalculateValueInCurrency(asset.CurrencyCode, _priceFacade);
            var priceYesterday =
                await _priceFacade
                    .CryptoRateRepository
                    .GetPastPriceInCurrency(asset.CryptoCoinCode, asset.CurrencyCode,
                        DateTime.Now - TimeSpan.FromDays(1));

            var yesterdayAssetValue = priceYesterday * 345345345; // dummy value

            var result = currentAssetValue - yesterdayAssetValue + buyAndSellDifference;
            return new ProfitLossBasis();
        }
    }
}