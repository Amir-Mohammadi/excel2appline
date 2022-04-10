using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class AddFinancialDocumentTransferInput
    {
        public int ToFinancialAccountId { get; set; }
        public double ToAmount { get; set; }
    }
}
