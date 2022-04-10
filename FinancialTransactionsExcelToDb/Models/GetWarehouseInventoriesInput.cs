using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetWarehouseInventoriesInput : SearchInput<WarehouseInventorySortType>
    {
        public GetWarehouseInventoriesInput(PagingInput pagingInput, WarehouseInventorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }

        public int? WarehouseId { get; set; }
        public int? StuffCategoryId { get; set; }
        public int? StuffId { get; set; }
        public int? BillOfMaterialVersion { get; set; }
        public bool GroupByBillOfMaterialVersion { get; set; }
        public string Serial { get; set; }
        public bool GroupBySerial { get; set; }
        public DateTime? FromEffectDateTime { get; set; }
        public DateTime? ToEffectDateTime { get; set; }
        public StuffSerialStatus[] SerialStatuses { get; set; }
    }
}
