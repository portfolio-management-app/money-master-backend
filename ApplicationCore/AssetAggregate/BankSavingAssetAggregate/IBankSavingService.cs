using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.BankSavingAssetAggregate
{
    public interface IBankSavingService: IBaseAssetService<BankSavingAsset>
    {
        BankSavingAsset AddBankSavingAsset(int portfolioId, CreateNewBankSavingAssetDto commandDto);
        BankSavingAsset EditBankSavingAsset(int portfolioId, int bankingAssetId, EditBankSavingAssetDto dto);
        Task<decimal> CalculateSumBankSavingByPortfolio(int portfolioId, string currencyCode);
    }
}