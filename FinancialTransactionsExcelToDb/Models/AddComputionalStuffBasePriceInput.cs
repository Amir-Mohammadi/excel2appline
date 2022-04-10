using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddComputionalStuffBasePriceInput
    {
        //public int StuffId { get; set; }
        public int StuffPriceId { get; set; }
        public byte[] StuffPriceRowVersion { get; set; }
        public int[] StuffIds { get; set; }
        public double MainPrice { get; set; }
        public int CurrencyId { get; set; }
        public AddStuffBasePriceCustomsInput StuffBasePriceCustoms { get; set; }
        public AddStuffBasePriceTransportInput StuffBasePriceTransport { get; set; }
    }
}
