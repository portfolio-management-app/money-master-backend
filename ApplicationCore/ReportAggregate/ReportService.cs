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

        public ReportService(IPortfolioService portfolioService, ICryptoService cryptoService, ICashService cashService,
            IRealEstateService realEstateService, ICustomAssetService customAssetService,
            IStockService stockService, IBankSavingService bankSavingService,
            IAssetTransactionService assetTransactionService)
        {
            _portfolioService = portfolioService;
            _cryptoService = cryptoService;
            _cashService = cashService;
            _realEstateService = realEstateService;
            _customAssetService = customAssetService;
            _stockService = stockService;
            _bankSavingService = bankSavingService;
            _assetTransactionService = assetTransactionService;
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
            // Get Type 1:
            // Get related transaction: 
            var relatedToFromOutsideTransactions =
                _assetTransactionService.GetTransactionsByType(SingleAssetTransactionTypes.AddValue,
                    SingleAssetTransactionTypes.BuyFromOutside);
            var removedBuyNotFromOutsideTransactions = relatedToFromOutsideTransactions
                .Where(transaction =>
                    !(transaction.SingleAssetTransactionTypes == SingleAssetTransactionTypes.AddValue
                      && transaction.ReferentialAssetType is null));
            var sankeyFlowBasis = relatedToFromOutsideTransactions
                .GroupBy(transaction => new
                {
                    transaction.DestinationAssetId, transaction.DestinationAssetName,
                    transaction.DestinationAssetType
                })
                .Select(group => new SankeyFlowBasis()
                {
                    SourceType = "outsideIn",
                    SourceName = "outsideIn",
                    SourceId = null,
                    TargetName = group.Key.DestinationAssetName,
                    TargetType = group.Key.DestinationAssetType,
                    TargetId = group.Key.DestinationAssetId,
                    Amount = group.Sum(g => g.Amount),
                    Currency = "USD"
                });

            return sankeyFlowBasis.ToList();
        }
    }
}