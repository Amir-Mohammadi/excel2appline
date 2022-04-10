using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class PurchaseOrderDetailInput
    {
        public int Id { get; set; }
        public double Qty { get; set; }
        public double OrderedQty { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
