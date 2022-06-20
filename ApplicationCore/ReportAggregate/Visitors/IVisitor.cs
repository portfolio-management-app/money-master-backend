using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate.Visitors
{
    public interface IVisitor
    {
        Task<IEnumerable<ProfitLossBasis>> VisitCrypto(Crypto asset, int period = 1);
         Task<IEnumerable<ProfitLossBasis>> VisitCash(CashAsset asset, int period = 1);
         
         
         Task<IEnumerable<ProfitLossBasis>> VisitBankSaving(BankSavingAsset asset, int period = 1);
         Task<IEnumerable<ProfitLossBasis>> VisitCustomAsset(CustomInterestAsset asset, int period = 1);
         Task<IEnumerable<ProfitLossBasis>> VisitStock(Stock asset, int period = 1);
         Task<IEnumerable<ProfitLossBasis>> VisitRealEstate(RealEstateAsset asset, int period = 1);
    }
}