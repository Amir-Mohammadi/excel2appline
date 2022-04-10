using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class RejectWarehouseIssueInput
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
