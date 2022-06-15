using System;

namespace ApplicationCore.ReportAggregate.Models
{
    public class ProfitLossBasis
    {
        public decimal Amount { get; set; }
        public string Unit { get; set; } // Currency 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}