using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.PortfolioAggregate;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.TransactionAggregate;

namespace ApplicationCore.ReportAggregate
{
    public class ReportService : IReportService
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ICashService _cashService;
        private readonly ICryptoService _cryptoService;
        private readonly IRealEstateService _realEstateService;
        private readonly ICustomAssetService _customAssetService;
        private readonly IStockService _stockService;
        private readonly IBankSavingService _bankSavingService;
        private readonly IAssetTransactionService _assetTransactionService;
        private readonly ExternalPriceFacade _priceFacade;

        private string _outsideOut = "OutsideOut";
        private string _outsideIn = "OutsideIn";

        public ReportService(IPortfolioService portfolioService, ICryptoService cryptoService, ICashService cashService,
            IRealEstateService realEstateService, ICustomAssetService customAssetService,
            IStockService stockService, IBankSavingService bankSavingService,
            IAssetTransactionService assetTransactionService, ExternalPriceFacade priceFacade)
        {
            _portfolioService = portfolioService;
            _cryptoService = cryptoService;
            _cashService = cashService;
            _realEstateService = realEstateService;
            _customAssetService = customAssetService;
            _stockService = stockService;
            _bankSavingService = bankSavingService;
            _assetTransactionService = assetTransactionService;
            _priceFacade = priceFacade;
        }

        public async Task<List<PieChartElementModel>> GetPieChart(int portfolioId)
        {
            var foundPortfolio = _portfolioService.GetPortfolioById(portfolioId);
            // get all cash
            var sumCash = await _cashService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency);

