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
    public class CalculateProfitLossVisitor : IVisitor
    {
        private readonly IAssetTransactionService _assetTransactionService;
        private readonly ExternalPriceFacade _priceFacade;

        public CalculateProfitLossVisitor(
            IAssetTransactionService assetTransactionService, ExternalPriceFacade priceFacade)
        {
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

        public async Task<IEnumerable<ProfitLossBasis>> VisitCrypto(Crypto asset, int period = 1)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var result = new List<ProfitLossBasis>();
            decimal currentAssetUnitAmount = 0;
            decimal accumulatedTransactionValues = 0;
            for (var time = asset.InputDay; time < DateTime.Now; time += TimeSpan.FromDays(period))
            {
                var isCurrentTimePeriod = false;
                var startTime = time;
                if (startTime + TimeSpan.FromDays(period) >= DateTime.Now) isCurrentTimePeriod = true;
                var basePrice = isCurrentTimePeriod
                    ? await _priceFacade.CryptoRateRepository.GetCurrentPriceInCurrency(asset.CryptoCoinCode,
                        asset.GetCurrency())
                    : await _priceFacade.CryptoRateRepository
                        .GetPastPriceInCurrency(asset.CryptoCoinCode,
                            asset.CurrencyCode, startTime + TimeSpan.FromDays(period));

                var subListTransactions = listTransaction
                    .Where(t => t.CreatedAt >= startTime && t.CreatedAt < startTime + TimeSpan.FromDays(period));
                var subListTransactionsArr =
                    subListTransactions as SingleAssetTransaction[] ?? subListTransactions.ToArray();
                var sellAndBuyDifferenceInPeriod = await
                    _assetTransactionService.CalculateSubTransactionProfitLoss(subListTransactionsArr,
                        asset.GetCurrency());

                var lastPeriodTransaction = subListTransactionsArr.LastOrDefault();
                if (lastPeriodTransaction is not null)
                    currentAssetUnitAmount =
                        DecideHistoryAssetSpecificAmountHelper(lastPeriodTransaction) ?? 0;
                accumulatedTransactionValues += sellAndBuyDifferenceInPeriod;

                result.Add(new ProfitLossBasis()
                {
                    Amount = currentAssetUnitAmount * basePrice + accumulatedTransactionValues,
                    StartTime = startTime,
                    EndTime = startTime + TimeSpan.FromDays(period),
                    Unit = asset.CurrencyCode
                });
            }

            return result;
        }

        public async Task<IEnumerable<ProfitLossBasis>> VisitCash(CashAsset asset, int period = 1)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var result = new List<ProfitLossBasis>();
            decimal currentAssetUnitAmount = 0;
            decimal accumulatedTransactionValues = 0;
            for (var time = asset.InputDay; time < DateTime.Now; time += TimeSpan.FromDays(period))
            {
                var isCurrentTimePeriod = false;
                var startTime = time;
                if (startTime + TimeSpan.FromDays(period) >= DateTime.Now) isCurrentTimePeriod = true;
                decimal basePrice = 1;

                var subListTransactions = listTransaction
                    .Where(t => t.CreatedAt >= startTime && t.CreatedAt < startTime + TimeSpan.FromDays(period));
                var subListTransactionsArr =
                    subListTransactions as SingleAssetTransaction[] ?? subListTransactions.ToArray();
                var sellAndBuyDifferenceInPeriod = await
                    _assetTransactionService.CalculateSubTransactionProfitLoss(subListTransactionsArr,
                        asset.GetCurrency());

                var lastPeriodTransaction = subListTransactionsArr.LastOrDefault();
                if (lastPeriodTransaction is not null)
                    currentAssetUnitAmount =
                        DecideHistoryAssetSpecificAmountHelper(lastPeriodTransaction) ?? 0;
                accumulatedTransactionValues += sellAndBuyDifferenceInPeriod;

                result.Add(new ProfitLossBasis()
                {
                    Amount = currentAssetUnitAmount * basePrice + accumulatedTransactionValues,
                    StartTime = startTime,
                    EndTime = startTime + TimeSpan.FromDays(period),
                    Unit = asset.CurrencyCode
                });
            }

            return result;
        }

        private async Task<IEnumerable<ProfitLossBasis>> VisitInterestAsset(InterestAsset asset, int period = 1)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var result = new List<ProfitLossBasis>();
            decimal currentAssetUnitAmount = 0;
            decimal accumulatedTransactionValues = 0;
            for (var time = asset.InputDay; time < DateTime.Now; time += TimeSpan.FromDays(period))
            {
                var isCurrentTimePeriod = false;
                var startTime = time;
                if (startTime + TimeSpan.FromDays(period) >= DateTime.Now) isCurrentTimePeriod = true;
                var basePrice = isCurrentTimePeriod
                    ? await asset.CalculateValueInCurrency(asset.GetCurrency(), _priceFacade)
                    : await asset.CalculateValueInPastInCurrency
                        (startTime + TimeSpan.FromDays(period), asset.GetCurrency(), _priceFacade);

                var subListTransactions = listTransaction
                    .Where(t => t.CreatedAt >= startTime && t.CreatedAt < startTime + TimeSpan.FromDays(period));
                var subListTransactionsArr =
                    subListTransactions as SingleAssetTransaction[] ?? subListTransactions.ToArray();
                var sellAndBuyDifferenceInPeriod = await
                    _assetTransactionService.CalculateSubTransactionProfitLoss(subListTransactionsArr,
                        asset.GetCurrency());

                var lastPeriodTransaction = subListTransactionsArr.LastOrDefault();
                if (lastPeriodTransaction is not null)
                    currentAssetUnitAmount =
                        DecideHistoryAssetSpecificAmountHelper(lastPeriodTransaction) ?? 0;
                accumulatedTransactionValues += sellAndBuyDifferenceInPeriod;

                result.Add(new ProfitLossBasis()
                {
                    Amount = currentAssetUnitAmount * basePrice + accumulatedTransactionValues,
                    StartTime = startTime,
                    EndTime = startTime + TimeSpan.FromDays(period),
                    Unit = asset.InputCurrency
                });
            }

            return result;
        }

        public async Task<IEnumerable<ProfitLossBasis>> VisitBankSaving(BankSavingAsset asset, int period = 1)
        {
            return await VisitInterestAsset(asset, period);
        }

        public async Task<IEnumerable<ProfitLossBasis>> VisitCustomAsset(CustomInterestAsset asset, int period = 1)
        {
            return await VisitInterestAsset(asset, period);
        }

        public Task<IEnumerable<ProfitLossBasis>> VisitStock(Stock asset, int period = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProfitLossBasis>> VisitRealEstate(RealEstateAsset asset, int period = 1)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var result = new List<ProfitLossBasis>();
            decimal currentAssetUnitAmount = 0;
            decimal accumulatedTransactionValues = 0;
            for (var time = asset.InputDay; time < DateTime.Now; time += TimeSpan.FromDays(period))
            {
                var isCurrentTimePeriod = false;
                var startTime = time;
                if (startTime + TimeSpan.FromDays(period) >= DateTime.Now) isCurrentTimePeriod = true;
                decimal basePrice = 1;

                var subListTransactions = listTransaction
                    .Where(t => t.CreatedAt >= startTime && t.CreatedAt < startTime + TimeSpan.FromDays(period));
                var subListTransactionsArr =
                    subListTransactions as SingleAssetTransaction[] ?? subListTransactions.ToArray();
                var sellAndBuyDifferenceInPeriod = await
                    _assetTransactionService.CalculateSubTransactionProfitLoss(subListTransactionsArr,
                        asset.GetCurrency());

                var lastPeriodTransaction = subListTransactionsArr.LastOrDefault();
                if (lastPeriodTransaction is not null)
                    currentAssetUnitAmount =
                        DecideHistoryAssetSpecificAmountHelper(lastPeriodTransaction) ?? 0;
                accumulatedTransactionValues += sellAndBuyDifferenceInPeriod;

                result.Add(new ProfitLossBasis()
                {
                    Amount = currentAssetUnitAmount * basePrice + accumulatedTransactionValues,
                    StartTime = startTime,
                    EndTime = startTime + TimeSpan.FromDays(period),
                    Unit = asset.InputCurrency
                });
            }

            return result;
        }
    }
}