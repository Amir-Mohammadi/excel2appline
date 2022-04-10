using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    //[DataContract]
    public class SearchInput<T> : SortInput<T>
    {
        //[DataMember]
        public string SearchText { get; set; }
        //[DataMember]
        public PagingInput PagingInput { get; set; }
        //[DataMember]
        public AdvanceSearchItem[] AdvanceSearchItems { get; set; }
        public SearchInput(PagingInput pagingInput, T sortType, SortOrder sortOrder) : base(sortOrder, sortType)
        {
            this.PagingInput = pagingInput;
        }
    }
}
