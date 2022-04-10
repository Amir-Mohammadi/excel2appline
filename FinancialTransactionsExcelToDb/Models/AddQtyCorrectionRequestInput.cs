using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddQtyCorrectionRequestInput
    {
        public int? StockCheckingTagId;
        public short WarehouseId { get; set; }
        public int StuffId { get; set; }
        public string Serial { get; set; }
        public string Description { get; set; }
        public double Qty { get; set; }
        public byte UnitId { get; set; }
        public QtyCorrectionRequestType Type { get; set; }
    }
}
