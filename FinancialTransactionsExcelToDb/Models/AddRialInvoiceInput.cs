using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddRialInvoiceInput
    {
        public int ReceiptId { get; set; }
        public string Description { get; set; }
    }
}
