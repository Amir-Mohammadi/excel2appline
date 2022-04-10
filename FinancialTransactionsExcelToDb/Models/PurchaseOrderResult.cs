using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class PurchaseOrderResult
    {

        public int Id { get; set; }
        public int? FinancialTransacionBatchId { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime PurchaseOrderDateTime { get; set; }
        public int? ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderCode { get; set; }
        public int StuffId { get; set; }
        public string StuffCode { get; set; }
        public string StuffName { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
        public int? CurrencuyId { get; set; }
        public string CurrencyTitle { get; set; }
        public string CurrencySign { get; set; }
        public double Qty { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public double RemainedQty { get; set; }
        public PurchaseOrderStatus PurchaseOrderStatus { get; set; }
        public int StuffCategoryId { get; set; }
        public string StuffCategoryName { get; set; }
        public string EmployeeFullName { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierFullName { get; set; }
        public PurchaseOrderType PurchaseOrderType { get; set; }
        public double CargoedQty { get; set; }
        public double ReceiptedQty { get; set; }
        public double QualityControlPassedQty { get; set; }
        public double QualityControlFailedQty { get; set; }
        public IEnumerable<string> PurchaseRequestDescriptionArray { get; set; }

        public string PurchaseRequsetDescription
        {
            get
            {
                return PurchaseRequestDescriptionArray != null
                    ? string.Join(" ، ", PurchaseRequestDescriptionArray)
                    : "";
            }
        }
        public string LatestBaseEntityDocumentDescription { get; set; }
        public IEnumerable<string> PlanCodeArray { get; set; }
        public string PlanCode
        {
            get
            {
                return PlanCodeArray != null
                    ? string.Join(" ، ", PlanCodeArray)
                    : "";
            }
        }
        public int? PriceConfirmerId { get; set; }
        public string PriceConfirmerFullName { get; set; }
        public ConfirmationStatus? PriceConfirmationStatus { get; set; }
        public string PriceConfirmDescription { get; set; }
        public bool IsArchived { get; set; }
        public string PurchaseOrderGroupCode { get; set; }
        public int? PurchaseOrderGroupId { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
