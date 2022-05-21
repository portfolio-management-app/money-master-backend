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

        public async Task<SingleAssetTransaction> WithdrawToCash(PersonalAsset asset, int destinationCashId, decimal amount, string currencyCode, bool isTransferringAll)
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
                DestinationAssetId = foundCash.Id,
                DestinationAssetType = "cash",
                SingleAssetTransactionTypes = SingleAssetTransactionTypes.WithdrawValue,
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.OtherAsset
            };
            _transactionRepository.Insert(transaction);
            return transaction;
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