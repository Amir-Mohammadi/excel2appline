using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetQualityControlTestUnitsInput : SearchInput<QualityControlTestUnitSortType>
    {
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public GetQualityControlTestUnitsInput(PagingInput pagingInput, QualityControlTestUnitSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }
    }
}
