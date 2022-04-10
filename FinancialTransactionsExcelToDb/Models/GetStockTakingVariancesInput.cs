using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetStockTakingVariancesInput : SearchInput<StockTakingVarianceSortType>
    {
        public GetStockTakingVariancesInput(PagingInput pagingInput, StockTakingVarianceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }

        public int StockCheckingId { get; set; }
        public int WarehouseId { get; set; }
        public int TagTypeId { get; set; }
        public int? StuffId { get; set; }
        public int[] StuffIds { get; set; }
        public StuffType? StuffType { get; set; }
        public int? StuffCategoryId { get; set; }
        public string Serial { get; set; }
        public bool GroupBySerial { get; set; }
        public StockCheckingTagStatus[] Statuses { get; set; }
        public string[] Serials { get; set; }
    }
}
