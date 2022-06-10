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
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using ApplicationCore.TransactionAggregate.DTOs;

namespace ApplicationCore.TransactionAggregate
{
    public class AssetTransactionService : IAssetTransactionService
    {
        private readonly IBaseRepository<SingleAssetTransaction> _transactionRepository;
        private readonly IBaseRepository<CashAsset> _cashRepository;
        private readonly ExternalPriceFacade _priceFacade;
        private readonly ICashService _cashService;
        private readonly IBankSavingService _bankSavingService;
        private readonly ICustomAssetService _customAssetService;
        private readonly ICryptoService _cryptoService;
        private readonly IStockService _stockService;
        private readonly IRealEstateService _realEstateService;
        private readonly IInvestFundService _investFundService;


        public AssetTransactionService(IBaseRepository<SingleAssetTransaction> transactionRepository,
            ICashService cashService, IBaseRepository<CashAsset> cashRepository, ExternalPriceFacade priceFacade,
            IBankSavingService bankSavingService, ICustomAssetService customAssetService, ICryptoService cryptoService,
            IStockService stockService, IRealEstateService realEstateService, IInvestFundService investFundService)
        {
            _transactionRepository = transactionRepository;
            _cashService = cashService;
            _cashRepository = cashRepository;
            _priceFacade = priceFacade;
            _bankSavingService = bankSavingService;
            _customAssetService = customAssetService;
            _cryptoService = cryptoService;
            _stockService = stockService;
            _realEstateService = realEstateService;
            _investFundService = investFundService;
        }

        public SingleAssetTransaction AddCreateNewAssetTransaction
        (PersonalAsset asset,
            decimal moneyAmount,
            string currency,
            bool isUsingInvestFund,
            bool isUsingCash,
            int? usingCashId,
            decimal? fee,
            decimal? tax)
        {
            var singleAssetTransactionType = SingleAssetTransactionType.BuyFromOutside;
            int? resultReferentialAssetId = null;
            string resultReferentialAssetType = null;
            string resultReferentialAssetName = null;
            if (isUsingInvestFund)
            {
                singleAssetTransactionType = SingleAssetTransactionType.BuyFromFund;
                resultReferentialAssetId = -1;
                resultReferentialAssetType = "fund";
                resultReferentialAssetName = "fund";
            }

            if (isUsingCash && usingCashId is not null)
            {
                var foundCash = _cashService.GetById(usingCashId.Value);
                singleAssetTransactionType = SingleAssetTransactionType.BuyFromCash;
                resultReferentialAssetId = usingCashId.Value;
                resultReferentialAssetType = foundCash.GetAssetType();
                resultReferentialAssetName = foundCash.Name;
            }

            var newAssetTransaction = new SingleAssetTransaction()
            {
                SingleAssetTransactionType = singleAssetTransactionType,
                ReferentialAssetId = resultReferentialAssetId,
                ReferentialAssetType = resultReferentialAssetType,
                ReferentialAssetName = resultReferentialAssetName,
                DestinationAssetId = asset.Id,
                DestinationAssetName = asset.Name,
                DestinationAssetType = asset.GetAssetType(),
                DestinationAmount = moneyAmount,
                DestinationCurrency = currency,
                Amount = moneyAmount,
                CreatedAt = DateTime.Now,
                CurrencyCode = currency,
                LastChanged = DateTime.Now,
                Fee = fee,
                Tax = tax
            };

            _transactionRepository.Insert(newAssetTransaction);
            return newAssetTransaction;
        }

        public List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset)
        {
            var listTransaction = _transactionRepository.List(
                trans =>
                    trans.ReferentialAssetId == asset.Id && trans.ReferentialAssetType == asset.GetAssetType()
                    || trans.DestinationAssetId == asset.Id && trans.DestinationAssetType == asset.GetAssetType());
            return listTransaction.ToList();
        }

