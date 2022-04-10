using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class AddFinancialDocumentInput2
    {
        public FinancialDocumentType Type { get; set; }
        public int FinancialAccountId { get; set; }
        public double CreditAmount { get; set; }
        public double DebitAmount { get; set; }
        public string FileKey { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Description { get; set; }
    }
}
