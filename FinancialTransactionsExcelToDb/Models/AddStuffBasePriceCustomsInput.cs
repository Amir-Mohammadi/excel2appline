using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddStuffBasePriceCustomsInput
    {
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public double Percent { get; set; }
        public int? HowToBuyId { get; set; }
        public double HowToBuyRatio { get; set; }
        public double? Tariff { get; set; }
        public double? Weight { get; set; }
        public StuffBasePriceCustomsType Type { get; set; }
    }
}
