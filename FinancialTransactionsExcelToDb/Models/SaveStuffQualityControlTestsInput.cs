using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class SaveStuffQualityControlTestsInput
    {
        public int StuffId { get; set; }
        public AddStuffQualityControlTestDocument[] AddQualityControlTestInputs { get; set; }
        public EditStuffQualityControlTestDocument[] EditQualityControlTestInputs { get; set; }
        public long[] DeleteQualityControlTestIds { get; set; }
    }
}
