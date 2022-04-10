using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddRepairProductionStuffDetailInput
    {
        public ProductionStuffDetailType ProductionStuffDetailType { get; set; }
        public int StuffId { get; set; }
        public int? BillOfMaterialVersion { get; set; }
        public long? StuffSerialCode { get; set; }
        public double Qty { get; set; }
        public int UnitId { get; set; }

        public int? ParentOperationId { get; set; }

        // [Backend only]
        public int? RepairProductoinFaultId { get; set; }
    }
}
