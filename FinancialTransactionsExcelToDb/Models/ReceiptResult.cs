using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class ReceiptResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CooperatorId { get; set; }
        public string CooperatorCode { get; set; }
        public string CooperatorName { get; set; }
        public string LadingCode { get; set; }
        public ReceiptStatus ReceiptStatus { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? ReceiptDateTime { get; set; }
        public int UserId { get; set; }
        public string EmployeeFullName { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
