using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddDetemineLinkForSerialInput
    {
        public string LinkSerial { get; set; }
        public int StuffId { get; set; }
        public long StuffSerialCode { get; set; }
    }
}
