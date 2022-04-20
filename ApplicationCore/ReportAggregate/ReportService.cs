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
            // get all cash
            decimal sumCash = await _cashService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency); 

            // get all real estate
            decimal sumRealEstate =
                await _realEstateService.CalculateSumByPortfolio(portfolioId, foundPortfolio.InitialCurrency); 
            // get all bank asset
            decimal sumBankAsset =
                await _interestAssetService.CalculateSumBankSavingByPortfolio(portfolioId,
                    foundPortfolio.InitialCurrency);
            // get all custom asset 

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
                    AssetType = "BankSavingAsset",
                    SumValue = sumBankAsset
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