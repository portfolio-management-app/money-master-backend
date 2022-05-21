using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate.Visitors
{
    public interface IVisitor
    {
        Task<ProfitLossBasis> VisitCrypto(Crypto asset);
    }
}