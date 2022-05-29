using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;

namespace ApplicationCore.TransactionAggregate
{
    public class AssetTransactionService: IAssetTransactionService
    {

    
        private readonly IBaseRepository<SingleAssetTransaction> _transactionRepository;
        private readonly IBaseRepository<CashAsset> _cashRepository;
        private readonly ExternalPriceFacade _priceFacade;

        public AssetTransactionService( IBaseRepository<SingleAssetTransaction> transactionRepository, ICashService cashService, IBaseRepository<CashAsset> cashRepository, ExternalPriceFacade priceFacade)
        {
            _transactionRepository = transactionRepository;
            _cashRepository = cashRepository;
            _priceFacade = priceFacade;
        }

        public SingleAssetTransaction AddCreateNewAssetTransaction
            (PersonalAsset asset, decimal moneyAmount, string currency,bool isUsingInvestFund, decimal? fee,decimal? tax)
        {
            var newAssetTransaction = new SingleAssetTransaction()
            {
                SingleAssetTransactionTypes = isUsingInvestFund?SingleAssetTransactionTypes.BuyFromFund:SingleAssetTransactionTypes.NewAsset,
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.Self,
                ReferentialAssetId = asset.Id,
                ReferentialAssetType = asset.GetAssetType(),
                ReferentialAssetName = asset.Name, 
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
                trans => trans.ReferentialAssetId == asset.Id &&
                         trans.ReferentialAssetType == asset.GetAssetType());
            return listTransaction.ToList(); 
        }

        public async Task<SingleAssetTransaction> CreateWithdrawToCashTransaction
            (PersonalAsset asset, int destinationCashId, decimal amount, string currencyCode, bool isTransferringAll,
                decimal? fee,decimal? tax)
        {
            var foundCash = _cashRepository.GetFirst( c => c.Id == destinationCashId );
            decimal valueToAddToCash = 0 ; 
            string destinationCurrencyCode = foundCash.CurrencyCode;
          
            if (isTransferringAll)
            {
                var valueInDestinationCurrency = await asset.CalculateValueInCurrency(destinationCurrencyCode, 
                    _priceFacade);
                await asset.WithdrawAll();
                foundCash.Amount += valueInDestinationCurrency;
            }
            else
            {
                var mandatoryWithdrawAll = new[] { "bankSaving", "realEstate" };
                if (mandatoryWithdrawAll.Contains(asset.GetAssetType()))
                {
                    throw new InvalidOperationException($" Not allowed for partial withdraw: {asset.GetAssetType()}");
                }

                if (!await asset.Withdraw(amount, currencyCode, _priceFacade))
                {
                    throw new OperationCanceledException("Insufficient value");
                }

                if (destinationCurrencyCode == currencyCode)
                    foundCash.Amount += amount;
                else
                {
                    var rateObj = await _priceFacade.CurrencyRateRepository.GetRateObject(currencyCode);
                    valueToAddToCash = rateObj.GetValue(destinationCurrencyCode) * amount;
                    foundCash.Amount += valueToAddToCash;
                }
            }

            _cashRepository.Update(foundCash);
            var transaction = new SingleAssetTransaction()
            {
                Amount = amount,
                CurrencyCode = currencyCode,
                ReferentialAssetId = asset.Id,
                ReferentialAssetType = asset.GetAssetType(),
                ReferentialAssetName = asset.Name, 
                DestinationAssetId = foundCash.Id,
                DestinationAssetType = "cash",
                DestinationAssetName = foundCash.Name,
                SingleAssetTransactionTypes = SingleAssetTransactionTypes.WithdrawValue,
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.OtherAsset,
                Fee = fee,
                Tax = tax
            };

            var cashAssetTransaction = new SingleAssetTransaction()
            {
                Amount = valueToAddToCash,
                CurrencyCode = currencyCode,
                ReferentialAssetId = asset.Id,
                ReferentialAssetType = asset.GetAssetType(),
                ReferentialAssetName = asset.Name,
                DestinationAssetId = foundCash.Id,
                DestinationAssetType = foundCash.GetAssetType(),
                DestinationAssetName = foundCash.Name,
                SingleAssetTransactionTypes = SingleAssetTransactionTypes.AddValue,
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.OtherAsset,
                Fee = fee,
                Tax = tax
            }; 
            _transactionRepository.InsertRange(new List<SingleAssetTransaction>() {transaction,cashAssetTransaction});
            return transaction;
        }

        public async Task<SingleAssetTransaction> CreateAddValueTransaction(PersonalAsset asset, decimal amountInAssetUnit, decimal? valueInCurrency,
            string currency, decimal? fee,decimal? tax)
        {
            var resultAdd = await asset.AddValue(amountInAssetUnit);
            if (!resultAdd)
            {
                throw new ApplicationException("The value after add is negative");
            }

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
                SingleAssetTransactionTypes.WithdrawValue => transaction.Amount,
                SingleAssetTransactionTypes.AddValue => -transaction.Amount,
                SingleAssetTransactionTypes.NewAsset => 0,
                _ => 0
            });
        }

        public async Task<SingleAssetTransaction> Fake()
        {
            return new SingleAssetTransaction(); 
        }
        
    }
}