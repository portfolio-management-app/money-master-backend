using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.PortfolioAggregate;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate
{
    public class ReportService : IReportService
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ICashService _cashService;
        private readonly ICryptoService _cryptoService;
        private readonly IRealEstateService _realEstateService;
        private readonly IInterestAssetService _interestAssetService;
        private readonly IStockService _stockService;

        public ReportService(IPortfolioService portfolioService, ICryptoService cryptoService, ICashService cashService,
            IRealEstateService realEstateService, IInterestAssetService interestAssetService,
            IStockService stockService)
        {
            _portfolioService = portfolioService;
            _cryptoService = cryptoService;
            _cashService = cashService;
            _realEstateService = realEstateService;
            _interestAssetService = interestAssetService;
            _stockService = stockService;
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
                await _interestAssetService.CalculateSumBankSavingByPortfolio(portfolioId,
                    foundPortfolio.InitialCurrency);
            // get all stock
            var sumStock = await _stockService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency);
            // get all custom asset 
            var sumCustomAsset =
                await _interestAssetService.CalculateSumCustomInterestAssetByPortfolio(portfolioId,
                    foundPortfolio.InitialCurrency);
            // get all crypto 
            //decimal sumCrypto =
            //   await _cryptoService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency); 
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
                    SumValue = 0
                }
            };
        }
    }
}