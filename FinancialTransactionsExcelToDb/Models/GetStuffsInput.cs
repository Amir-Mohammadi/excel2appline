using FinancialTransactionsExcelToDb.Common;
using FinancialTransactionsExcelToDb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class GetStuffsInput : SearchInput<StuffSortType>
    {
        public GetStuffsInput(PagingInput pagingInput, StuffSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
        {
        }

        public TValue<string> Code { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTraceable { get; set; }
        public bool? NeedToQualityControl { get; set; }
        public bool? NeedToQualityControlDocumentUpload { get; set; }
        public StuffType? StuffType { get; set; }
        public int? StuffCategoryId { get; set; }
        public int? QualityControlDepartmentId { get; set; }
        public int? QualityControlEmployeeId { get; set; }
        public bool? HasProjectHeader { get; set; }
        public StuffType[] StuffTypes { get; set; }
        public int? StuffPurchaseCategoryId { get; set; }
        public StuffDefinitionStatus? DefinitionStatus { get; set; }

    }
}
