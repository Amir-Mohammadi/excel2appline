using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class DeletePurchaseOrderFinancingInput
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