        public async Task<SingleAssetTransaction> CreateAddValueTransaction(int requestPortfolioId,
            CreateTransactionDto createTransactionDto)
        {
            const SingleAssetTransactionType singleAssetTransactionType = SingleAssetTransactionType.AddValue;
            var sourceAssetId = createTransactionDto.ReferentialAssetId;
            var targetAssetId = createTransactionDto.DestinationAssetId;
            PersonalAsset sourceAsset = null;
            PersonalAsset targetAsset = null;
            if (targetAssetId != null)
            {
                targetAsset = GetAssetByIdAndType(createTransactionDto.DestinationAssetType, targetAssetId.Value);
                if (sourceAssetId != null)
                {
                    sourceAsset = GetAssetByIdAndType(createTransactionDto.ReferentialAssetType, sourceAssetId.Value);
                    if (!await sourceAsset.Withdraw
                            (createTransactionDto.Amount, createTransactionDto.CurrencyCode, _priceFacade))
                        throw new InvalidOperationException("Insufficient amount in source asset");
                }
                else if (createTransactionDto.IsUsingFundAsSource)
                {
                     await _investFundService.WithdrawFromInvestFund(requestPortfolioId, createTransactionDto.Amount,
                        createTransactionDto.CurrencyCode);
                }


                if (createTransactionDto.AmountInDestinationAssetUnit != null)
                    _ = await targetAsset.AddValue(
                        createTransactionDto.AmountInDestinationAssetUnit.Value);
            }

            var newTransaction = new SingleAssetTransaction()
            {
                SingleAssetTransactionType = singleAssetTransactionType,
                ReferentialAssetId = createTransactionDto.ReferentialAssetId,
                ReferentialAssetType = createTransactionDto.ReferentialAssetType,
                ReferentialAssetName = sourceAsset?.Name,
                DestinationAssetId = targetAssetId,
                DestinationAssetName = targetAsset?.Name,
                DestinationAssetType = createTransactionDto.DestinationAssetType,
                DestinationAmount = createTransactionDto.Amount,
                DestinationCurrency = createTransactionDto.CurrencyCode,
                AmountInDestinationAssetUnit = createTransactionDto.AmountInDestinationAssetUnit,
                Amount = createTransactionDto.Amount,
                CreatedAt = DateTime.Now,
                CurrencyCode = createTransactionDto.CurrencyCode,
                LastChanged = DateTime.Now,
                Fee = createTransactionDto.Fee,
                Tax = createTransactionDto.Tax
            };

            _transactionRepository.Insert(newTransaction);
            return newTransaction;
        }



        public async Task<SingleAssetTransaction> CreateWithdrawToOutsideTransaction(
            CreateTransactionDto createTransactionDto)
        {
            if (createTransactionDto.ReferentialAssetId is null) throw new InvalidOperationException("Asset not found");
            var sourceAssetId = createTransactionDto.ReferentialAssetId.Value;
            var foundAsset = GetAssetByIdAndType(createTransactionDto.ReferentialAssetType,
                sourceAssetId);
            if (foundAsset is null)
                throw new InvalidOperationException("Asset not found");
            var withdrawResult = await foundAsset.Withdraw(createTransactionDto.Amount, createTransactionDto.CurrencyCode, _priceFacade);
            if (!withdrawResult) throw new InvalidOperationException("Insufficient value to withdraw");
            var newTransaction = new SingleAssetTransaction()
            {
                ReferentialAssetId = sourceAssetId,
                ReferentialAssetType = createTransactionDto.ReferentialAssetType,
                ReferentialAssetName = foundAsset.Name,
                Amount = createTransactionDto.Amount,
                CurrencyCode = createTransactionDto.CurrencyCode,
                SingleAssetTransactionType = SingleAssetTransactionType.WithdrawToOutside,
                Fee = createTransactionDto.Fee,
                Tax = createTransactionDto.Tax,
                CreatedAt = DateTime.Now,
                LastChanged = DateTime.Now,
                DestinationAssetId = null,
                DestinationAssetName = null,
                DestinationAssetType = null,
                DestinationAmount = createTransactionDto.Amount,
                DestinationCurrency = createTransactionDto.CurrencyCode,
                AmountInDestinationAssetUnit = createTransactionDto.Amount
            };

            _transactionRepository.Insert(newTransaction);
            return newTransaction;
        }

        public decimal CalculateSubTransactionProfitLoss
            (IEnumerable<SingleAssetTransaction> singleAssetTransactions, string currencyCode)
        {
            return singleAssetTransactions.Sum(transaction => transaction.SingleAssetTransactionType switch
            {
                SingleAssetTransactionType.MoveToFund => transaction.Amount,
                SingleAssetTransactionType.WithdrawToCash => transaction.Amount,
                SingleAssetTransactionType.AddValue => -transaction.Amount,
                SingleAssetTransactionType.BuyFromFund => 0,
                _ => 0
            });
        }

        public List<SingleAssetTransaction> GetTransactionsByType(
            params SingleAssetTransactionType[] assetTransactionTypesArray)
        {
            var resultTransactions = _transactionRepository.List(transaction =>
                !transaction.IsDeleted &&
                assetTransactionTypesArray.Contains(transaction.SingleAssetTransactionType));

            return resultTransactions.ToList();
        }

