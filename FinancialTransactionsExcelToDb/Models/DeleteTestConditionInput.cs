using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class DeleteTestConditionInput
    {
        public int TestConditionId { get; set; }
        public int QualityControlTestId { get; set; }

    }
}
