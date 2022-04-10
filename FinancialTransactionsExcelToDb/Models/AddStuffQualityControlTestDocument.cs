using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddStuffQualityControlTestDocument
    {
        public long QualityControlTestId { get; set; }
        public string FileKey { get; set; }
        public AddStuffQualityControlTestConditionInput[] AddStuffQualityControlTestConditionInputs { get; set; }
    }
}
