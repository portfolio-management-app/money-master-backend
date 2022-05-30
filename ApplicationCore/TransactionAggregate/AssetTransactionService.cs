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


        public AssetTransactionService(IBaseRepository<SingleAssetTransaction> transactionRepository,
            ICashService cashService, IBaseRepository<CashAsset> cashRepository, ExternalPriceFacade priceFacade,
            IBankSavingService bankSavingService, ICustomAssetService customAssetService, ICryptoService cryptoService,
            IStockService stockService, IRealEstateService realEstateService)
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
            var singleAssetTransactionType = SingleAssetTransactionTypes.BuyFromOutside;
            var resultReferentialAssetId = -1;
            string resultReferentialAssetType = null;
            string resultReferentialAssetName = null; 
            if (isUsingInvestFund)
            {
                singleAssetTransactionType = SingleAssetTransactionTypes.BuyFromFund;
                resultReferentialAssetId = -1;
                resultReferentialAssetType = "fund";
                resultReferentialAssetName = "fund"; 
            }

            if (isUsingCash && usingCashId is not null)
            {
                var foundCash = _cashService.GetById(usingCashId.Value);
                singleAssetTransactionType = SingleAssetTransactionTypes.BuyFromCash;
                resultReferentialAssetId = usingCashId.Value;
                resultReferentialAssetType = foundCash.GetAssetType();
                resultReferentialAssetName = foundCash.Name; 
            }
            var newAssetTransaction = new SingleAssetTransaction()
            {
                SingleAssetTransactionTypes = singleAssetTransactionType,
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


        public async Task<SingleAssetTransaction> CreateAddValueTransaction(PersonalAsset asset,
            decimal amountInAssetUnit, decimal? valueInCurrency,
            string currency, decimal? fee, decimal? tax)
        {
            var resultAdd = await asset.AddValue(amountInAssetUnit);
            if (!resultAdd) throw new ApplicationException("The value after add is negative");

            var singleAssetTransaction = new SingleAssetTransaction()
            {
                Amount = valueInCurrency ?? decimal.Zero,
                CurrencyCode = currency,
                AmountInDestinationAssetUnit = amountInAssetUnit,
                CreatedAt = DateTime.Now,
                DestinationAssetId = asset.Id,
                DestinationAssetType = asset.GetAssetType(),
                DestinationAssetName = asset.Name,
                LastChanged = DateTime.Now,
                Fee = fee,
                Tax = tax
            };
            _transactionRepository.Insert(singleAssetTransaction);
            return singleAssetTransaction;
        }

        public decimal CalculateSubTransactionProfitLoss
            (IEnumerable<SingleAssetTransaction> singleAssetTransactions, string currencyCode)
        {
            return singleAssetTransactions.Sum(transaction => transaction.SingleAssetTransactionTypes switch
            {
                SingleAssetTransactionTypes.MoveToFund => transaction.Amount,
                SingleAssetTransactionTypes.WithdrawToCash => transaction.Amount,
                SingleAssetTransactionTypes.AddValue => -transaction.Amount,
                SingleAssetTransactionTypes.BuyFromFund => 0,
                _ => 0
            });
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
            PersonalAsset sourceAsset = null;
            if (sourceAssetId is not null)
            {
                sourceAsset = createTransactionDto.ReferentialAssetType switch
                {
                    "bankSaving" => _bankSavingService.GetById(sourceAssetId.Value),
                    "custom" => _customAssetService.GetById(sourceAssetId.Value),
                    "crypto" => _cryptoService.GetById(sourceAssetId.Value),
                    "stock" => _stockService.GetById(sourceAssetId.Value),
                    "realEstate" => _realEstateService.GetById(sourceAssetId.Value),
                    "cash" => _cashService.GetById(sourceAssetId.Value),
                    _ => throw new ArgumentException()
                };
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

            if (createTransactionDto.ReferentialAssetId != null)
            {
                var newTransaction = new SingleAssetTransaction
                {
                    ReferentialAssetId = sourceAssetId.Value,
                    ReferentialAssetType = createTransactionDto.ReferentialAssetType,
                    ReferentialAssetName = sourceAsset.Name,
                    Amount = createTransactionDto.Amount,
                    CurrencyCode = createTransactionDto.CurrencyCode,
                    SingleAssetTransactionTypes = SingleAssetTransactionTypes.WithdrawToCash,
                    Fee = createTransactionDto.Fee,
                    Tax = createTransactionDto.Tax,
                    CreatedAt = DateTime.Now,
                    LastChanged = DateTime.Now,
                    DestinationAssetId = foundCash.Id,
                    DestinationAssetName = foundCash.Name,
                    DestinationAssetType = foundCash.GetAssetType(),
                    DestinationAmount = valueToAddToCash,
                    DestinationCurrency = foundCash.CurrencyCode,
                };
                _transactionRepository.Insert(newTransaction);

                return newTransaction;
            }

            throw new InvalidOperationException();
        }

        public async Task<SingleAssetTransaction> Fake()
        {
            return new SingleAssetTransaction();
        }
    }
}