            // get all real estate
            var sumRealEstate =
                await _realEstateService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency);
            // get all bank asset
            var sumBankAsset =
                await _bankSavingService.CalculateSumBankSavingByPortfolio(portfolioId,
                    foundPortfolio.InitialCurrency);
            // get all stock
            var sumStock = await _stockService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency);
            // get all custom asset 
            var sumCustomAsset =
                await _customAssetService.CalculateSumCustomInterestAssetByPortfolio(portfolioId,
                    foundPortfolio.InitialCurrency);
            // get all crypto 
            var sumCrypto =
                await _cryptoService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency);
            return new List<PieChartElementModel>
            {
                new()
                {
                    AssetType = "Cash",
                    SumValue = sumCash
                },
                new()
                {
                    AssetType = "RealEstate",
                    SumValue = sumRealEstate
                },
                new()
                {
                    AssetType = "Stock",
                    SumValue = sumStock
                },
                new()
                {
                    AssetType = "BankSavingAsset",
                    SumValue = sumBankAsset
                },
                new()
                {
                    AssetType = "CustomAsset",
                    SumValue = sumCustomAsset
                },
                new()
                {
                    AssetType = "Crypto",
                    SumValue = sumCrypto
                }
            };
        }


        public async Task<List<SankeyFlowBasis>> GetSankeyChart(int portfolioId)
        {
            var foundPortfolio = _portfolioService.GetPortfolioById(portfolioId);
            if (foundPortfolio is null)
                throw new InvalidOperationException("Portfolio not found");
            var currency = foundPortfolio.InitialCurrency;
            var type1SankeyBasis = await GetType1SankeyBasis(currency, portfolioId);
            var type2SankeyBasis = await GetType2SankeyBasis(currency, portfolioId);
            var type3SankeyBasis = await GetType3SankeyBasis(currency, portfolioId);
            return new List<SankeyFlowBasis>();
        }
        // The below code implements the sankey type elements, documented in the sankey formation documentation 


        /// <summary>
        /// Type 1 sankey: outside to assets
        /// </summary>
        /// <param name="inputCurrency"></param>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SankeyFlowBasis>> GetType1SankeyBasis(string inputCurrency, int portfolioId)
        {
            var relatedToFromOutsideTransactions =
                _assetTransactionService.GetTransactionsByType(portfolioId, SingleAssetTransactionType.AddValue,
                    SingleAssetTransactionType.BuyFromOutside);
            var removedBuyNotFromOutsideTransactions = relatedToFromOutsideTransactions
                .Where(transaction =>
                    !(transaction.SingleAssetTransactionType == SingleAssetTransactionType.AddValue
                      && transaction.ReferentialAssetType is null));
            var sankeyFlowBasis = removedBuyNotFromOutsideTransactions
                .GroupBy(transaction => new
                {
                    transaction.DestinationAssetId, transaction.DestinationAssetName,
                    transaction.DestinationAssetType
                })
                .Select(async grouping => new SankeyFlowBasis()
                {
                    SourceType = _outsideIn,
                    SourceName = _outsideIn,
                    SourceId = null,
                    TargetName = grouping.Key.DestinationAssetName,
                    TargetType = grouping.Key.DestinationAssetType,
                    TargetId = grouping.Key.DestinationAssetId,
                    Amount = await GetType1SankeyChartCalculationHelper(inputCurrency, grouping),
                    Currency = inputCurrency
                });

            return await Task.WhenAll(sankeyFlowBasis);
        }

        private async Task<decimal> GetType1SankeyChartCalculationHelper(string inputCurrency,
            IEnumerable<SingleAssetTransaction> singleAssetTransactions)
        {
            var taskList = singleAssetTransactions
                .Select(t => t.CalculateValueInCurrency(inputCurrency, _priceFacade));
            var calculationSegments = await Task.WhenAll(taskList);
            return calculationSegments.Sum();
        }

        /// <summary>
        /// Type 2: flow from asset to outside out 
        /// </summary>
        /// <param name="inputCurrency"></param>
        /// <param name="portfolioId">portfolioId</param>
        /// <returns>The partial list of sankey flow basis</returns>
        private async Task<IEnumerable<SankeyFlowBasis>> GetType2SankeyBasis(string inputCurrency, int portfolioId)
        {
            var listTransactions = _assetTransactionService.GetTransactionsByType
            (portfolioId, SingleAssetTransactionType.AddValue,
                SingleAssetTransactionType.WithdrawToOutside,
                SingleAssetTransactionType.BuyFromCash,
                SingleAssetTransactionType.AddValue);

            var eligibleTransactions = listTransactions
                .Where(t => !(t.SingleAssetTransactionType == SingleAssetTransactionType.AddValue
                              && t.ReferentialAssetType != "cash"));

            var sankeyBasis = eligibleTransactions.GroupBy(transaction => new
            {
                transaction.ReferentialAssetId, transaction.ReferentialAssetType, transaction.ReferentialAssetName
            }).Select(async g => new SankeyFlowBasis()
            {
                SourceId = g.Key.ReferentialAssetId!.Value,
                SourceType = g.Key.ReferentialAssetType,
                SourceName = g.Key.ReferentialAssetName,
                TargetId = null,
                TargetName = _outsideOut,
                TargetType = _outsideOut,
                Amount =
                    await GetType2SankeyChartCalculationHelper(inputCurrency, g),
                Currency = inputCurrency
            });

            return await Task.WhenAll(sankeyBasis);
        }


        private async Task<decimal> GetType2SankeyChartCalculationHelper(string inputCurrency,
            IEnumerable<SingleAssetTransaction> singleAssetTransactions)
        {
            var assetTransactions =
                singleAssetTransactions as SingleAssetTransaction[] ?? singleAssetTransactions.ToArray();
            var taskList = assetTransactions
                .Select(t => t.CalculateSumOfTaxAndFee(inputCurrency, _priceFacade)).ToList();
            var addList = assetTransactions.Select(t => t.CalculateValueInCurrency(inputCurrency, _priceFacade))
                .ToList();
            taskList.AddRange(addList);

            var calculatedSegments = await Task.WhenAll(taskList);
            return calculatedSegments.Sum();
        }

        /// <summary>
        /// Get Type 3 sankey flow basis
        /// </summary>
        /// <param name="inputCurrency"></param>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SankeyFlowBasis>> GetType3SankeyBasis(string inputCurrency, int portfolioId)
        {
            var listTransactions = _assetTransactionService
                .GetTransactionsByType(portfolioId, SingleAssetTransactionType.BuyFromFund,
                    SingleAssetTransactionType.AddValue);
            var eligibleTransactions = listTransactions.Where(t =>
                !(t.SingleAssetTransactionType == SingleAssetTransactionType.AddValue &&
                  t.ReferentialAssetType != "fund"));

            var sankeyBasis = eligibleTransactions.GroupBy(transaction => new
            {
                transaction.ReferentialAssetId, transaction.ReferentialAssetType, transaction.ReferentialAssetName
            }).Select(async g => new SankeyFlowBasis()
                {
                    SourceId = null,
                    SourceName = "fund",
                    SourceType = "fund",
                    Amount = await GetType3SankeyBasisHelper(inputCurrency, g),
                    TargetId = null,
                    TargetName = _outsideOut,
                    TargetType = _outsideOut
                }
            );
            return await Task.WhenAll(sankeyBasis);
        }

        private async Task<decimal> GetType3SankeyBasisHelper(string inputCurrency,
            IEnumerable<SingleAssetTransaction> singleAssetTransactions)
        {
            var taskList = singleAssetTransactions
                .Select(t => t.CalculateSumOfTaxAndFee(inputCurrency, _priceFacade));
            var calculatedSegments = await Task.WhenAll(taskList);
            return calculatedSegments.Sum();
        }
    }
}