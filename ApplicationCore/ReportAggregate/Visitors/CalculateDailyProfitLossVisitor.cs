using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
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

        private decimal? DecideHistoryAssetSpecificAmountHelper(SingleAssetTransaction transaction)
        {
            return transaction.SingleAssetTransactionType switch
            {
                SingleAssetTransactionType.BuyFromFund => transaction
                    .AmountOfDestinationAfterCreatingTransactionInSpecificUnit,
                SingleAssetTransactionType.BuyFromCash => transaction
                    .AmountOfDestinationAfterCreatingTransactionInSpecificUnit,
                SingleAssetTransactionType.AddValue => transaction
                    .AmountOfDestinationAfterCreatingTransactionInSpecificUnit,
                SingleAssetTransactionType.BuyFromOutside => transaction
                    .AmountOfDestinationAfterCreatingTransactionInSpecificUnit,
                _ => transaction.AmountOfSourceAssetAfterCreatingTransactionInSpecificUnit
            };
        }

        public override async Task<IEnumerable<ProfitLossBasis>> VisitCrypto(Crypto asset)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var result = new List<ProfitLossBasis>();
            decimal currentAssetUnitAmount = 0;
            decimal accumulatedTransactionValues = 0;
            for (var time = asset.InputDay; time < DateTime.Now; time += TimeSpan.FromDays(1))
            {
                var isCurrentTimePeriod = false;
                var startTime = time;
                if (startTime + TimeSpan.FromDays(1) >= DateTime.Now) isCurrentTimePeriod = true;
                var basePrice = isCurrentTimePeriod
                    ? await _priceFacade.CryptoRateRepository
                        .GetCurrentPriceInCurrency(asset.CryptoCoinCode, asset.CurrencyCode)
                    : await _priceFacade.CryptoRateRepository
                        .GetPastPriceInCurrency(asset.CryptoCoinCode,
                            asset.CurrencyCode, startTime + TimeSpan.FromDays(1));

                var subListTransactions = listTransaction
                    .Where(t => t.CreatedAt >= startTime && t.CreatedAt < startTime + TimeSpan.FromDays(1));
                var subListTransactionsArr =
                    subListTransactions as SingleAssetTransaction[] ?? subListTransactions.ToArray();
                var sellAndBuyDifferenceInPeriod = await
                    _assetTransactionService.CalculateSubTransactionProfitLoss(subListTransactionsArr,
                        asset.CurrencyCode);

                var lastPeriodTransaction = subListTransactionsArr.LastOrDefault();
                if (lastPeriodTransaction is not null)
                    currentAssetUnitAmount =
                        DecideHistoryAssetSpecificAmountHelper(lastPeriodTransaction) ?? 0;
                accumulatedTransactionValues += sellAndBuyDifferenceInPeriod;

                result.Add(new ProfitLossBasis()
                {
                    Amount = currentAssetUnitAmount * basePrice + accumulatedTransactionValues,
                    StartTime = startTime,
                    EndTime = startTime + TimeSpan.FromDays(1),
                    Unit = asset.CurrencyCode
                });
            }

            return result;
        }
    }
}