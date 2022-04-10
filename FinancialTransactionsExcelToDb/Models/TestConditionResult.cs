using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class TestConditionResult
    {
        public int Id { get; set; }
        public string Condition { get; set; }
        public int UserId { get; set; }
        public string EmployeeFullName { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
