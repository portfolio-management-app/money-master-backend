using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;
using Mapster;
using Transaction = ApplicationCore.Entity.Transactions.Transaction;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public class RealEstateService : IRealEstateService
    {
        private readonly IBaseRepository<RealEstateAsset> _realEstateRepository;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private TransactionFactory TransactionFactory { get; set; }

        public RealEstateService(IBaseRepository<RealEstateAsset> realEstateRepository, IBaseRepository<Transaction> transactionRepository, TransactionFactory transactionFactory)
        {
            _realEstateRepository = realEstateRepository;
            _transactionRepository = transactionRepository;
            TransactionFactory = transactionFactory;
        }

        public RealEstateAsset CreateNewRealEstateAsset(int portfolioId, RealEstateDto dto)
        {
            var newRealEstate = dto.Adapt<RealEstateAsset>();
            newRealEstate.PortfolioId = portfolioId;
            _realEstateRepository.Insert(newRealEstate);
            
            // create a transaction
         
            var newTransaction =  TransactionFactory
                .CreateNewTransaction
                (TransactionType.NewAsset,
                    "Create new real estate",
                    "None",
                    newRealEstate.Id,
                    "realEstate",
                    newRealEstate.Id,
                    newRealEstate.InputMoneyAmount,
                    newRealEstate.InputCurrency,
                    100
                );
            _transactionRepository.Insert(newTransaction); 
            return newRealEstate;
        }

        public List<RealEstateAsset> GetAllRealEstateAssetByPortfolio(int portfolioId)
        {
            return _realEstateRepository.List(realEstate => realEstate.PortfolioId == portfolioId).ToList();
        }

        public RealEstateAsset UpdateRealEstateAsset(int portfolioId, int realEstateId, RealEstateDto dto)
        {
            var foundRealEstate =
                _realEstateRepository.GetFirst(r => r.Id == realEstateId && r.PortfolioId == portfolioId);
            if (foundRealEstate is null)
                return null;
            dto.Adapt(foundRealEstate);
            foundRealEstate.LastChanged = DateTime.Now;
            _realEstateRepository.Update(foundRealEstate);

            return foundRealEstate;
        }
    }
}