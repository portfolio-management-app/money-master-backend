using System.Diagnostics;
using System.Threading.Tasks;

namespace ApplicationCore.Entity.Transactions
{
    public class SingleAssetTransaction : Transaction
    {
        public SingleAssetTransactionType SingleAssetTransactionType { get; set; }
        public int? DestinationAssetId { get; set; } = null;
        public string DestinationAssetType { get; set; } = null;
        public string DestinationAssetName { get; set; } = null;

        public decimal DestinationAmount { get; set; }
        public string DestinationCurrency { get; set; }


        public SingleAssetTransaction()
        {
        }

        public async Task<decimal> CalculateValueInCurrency(string inputCurrency, ExternalPriceFacade priceFacade)
        {
            if (inputCurrency == CurrencyCode)
                return Amount;
            var rateObj = await priceFacade.CurrencyRateRepository.GetRateObject(CurrencyCode);
            return Amount * rateObj.GetValue(inputCurrency);
        }


        public async Task<decimal> CalculateSumOfTaxAndFee(string inputCurrency, ExternalPriceFacade priceFacade)
        {
            var taxAmount = decimal.Zero;
            var feeAmount = decimal.Zero; 
            if (Tax != null)
            {
                taxAmount = await CalculateValueInCurrency(inputCurrency, priceFacade) * Tax.Value / 100;
            }

            if (Fee != null)
            {
                var rateObj = await priceFacade.CurrencyRateRepository.GetRateObject(CurrencyCode);
                feeAmount = rateObj.GetValue(inputCurrency) * Fee.Value;
            }
            return taxAmount + feeAmount;
        }
    }
}