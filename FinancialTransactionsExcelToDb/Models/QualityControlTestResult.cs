using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class QualityControlTestResult
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public IQueryable<QualityControlTestConditionResult> QualityControlTestConditions { get; set; }
        //public TestConditionResult TestConditionResult { get; set; }

        public int TestConditionId { get; set; }
        public string TestConditionCondition { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