        public async Task<SingleAssetTransaction> CreateWithdrawToCashTransaction(
            CreateTransactionDto createTransactionDto)
        {
            var foundCash = _cashRepository.GetFirst(c => c.Id == createTransactionDto.DestinationAssetId);
            if (foundCash is null)
                throw new InvalidOperationException(
                    $"Cash with Id {createTransactionDto.DestinationAssetId} not found");

            decimal valueToAddToCash = 0;
            var sourceAssetId = createTransactionDto.ReferentialAssetId;
            PersonalAsset sourceAsset;
            if (sourceAssetId is not null)
            {
                sourceAsset = GetAssetByIdAndType(createTransactionDto.ReferentialAssetType, sourceAssetId.Value);
                if (createTransactionDto.IsTransferringAll)
                {
                    var valueInDestinationCurrency = await sourceAsset.CalculateValueInCurrency(foundCash.CurrencyCode,
                        _priceFacade);
                    await sourceAsset.WithdrawAll();
                    foundCash.Amount += valueInDestinationCurrency;
                }
                else
                {
                    var mandatoryWithdrawAll = new[] { "bankSaving", "realEstate" };
                    if (mandatoryWithdrawAll.Contains(sourceAsset.GetAssetType()))
                        throw new InvalidOperationException(
                            $" Not allowed for partial withdraw: {sourceAsset.GetAssetType()}");

                    if (!await sourceAsset.Withdraw(createTransactionDto.Amount, createTransactionDto.CurrencyCode,
                            _priceFacade))
                        throw new OperationCanceledException("Insufficient value");

                    if (foundCash.CurrencyCode == createTransactionDto.CurrencyCode)
                    {
                        valueToAddToCash = createTransactionDto.Amount;
                        foundCash.Amount += valueToAddToCash;
                    }
                    else
                    {
                        var rateObj =
                            await _priceFacade.CurrencyRateRepository.GetRateObject(createTransactionDto.CurrencyCode);
                        valueToAddToCash = rateObj.GetValue(foundCash.CurrencyCode) * createTransactionDto.Amount;
                        foundCash.Amount += valueToAddToCash;
                    }
                }

                _cashRepository.Update(foundCash);
            }
            else
            {
                throw new InvalidOperationException("Source asset is not specified");
            }

            if (createTransactionDto.ReferentialAssetId == null) throw new InvalidOperationException();
            var newTransaction = new SingleAssetTransaction
            {
                ReferentialAssetId = sourceAssetId.Value,
                ReferentialAssetType = createTransactionDto.ReferentialAssetType,
                ReferentialAssetName = sourceAsset.Name,
                Amount = createTransactionDto.Amount,
                CurrencyCode = createTransactionDto.CurrencyCode,
                SingleAssetTransactionType = SingleAssetTransactionType.WithdrawToCash,
                Fee = createTransactionDto.Fee,
                Tax = createTransactionDto.Tax,
                CreatedAt = DateTime.Now,
                LastChanged = DateTime.Now,
                DestinationAssetId = foundCash.Id,
                DestinationAssetName = foundCash.Name,
                DestinationAssetType = foundCash.GetAssetType(),
                DestinationAmount = valueToAddToCash,
                DestinationCurrency = foundCash.CurrencyCode,
                AmountInDestinationAssetUnit = valueToAddToCash
            };
            _transactionRepository.Insert(newTransaction);

            return newTransaction;

        }
        
        public async Task<SingleAssetTransaction> CreateMoveToFundTransaction(
            int portfolioId, PersonalAsset asset, decimal amount,
            string currencyCode, bool isTransferringAll)
        {
            var newTransaction = new SingleAssetTransaction
            {
                ReferentialAssetId = asset.Id,
                ReferentialAssetType = asset.GetAssetType(),
                ReferentialAssetName = asset.Name,
                Amount = amount,
                CurrencyCode = currencyCode,
                SingleAssetTransactionType = SingleAssetTransactionType.MoveToFund,
                Fee = 0,
                Tax = 0,
                CreatedAt = DateTime.Now,
                LastChanged = DateTime.Now,
                DestinationAssetId = null,
                DestinationAssetName = null,
                DestinationAssetType = null,
                DestinationAmount = 0,
                DestinationCurrency = currencyCode,
            };
            _transactionRepository.Insert(newTransaction);
            return newTransaction;
        } 

        public async Task<SingleAssetTransaction> Fake()
        {
            return new SingleAssetTransaction();
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