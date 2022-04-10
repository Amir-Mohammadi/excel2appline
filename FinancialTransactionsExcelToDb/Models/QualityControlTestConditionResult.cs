using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class QualityControlTestConditionResult
    {
        public int TestConditionId { get; set; }
        public string Condition { get; set; }
        public long QualityControlTestId { get; set; }
        public string QualityControlTestName { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
