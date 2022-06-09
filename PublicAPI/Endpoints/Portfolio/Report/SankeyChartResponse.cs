namespace PublicAPI.Endpoints.Portfolio.Report
{
    public class SankeyChartResponse
    {
        public string SourceType { get; set; }
        public string SourceName { get; set; }
        public int? SourceId { get; set; }
        public string TargetType { get; set; }
        public string TargetName { get; set; }
        public int? TargetId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}