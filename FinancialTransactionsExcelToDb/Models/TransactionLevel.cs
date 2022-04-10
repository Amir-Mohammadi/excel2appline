using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public enum TransactionLevel
    {
        Available = 1,
        Blocked = 2,
        QualityControl = 3,
        Plan = 4,
        Waste = 5
    }
}
