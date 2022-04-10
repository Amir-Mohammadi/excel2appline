using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddProductionOperationInput
    {
        public long Time { get; set; }
        public int[] EmployeeIds { get; set; }
        public int? ProductionOperatorId { get; set; }
        public int OperationId { get; set; }
        public AddRepairProductionStuffDetailInput[] AddProductionStuffDetails { get; set; }
    }
}
