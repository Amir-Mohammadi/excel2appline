using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetSerialBuffersInput : SearchInput<SerialBufferSortType>
    {
        public GetSerialBuffersInput(PagingInput pagingInput, SerialBufferSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }
        public string Serial { get; set; }
        public int? StuffId { get; set; }
        public string StuffCode { get; set; }
        public int? WarehouseId { get; set; }
        public int? ProductionLineId { get; set; }
        public int? ProductionTerminalId { get; set; }

    }
}
