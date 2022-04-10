using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    //[DataContract]
    public class SortInput<T>
    {
        //[DataMember]
        public SortOrder SortOrder;
        //[DataMember]
        public T SortType { get; set; }
        public SortInput(SortOrder sortOrder, T sortType)
        {
            SortOrder = sortOrder;
            SortType = sortType;
        }
    }
}
