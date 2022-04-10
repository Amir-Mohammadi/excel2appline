using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class CorrectSerialWarehouseInventoryInput
    {
        public int StockCheckingId { get; set; }
        public int TagTypeId { get; set; }
        public int WarehouseId { get; set; }
        public int? StockCheckingTagId { get; set; }
        public double TagAmount { get; set; }
        public double StockSerialAmount { get; set; }
        public double ContradictionAmount { get; set; }
        public int UnitId { get; set; }
        public string Serial { get; set; }
        public long? StuffSerialCode { get; set; }
        public int StuffSerialStuffId { get; set; }
    }
}
