using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class GetPurchaseOrderInput
    {
        public int? Id { get; set; }
        public string Code { get; set; }
    }
}
