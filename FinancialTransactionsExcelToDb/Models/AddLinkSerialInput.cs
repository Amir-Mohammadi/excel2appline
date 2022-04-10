using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddLinkSerialInput
    {
        public string[] LinkedSerials { get; set; }
        public int CustomerId { get; set; }
    }
}
