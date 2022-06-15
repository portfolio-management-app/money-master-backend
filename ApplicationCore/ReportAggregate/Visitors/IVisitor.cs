using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate.Visitors
{
    public interface IVisitor
    {
        Task<IEnumerable<ProfitLossBasis>> VisitCrypto(Crypto asset);
    }
}