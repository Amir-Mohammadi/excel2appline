using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetStuffSerialInput
    {
        public string Serial { get; set; }
        public int? StuffId { get; set; }
        public long? Code { get; set; }
        public int? ProductionOrderId { get; set; }
    }
}
