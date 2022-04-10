using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddWarehouseIssueInput
    {
        public int FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public AddWarehouseIssueItemInput[] AddWarehouseIssueItems { get; set; }
        public string Description { get; set; }
        public int? ToEmployeeId { get; set; }
        public int? ToDepartmentId { get; set; }
        public TransactionLevel? TransactionLevel { get; set; }
    }
}
