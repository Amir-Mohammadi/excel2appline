using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddPurchaseOrderInput
    {
        public int Id { get; set; }
        public int StuffId { get; set; }
        public int PurchaseRequestId { get; set; }
        public double Qty { get; set; }
        public double OrderedQty { get; set; }
        public int UnitId { get; set; }
        public double? Price { get; set; }
        public int? CurrencyId { get; set; }
        public int? ProviderId { get; set; }
        public int? SupplierId { get; set; }
        public int PurchaseOrderGroupId { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime PurchaseOrderDateTime { get; set; }
    }
}
