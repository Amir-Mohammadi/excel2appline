using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddStuffQualityControlTestConditionInput
    {
        public int StuffId { get; set; }
        public long QualityControlTestId { get; set; }
        public long QualityControlTestConditionQualityControlTestId { get; set; }
        public int QualityControlConditionTestConditionId { get; set; }

        public double Min { get; set; }
        public double Max { get; set; }
        public double TestedQty { get; set; }
        public int QualityControlTestUnitId { get; set; }
        public string AcceptanceLimit { get; set; }
        public ToleranceType ToleranceType { get; set; }
        public string Description { get; set; }
    }
}
