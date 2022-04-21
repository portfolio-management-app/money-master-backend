using System.Collections.Generic;
using System.Linq;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;

namespace ApplicationCore.PortfolioAggregate
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IBaseRepository<Portfolio> _portfolioRepository;
        private readonly ICashService _cashService;
        private readonly ICryptoService _cryptoService;
        private readonly IInterestAssetService _interestAssetService;
        private readonly IStockService _stockService;
        private readonly IRealEstateService _realEstateService;

        public PortfolioService(IBaseRepository<Portfolio> portfolioRepository, ICashService cashService,
            ICryptoService cryptoService, IInterestAssetService interestAssetService, IStockService stockService,
            IRealEstateService realEstateService)
        {
            _portfolioRepository = portfolioRepository;
            _cashService = cashService;
            _cryptoService = cryptoService;
            _interestAssetService = interestAssetService;
            _stockService = stockService;
            _realEstateService = realEstateService;
        }

        public Portfolio CreatePortfolio(int userId, string name, decimal initialCash, string initialCurrency)
        {
            var newPortfolio = new Portfolio(userId, name, initialCash, initialCurrency);
            _portfolioRepository.Insert(newPortfolio);
            return newPortfolio;
        }

        public Portfolio GetPortfolioById(int portfolioId)
        {
            var foundPortfolio = _portfolioRepository.GetFirst(p => p.Id == portfolioId);
            return foundPortfolio;
        }

        public List<Portfolio> GetPortfolioList(int userId)
        {
            var listPortfolio = _portfolioRepository.List(p => p.UserId == userId).ToList();
            return listPortfolio;
        }

        public PersonalAsset GetAssetByPortfolioAndAssetId(int portfolioId, string assetType, int assetId)
        {
            PersonalAsset foundAsset = assetType switch
            {
                "cash" => _cashService.GetById(assetId),
                "crypto" => _cryptoService.GetById(assetId),
                "realEstate" => _realEstateService.GetById(assetId),
                "stock" => _stockService.GetById(assetId),
                "bankSaving" => _interestAssetService.GetBankSavingAssetById(assetId),
                "custom" => _interestAssetService.GetCustomAssetById(assetId),
                _ => null
            };

            return foundAsset;
        }
    }
}