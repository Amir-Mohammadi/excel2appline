using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class DeleteStuffQualityControlTestConditionInput
    {
        public int StuffId { get; set; }
        public long QualityControlTestId { get; set; }
        public long QualityControlTestConditionQualityControlTestId { get; set; }
        public int QualityControlConditionTestConditionId { get; set; }

    }
}
