using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class CloseSerialBufferInput
    {
        public string Serial { get; set; }
        public int WarehouseId { get; set; }
    }
}
