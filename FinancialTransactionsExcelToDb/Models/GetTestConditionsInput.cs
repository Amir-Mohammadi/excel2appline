using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetTestConditionsInput : SearchInput<TestConditionSortType>
    {
        public string Condition { get; set; }
        public GetTestConditionsInput(PagingInput pagingInput, TestConditionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }
    }
}
