using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddStuffBasePriceTransportInput
    {
        public StuffBasePriceTransportType Type = StuffBasePriceTransportType.Percentage;
        public StuffBasePriceTransportComputeType ComputeType = StuffBasePriceTransportComputeType.Weighing;
        public double Percent;
        public double Price;

    }
}
