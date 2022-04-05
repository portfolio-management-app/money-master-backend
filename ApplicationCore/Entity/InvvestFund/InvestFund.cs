using System.Collections.Generic;
using ApplicationCore.Entity.Transactions;

namespace ApplicationCore.Entity.InvvestFund
{
    public class InvestFund: BaseEntity
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal CurrentCurrency { get; set; }
        public List<Transaction> InGoingTransaction { get; set; }
        public List<Transaction> OutGoingTransaction { get; set; }
        
    }
}