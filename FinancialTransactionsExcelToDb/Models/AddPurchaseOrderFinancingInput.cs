using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class AddPurchaseOrderFinancingInput
    {
        public string Description { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Percent { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
    }
}
