using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetReceiptsInput : SearchInput<ReceiptSortType>
    {
        public GetReceiptsInput(PagingInput pagingInput, ReceiptSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CooperatorId { get; set; }
        public int? UserId { get; set; }
        public int? ReceiptId { get; set; }
        public int? StuffId { get; set; }
        public string ReceiptCode { get; set; }
        public string LadingCode { get; set; }
        public string CargoItemCode { get; set; }
        public string PurchaseOrderCode { get; set; }
        public int[] ReceiptIds { get; set; }
    }
}
