using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgarCacheFramework.Models
{
    public class FinancialStatementInstance
    {

        //General data
        [Key]
        public int Id { get; set; }
        public long PullTime { get; set; }
        public string Ticker { get; set; }
        public long PeriodStart { get; set; }
        public long PeriodEnd { get; set; }

        //Income Statement Items
        public float? Revenue { get; set; }
        public float? SellingGeneralAndAdministrativeExpense { get; set; }
        public float? ResearchAndDevelopmentExpense { get; set; }
        public float? OperatingIncome { get; set; }
        public float? NetIncome { get; set; }

        //Balance Sheet Items
        public float? Assets { get; set; }
        public float? Liabilities { get; set; }
        public float? Equity { get; set; }
        public float? Cash { get; set; }
        public float? CurrentAssets { get; set; }
        public float? CurrentLiabilities { get; set; }
        public float? RetainedEarnings { get; set; }
        public long? CommonStockSharesOutstanding { get; set; }


        //Cash Flow Statement Items
        public float? OperatingCashFlows { get; set; }
        public float? InvestingCashFlows { get; set; }
        public float? FinancingCashFlows { get; set; }
        public float? ProceedsFromIssuanceOfDebt { get; set; }
        public float? PaymentsOfDebt { get; set; }
        public float? DividendsPaid { get; set; }

        public string xml { get; set; }
    }
}
