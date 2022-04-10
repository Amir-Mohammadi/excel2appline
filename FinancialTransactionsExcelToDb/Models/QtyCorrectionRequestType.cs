using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public enum QtyCorrectionRequestType : byte
    {
        Missing, // مفقودی
        IncreaseAmount, // افزایش موجودی
        DecreaseAmount, // کاهش موجودی
        IncreaseStockChecking, // افزایش موجودی انبارگردانی
        DecreaseStockChecking // کاهش موجودی انبارگردانی
    }
}
