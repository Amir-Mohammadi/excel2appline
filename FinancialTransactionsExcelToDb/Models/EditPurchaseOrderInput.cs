using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class EditPurchaseOrderInput
    {
        public int Id { get; set; }
        public double Qty { get; set; }
        public double OrderedQty { get; set; }
        public int UnitId { get; set; }
        public double? Price { get; set; }
        public int? CurrencyId { get; set; }
        public int? ProviderId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime BuyDeadline { get; set; }
        public PurchaseOrderType PurchaseOrderType { get; set; }
        public AddPurchaseOrderFinancingInput[] AddPurchaseOrderFinancings { get; set; }
        public DeletePurchaseOrderFinancingInput[] DeletePurchaseOrderFinancings { get; set; }
        public PurchaseOrderDetailInput[] PurchaseOrderDetail { get; set; }
        public AddPurchaseOrderInput[] NewAddedPurchaseOrders { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
