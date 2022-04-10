using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    [Flags]
    public enum PurchaseOrderStatus
    {
        None = 0,
        NotAction = 1,
        Cargoing = 2,
        Cargoed = 4,
        Receipting = 8,
        Receipted = 16,
        QualityControling = 32,
        QualityControled = 64
    }
}
