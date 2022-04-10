using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetPurchaseOrdersInput
    {
        public int? StuffId { get; set; }
        public int? StuffCategoryId { get; set; }
        public string Code { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? FromDeadlineDateTime { get; set; }
        public DateTime? ToDeadlineDateTime { get; set; }
        public PurchaseOrderStatus? PurchaseOrderStatus { get; set; }
        public double? Price { get; set; }
        public double? Qty { get; set; }
        public string StuffCode { get; set; }
        public int? HowToBuyId { get; set; }
        public int? CargoId { get; set; }
        public int? ProviderId { get; set; }
        public int? CurrencyId { get; set; }
        public int? EmployeeId { get; set; }
        public int? SupplierId { get; set; }
        public PurchaseOrderType? PurchaseOrderType { get; set; }
        public int[] PurchaseOrderIds { get; set; }
        public PurchaseOrderStatus[] PurchaseOrderStatuses { get; set; }
        public PurchaseOrderStatus[] PurchaseOrderNotHasStatuses { get; set; }
        public int[] Ids { get; set; }
        public int? FinancialTransactionBatchId { get; set; }
        public string PlanCode { get; set; }
        public string PurchaseRequsetDescription { get; set; }
        public string PurchaseOrderGroupCode { get; set; }
        public int? PurchaseOrderGroupId { get; set; }
        public bool? IsArchived { get; set; }
    }
}
