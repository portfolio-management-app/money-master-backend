using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate.DTOs;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using Mapster;

namespace ApplicationCore.AssetAggregate.BankSavingAssetAggregate
{
    public class BankSavingService : IBankSavingService
    {
        private readonly ExternalPriceFacade _priceFacade;
        private readonly IBaseRepository<BankSavingAsset> _bankSavingRepository;
        private readonly IInvestFundService _investFundService;
        private readonly ICashService _cashService;

        public BankSavingService(IBaseRepository<BankSavingAsset> bankSavingRepository,
            IInvestFundService investFundService, ExternalPriceFacade priceFacade, ICashService cashService)
        {
            _bankSavingRepository = bankSavingRepository;
            _investFundService = investFundService;
            _priceFacade = priceFacade;
            _cashService = cashService;
        }


        public async Task<BankSavingAsset> AddBankSavingAsset(int portfolioId, CreateNewBankSavingAssetDto dto)
        {
            if (dto.IsUsingCash && dto.UsingCashId is not null && !dto.IsUsingInvestFund)
            {
                var cashId = dto.UsingCashId;

                var foundCash = _cashService.GetById(cashId.Value);
                if (foundCash is null)
                    throw new InvalidOperationException("Cash not found");
                var withdrawResult = await foundCash.Withdraw(dto.InputMoneyAmount, dto.InputCurrency, _priceFacade);

                if (!withdrawResult)
                    throw new InvalidOperationException("The specified cash does not have sufficient amount");
            }

            var newAsset = dto.Adapt<BankSavingAsset>();
            newAsset.PortfolioId = portfolioId;

            _bankSavingRepository.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _bankSavingRepository.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }


        public BankSavingAsset EditBankSavingAsset(int portfolioId, int bankingAssetId, EditBankSavingAssetDto dto)
        {
            var foundBankingAsset =
                _bankSavingRepository
                    .GetFirst(b => b.Id == bankingAssetId
                                   && b.PortfolioId == portfolioId);
            if (foundBankingAsset is null)
                return null;
            foundBankingAsset
                .Update(dto.Name,
                    dto.InputDay,
                    dto.Description,
                    dto.InputMoneyAmount,
                    dto.InputCurrency,
                    dto.InterestRate,
                    dto.TermRange,
                    dto.BankCode,
                    dto.IsGoingToReinState);

            // TODO: deal with change interest rate (continue with the current amount or reset from start)

            _bankSavingRepository.Update(foundBankingAsset);
            return foundBankingAsset;
        }


        public async Task<decimal> CalculateSumBankSavingByPortfolio(int portfolioId, string currencyCode)
        {
            var bankSavingAsset = await ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                bankSavingAsset.Select(bankSaving =>
                    bankSaving.CalculateValueInCurrency(currencyCode, _priceFacade));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }

        public BankSavingAsset GetById(int assetId)
        {
            return _bankSavingRepository.GetFirst(b => b.Id == assetId);
        }

        public async Task<List<BankSavingAsset>> ListByPortfolio(int portfolioId)
        {
            var listBankSavingAsset = _bankSavingRepository
                .List(b => b.PortfolioId == portfolioId)
                .ToList();
            return listBankSavingAsset;
        }

        public BankSavingAsset SetAssetToDelete(int assetId)
        {
            var found = _bankSavingRepository.GetFirst(b => b.Id == assetId);
            if (found is null)
                return null;
            return _bankSavingRepository.SetToDeleted(found);
        }
    }
}