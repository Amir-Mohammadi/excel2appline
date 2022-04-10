using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    [Flags]
    public enum ReceiptStatus
    {
        None = 0,
        Temporary = 1,
        NoReceipt = 2,
        TemporaryReceipt = 4,
        EternalReceipt = 8,
        Priced = 16,
    }
}
