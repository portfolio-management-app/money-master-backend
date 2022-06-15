using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate.Models;

namespace ApplicationCore.ReportAggregate
{
    public interface IReportService
    {
        Task<List<PieChartElementModel>> GetPieChart(int portfolioId);
        Task<List<SankeyFlowBasis>> GetSankeyChart(int portfolioId);

        Task<List<ProfitLossBasis>> GetPeriodProfitLossByAsset(int assetId, string assetType, string period = "day");
    }
}