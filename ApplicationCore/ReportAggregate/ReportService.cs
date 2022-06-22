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
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.PortfolioAggregate;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;
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
        private readonly CalculateProfitLossVisitor _calculateProfitLossVisitor;

        private string _outsideOut = "OutsideOut";
        private string _outsideIn = "OutsideIn";

        public ReportService(IPortfolioService portfolioService, ICryptoService cryptoService, ICashService cashService,
            IRealEstateService realEstateService, ICustomAssetService customAssetService,
            IStockService stockService, IBankSavingService bankSavingService,
            IAssetTransactionService assetTransactionService, ExternalPriceFacade priceFacade,
            CalculateProfitLossVisitor calculateProfitLossVisitor)
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
            _calculateProfitLossVisitor = calculateProfitLossVisitor;
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


        public async Task<List<SankeyFlowBasis>> GetSankeyChart(int portfolioId, DateTime? startTime = null, DateTime? endTime = null)
        {
            var foundPortfolio = _portfolioService.GetPortfolioById(portfolioId);
            if (foundPortfolio is null)
                throw new InvalidOperationException("Portfolio not found");
            var currency = foundPortfolio.InitialCurrency;
            var type1SankeyBasis = await GetType1SankeyBasis(currency, portfolioId, startTime, endTime);
            var type2SankeyBasis = await GetType2SankeyBasis(currency, portfolioId, startTime, endTime);
            var type3SankeyBasis = await GetType3SankeyBasis(currency, portfolioId, startTime, endTime);
            var type4SankeyBasis = await GetType4SankeyBasis(currency, portfolioId, startTime, endTime);
            var type5SankeyBasis = await GetType5SankeyBasis(currency, portfolioId, startTime, endTime);

            var resultList = new List<SankeyFlowBasis>();
            resultList.AddRange(type1SankeyBasis);
            resultList.AddRange(type2SankeyBasis);
            resultList.AddRange(type3SankeyBasis);
            resultList.AddRange(type4SankeyBasis);
            resultList.AddRange(type5SankeyBasis);
            return resultList;
        }


        // The below code implements the sankey type elements, documented in the sankey formation documentation 


        /// <summary>
        /// Type 1 sankey: outside to assets
        /// </summary>
        /// <param name="inputCurrency"></param>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SankeyFlowBasis>> GetType1SankeyBasis(string inputCurrency, int portfolioId, DateTime? startTime, DateTime? endTime)
        {
            var relatedToFromOutsideTransactions =
                _assetTransactionService.GetTransactionsByType(portfolioId,startTime,endTime, SingleAssetTransactionType.AddValue,
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
        private async Task<IEnumerable<SankeyFlowBasis>> GetType2SankeyBasis(string inputCurrency, int portfolioId, DateTime? startTime, DateTime? endTime)
        {
            var listTransactions = _assetTransactionService.GetTransactionsByType
            (portfolioId, startTime, endTime, SingleAssetTransactionType.WithdrawToCash,
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
            var withdrawToOutsideTransactions = assetTransactions.Where(t =>
                t.SingleAssetTransactionType == SingleAssetTransactionType.WithdrawToOutside);
            var addList = withdrawToOutsideTransactions
                .Select(t => t.CalculateValueInCurrency(inputCurrency, _priceFacade)).ToList();

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
        private async Task<IEnumerable<SankeyFlowBasis>> GetType3SankeyBasis(string inputCurrency, int portfolioId, DateTime? startTime, DateTime? endTime)
        {
            var listTransactions = _assetTransactionService
                .GetTransactionsByType(portfolioId,startTime,endTime,SingleAssetTransactionType.BuyFromFund,
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

        private async Task<IEnumerable<SankeyFlowBasis>> GetType4SankeyBasis(string inputCurrency, int portfolioId, DateTime? startTime, DateTime? endTime)
        {
            var listTransactions = _assetTransactionService.GetTransactionsByType(portfolioId, 
                startTime,endTime,
                SingleAssetTransactionType.MoveToFund,
                SingleAssetTransactionType.BuyFromFund,
                SingleAssetTransactionType.AddValue);
            var eligibleTransactions = listTransactions
                .Where(t => !(t.SingleAssetTransactionType == SingleAssetTransactionType.AddValue &&
                              t.ReferentialAssetType != "fund"));
            var sankeyBasis = eligibleTransactions.GroupBy(transaction => new
            {
                transaction.ReferentialAssetId, transaction.ReferentialAssetType, transaction.ReferentialAssetName,
                transaction.DestinationAssetId, transaction.DestinationAssetType, transaction.DestinationAssetName
            }).Select(async g => new SankeyFlowBasis()
            {
                TargetId = g.Key.DestinationAssetId,
                TargetName = g.Key.DestinationAssetName,
                TargetType = g.Key.DestinationAssetType,
                SourceId = g.Key.ReferentialAssetId,
                SourceName = g.Key.ReferentialAssetName,
                SourceType = g.Key.ReferentialAssetType,
                Amount = await GetType4SankeyBasisHelper(inputCurrency, g)
            });

            return await Task.WhenAll(sankeyBasis);
        }

        private async Task<decimal> GetType4SankeyBasisHelper(string inputCurrency,
            IEnumerable<SingleAssetTransaction> eligibleTransactions)
        {
            var assetTransactions = eligibleTransactions as SingleAssetTransaction[] ?? eligibleTransactions.ToArray();
            var listTasks = assetTransactions
                .Select(t => t.CalculateValueInCurrency(inputCurrency, _priceFacade));
            var calculationSegments = await Task.WhenAll(listTasks);


            var result = assetTransactions.Select((t, i) => t.SingleAssetTransactionType switch
                {
                    SingleAssetTransactionType.MoveToFund => calculationSegments[i],
                    SingleAssetTransactionType.BuyFromFund => -calculationSegments[i],
                    SingleAssetTransactionType.AddValue => -calculationSegments[i],
                    _ => throw new InvalidOperationException("Invalid transaction while calculating type 4 sankey")
                })
                .Sum();

            if (result < 0)
                return -result;
            return result;
        }

        private async Task<IEnumerable<SankeyFlowBasis>> GetType5SankeyBasis(string inputCurrency, int portfolioId, DateTime? startTime, DateTime? endTime)
        {
            var listTransactions = _assetTransactionService.GetTransactionsByType(portfolioId,
                startTime,endTime,
                SingleAssetTransactionType.AddValue,
                SingleAssetTransactionType.BuyFromCash,
                SingleAssetTransactionType.WithdrawToCash);

            var eligibleTransaction = listTransactions.Where(
                t => !(t.SingleAssetTransactionType == SingleAssetTransactionType.AddValue &&
                       t.ReferentialAssetType != "cash"));

            var sankeyBasis = eligibleTransaction.GroupBy(transaction => new
            {
                transaction.ReferentialAssetId, transaction.ReferentialAssetType, transaction.ReferentialAssetName,
                transaction.DestinationAssetId, transaction.DestinationAssetType, transaction.DestinationAssetName
            }).Select(async g => new SankeyFlowBasis()
            {
                SourceId = g.Key.ReferentialAssetId,
                SourceName = g.Key.ReferentialAssetName,
                SourceType = g.Key.ReferentialAssetType,
                TargetId = g.Key.DestinationAssetId,
                TargetName = g.Key.DestinationAssetName,
                TargetType = g.Key.DestinationAssetType,
                Amount = await GetType5SankeyBasisHelper(inputCurrency, g)
            });
            return await Task.WhenAll(sankeyBasis);
        }

        private async Task<decimal> GetType5SankeyBasisHelper(string inputCurrency,
            IEnumerable<SingleAssetTransaction> eligibleTransactions)
        {
            var assetTransactions = eligibleTransactions as SingleAssetTransaction[] ?? eligibleTransactions.ToArray();
            var listTasks = assetTransactions
                .Select(t => t.CalculateValueInCurrency(inputCurrency, _priceFacade));
            var calculationSegments = await Task.WhenAll(listTasks);
            var result = assetTransactions.Select((t, i) => t.SingleAssetTransactionType switch
                {
                    SingleAssetTransactionType.AddValue => calculationSegments[i],
                    SingleAssetTransactionType.BuyFromCash => calculationSegments[i],
                    SingleAssetTransactionType.WithdrawToCash => -calculationSegments[i],
                    _ => throw new InvalidOperationException("Invalid transaction type in type 5 sankey")
                })
                .Sum();

            if (result < 0) return -result;
            return result;
        }

        public async Task<List<ProfitLossBasis>> GetPeriodProfitLossByAsset(int assetId, string assetType,
            string period = "day")
        {
            var asset = GetAssetByIdAndType(assetType, assetId);
            var periodLength = period switch
            {
                "day" => 1,
                "week" => 7,
                "month" => 30,
                _ => throw new InvalidOperationException("Invalid period")
            };
            var result = await asset.AcceptVisitor(_calculateProfitLossVisitor,periodLength);
            return result.ToList();
        }

        private PersonalAsset GetAssetByIdAndType(string type, int id)
        {
            return type switch
            {
                "bankSaving" => _bankSavingService.GetById(id),
                "custom" => _customAssetService.GetById(id),
                "crypto" => _cryptoService.GetById(id),
                "stock" => _stockService.GetById(id),
                "realEstate" => _realEstateService.GetById(id),
                "cash" => _cashService.GetById(id),
                _ => throw new ArgumentException()
            };
        }
    }
}