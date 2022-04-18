using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.PortfolioAggregate;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate
{
    public class ReportService: IReportService
    {
        private readonly IBaseRepository<CashAsset> _cashRepository;
        private readonly IPortfolioService _portfolioService;
        private readonly ICashService _cashService;
        private readonly ICryptoService _cryptoService;
        private readonly IRealEstateService _realEstateService;
        private readonly IInterestAssetService _interestAssetService;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;

        public ReportService(IPortfolioService portfolioService, ICryptoService cryptoService, ICashService cashService, IRealEstateService realEstateService, IInterestAssetService interestAssetService, ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository, IBaseRepository<CashAsset> cashRepository)
        {
            _portfolioService = portfolioService;
            _cryptoService = cryptoService;
            _cashService = cashService;
            _realEstateService = realEstateService;
            _interestAssetService = interestAssetService;
            _currencyRateRepository = currencyRateRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _cashRepository = cashRepository;
        }

        public async Task<List<PieChartElementModel>> GetPieChart(int portfolioId)
        {
            var foundPortfolio = _portfolioService.GetPortfolioById(portfolioId);
            
            var result = new List<PieChartElementModel>();

            decimal sumCash = await _cashService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency); 
            // get all cash

            // get all real estate

            // get all bank asset

            // get all custom asset 

            // get all 
            return new List<PieChartElementModel>()
            {
                new()
                {
                    AssetType = "cash",
                    SumValue = sumCash
                }
            };
        }
    }
}