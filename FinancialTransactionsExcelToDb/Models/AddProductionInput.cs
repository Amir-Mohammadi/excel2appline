using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddProductionInput
    {
        public int ProductionOrderId { get; set; }
        public int ProductionTerminalId { get; set; }
        public bool IsFailed { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }
        public AddProductionOperationInput[] AddProductionOperations { get; set; }
    }
}
