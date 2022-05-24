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

        public SingleAssetTransaction AddCreateNewAssetTransaction(PersonalAsset asset, decimal moneyAmount, string currency)
        {
            var newAssetTransaction = new SingleAssetTransaction()
            {
                SingleAssetTransactionTypes = SingleAssetTransactionTypes.NewAsset,
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.Self,
                ReferentialAssetId = asset.Id,
                ReferentialAssetType = asset.GetAssetType(),
                ReferentialAssetName = asset.Name, 
                Amount = moneyAmount,
                CreatedAt = DateTime.Now,
                CurrencyCode = currency,
                LastChanged = DateTime.Now

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

        public async Task<SingleAssetTransaction> CreateWithdrawToCashTransaction(PersonalAsset asset, int destinationCashId, decimal amount, string currencyCode, bool isTransferringAll)
        {
            var foundCash = _cashRepository.GetFirst( c => c.Id == destinationCashId );
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
                    var realValueToAdd = rateObj.GetValue(destinationCurrencyCode) * amount;
                    foundCash.Amount += realValueToAdd;
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
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.OtherAsset
            };
            _transactionRepository.Insert(transaction);
            return transaction;
        }

        public async Task<SingleAssetTransaction> CreateAddValueTransaction(PersonalAsset asset, decimal amountInAssetUnit, decimal? valueInCurrency,
            string currency)
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
                SingleAssetTransactionTypes.SellAsset => transaction.Amount,
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