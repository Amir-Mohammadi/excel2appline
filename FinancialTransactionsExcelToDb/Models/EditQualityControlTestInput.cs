using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class EditQualityControlTestInput
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddTestConditionInput[] AddTestConditions { get; set; }
        public DeleteTestConditionInput[] DeleteTestConditions { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
