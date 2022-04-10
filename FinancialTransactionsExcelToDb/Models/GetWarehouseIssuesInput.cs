using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetWarehouseIssuesInput : SearchInput<WarehouseIssueSortType>
    {
        public GetWarehouseIssuesInput(PagingInput pagingInput, WarehouseIssueSortType sortType, SortOrder sortOrder)
            : base(pagingInput, sortType, sortOrder)
        {
        }
        public int? FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public WarehouseIssueStatusType? Status { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string Serial { get; set; }
        public int? ToEmployeeId { get; set; }
        public int? ToDepartmentId { get; set; }
    }
}
