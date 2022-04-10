using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class AddFinancialDocumentInput
    {
        public FinancialDocumentType Type { get; set; }
        public int FinancialAccountId { get; set; }
        public string CooperatorName { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
        public string FileKey { get; set; }
        public AddFinancialDocumentTransferInput FinancialDocumentTransfer { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsFromParlar { get; set; }
    }
}
