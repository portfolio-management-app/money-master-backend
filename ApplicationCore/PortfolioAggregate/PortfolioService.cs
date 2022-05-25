using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;

namespace ApplicationCore.PortfolioAggregate
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IBaseRepository<Portfolio> _portfolioRepository;
        private readonly ICashService _cashService;
        private readonly ICryptoService _cryptoService;
        private readonly ICustomAssetService _customAssetService;
        private readonly IStockService _stockService;
        private readonly IRealEstateService _realEstateService;
        private readonly IBankSavingService _bankSavingService;
        private readonly IInvestFundService _investFundService;

        public PortfolioService(IBaseRepository<Portfolio> portfolioRepository, ICashService cashService,
            ICryptoService cryptoService, ICustomAssetService customAssetService, IStockService stockService,
            IRealEstateService realEstateService, IBankSavingService bankSavingService, IInvestFundService investFundService)
        {
            _portfolioRepository = portfolioRepository;
            _cashService = cashService;
            _cryptoService = cryptoService;
            _customAssetService = customAssetService;
            _stockService = stockService;
            _realEstateService = realEstateService;
            _bankSavingService = bankSavingService;
            _investFundService = investFundService;
        }

        public Portfolio CreatePortfolio(int userId, string name, decimal initialCash, string initialCurrency)
        {
            var newPortfolio = new Portfolio(userId, name, initialCash, initialCurrency);
            _portfolioRepository.Insert(newPortfolio);
            return newPortfolio;
        }

        public Portfolio GetPortfolioById(int portfolioId)
        {
            var foundPortfolio = _portfolioRepository.GetFirst(p => p.Id == portfolioId && !p.IsDeleted) ;
            return foundPortfolio;
        }

        public List<Portfolio> GetPortfolioList(int userId)
        {
            var listPortfolio = _portfolioRepository
                .List(p => p.UserId == userId && !p.IsDeleted).OrderBy(p => p.Id)
                .ToList();
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
                "bankSaving" => _bankSavingService.GetById(assetId),
                "custom" => _customAssetService.GetById(assetId),
                _ => null
            };

            return foundAsset;
        }

        public async Task<Portfolio> EditPortfolio(int portfolioId, string newName, string newCurrency)
        {
            var foundPortfolio = GetPortfolioById(portfolioId);
            if (foundPortfolio is null)
                return null;
            foundPortfolio.Name = newName;
            foundPortfolio.InitialCurrency = newCurrency;
            foundPortfolio.InitialCash = 0; // have to set to zero for report correctness 
            _portfolioRepository.Update(foundPortfolio);
            await _investFundService.EditCurrency(portfolioId,newCurrency); 
            return foundPortfolio;
        }

        public Portfolio DeletePortfolio(int portfolioId)
        {
            var foundPortfolio = GetPortfolioById(portfolioId);
            if (foundPortfolio.IsDeleted)
                return null;
            foundPortfolio.IsDeleted = true;
            _portfolioRepository.Update(foundPortfolio); 
            return foundPortfolio; 
        }
    }
}