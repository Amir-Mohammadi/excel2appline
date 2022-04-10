using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class PagingInput
    {
        public int PageNumber { get; internal set; }
        public int PageSize { get; internal set; }
        public PagingInput(int pageNumber, int pageSize)
        {
            this.PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
