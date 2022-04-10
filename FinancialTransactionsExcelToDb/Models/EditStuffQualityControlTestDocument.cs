using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class EditStuffQualityControlTestDocument
    {
        public long QualityControlTestId { get; set; }
        public string FileKey { get; set; }

        public AddStuffQualityControlTestConditionInput[] AddStuffQualityControlTestConditionInputs { get; set; }
        public DeleteStuffQualityControlTestConditionInput[] DeleteStuffQualityControlTestConditionInputs { get; set; }
    }
}
