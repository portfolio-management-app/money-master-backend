using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate.Visitors
{
    public abstract class CalculatePeriodProfitLossVisitor: IVisitor
    {
        public abstract Task<ProfitLossBasis> VisitCrypto(Crypto asset);
    }
}