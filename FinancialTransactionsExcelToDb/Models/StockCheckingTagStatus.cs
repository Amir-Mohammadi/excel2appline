using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public enum StockCheckingTagStatus
    {
        None = 0,
        NotCounted = 1,
        CorrectCounting = 2,
        Contradiction = 3,
        NonSerialTag = 4,
        NotInventory = 5
    }
}
