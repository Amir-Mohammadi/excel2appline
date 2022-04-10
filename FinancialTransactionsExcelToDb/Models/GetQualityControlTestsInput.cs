using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetQualityControlTestsInput : SearchInput<QualityControlTestSortType>
    {
        public string Name { get; set; }
        public GetQualityControlTestsInput(PagingInput pagingInput, QualityControlTestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }
    }
}
