using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    [Flags]
    public enum ProductionOrderStatus
    {
        None = 0,
        NotAction = 1,
        ProductionMaterialRequested = 2,
        InProduction = 4,
        Produced = 8,
        Finished = 16
    }
}
