using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class GetFinancialAccountSummaryInput
    {
        public int? FinancialAccountId { get; set; }
        public int? CooperatorId { get; set; }
        public int? CurrencyId { get; set; }
        public string FinancialAccountCode { get; set; }
        public DateTime? FromEffectDateTime { get; set; }
        public DateTime? ToEffectDateTime { get; set; }
        public bool? HasCorrectionDoc { get; set; }
    }
}
