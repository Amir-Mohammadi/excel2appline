using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class CorrectWarehouseInventoriesInput
    {
        public int StockCheckingId { get; set; }
        public int TagTypeId { get; set; }
        public int WarehouseId { get; set; }
        public GetStuffSerialInput[] Serials { get; set; }
    }
}
