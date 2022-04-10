using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddWarehouseIssueItemInput
    {
        public int StuffId { get; set; }
        public string Serial { get; set; }
        public double Amount { get; set; }
        public int UnitId { get; set; }
        public string Description { get; set; }
    }
}
