using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class AddConstantStuffBasePriceInput
    {
        public int? StuffPriceId { get; set; }
        public byte[] StuffPriceRowVersion { get; set; }
        public int[] StuffIds { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
    }
}
