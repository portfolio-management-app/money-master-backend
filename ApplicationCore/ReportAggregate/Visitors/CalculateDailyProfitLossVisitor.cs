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

        public override async Task<IEnumerable<ProfitLossBasis>> VisitCrypto(Crypto asset)
        {
            var listTransaction = _assetTransactionService.GetTransactionListByAsset(asset);
            var currentAssetValue = await asset.CalculateValueInCurrency(asset.CurrencyCode, _priceFacade);
            var result = new List<ProfitLossBasis>();
            for (var time = DateTime.Now; time > asset.InputDay; time -= TimeSpan.FromDays(1))
            {
                var pastPrice = await _priceFacade.CryptoRateRepository.GetPastPriceInCurrency(asset.CryptoCoinCode,
                    asset.CurrencyCode, time);
                var endTime = time;
                var subListTransactions = listTransaction
                    .Where(t => t.CreatedAt < endTime && t.CreatedAt >= endTime - TimeSpan.FromDays(1));
                var subListTransactionsArr = subListTransactions as SingleAssetTransaction[] ?? subListTransactions.ToArray();
                var sellAndBuyDifference  = await 
                    _assetTransactionService.CalculateSubTransactionProfitLoss(subListTransactionsArr, asset.CurrencyCode);
                var firstTransactionInPeriod = subListTransactionsArr.FirstOrDefault();
                if (firstTransactionInPeriod is null)
                {
                   result.Add(new ProfitLossBasis()
                   {
                       Amount = 0,
                       Unit = asset.CurrencyCode,
                       StartTime = endTime - TimeSpan.FromDays(1),
                       EndTime = endTime
                   });
                   continue;
                }
                var amountAtTheStartOfPeriod =
                    firstTransactionInPeriod.AmountOfReferentialAssetBeforeCreatingTransaction;

                var periodProfitLoss = pastPrice * amountAtTheStartOfPeriod + sellAndBuyDifference;
                if (periodProfitLoss != null)
                    result.Add(new ProfitLossBasis()
                    {
                        Amount = periodProfitLoss.Value,
                        Unit =asset.CurrencyCode,
                        StartTime = endTime - TimeSpan.FromDays(1),
                        EndTime = endTime
                        
                    });
            }

            return result;
        }
    }
}