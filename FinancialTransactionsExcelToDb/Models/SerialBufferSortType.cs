using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public enum SerialBufferSortType
    {
        DamagedAmount,
        ShortageAmount,
        RemainingAmount,
        Serial,
        StuffCode,
        StuffName,
        UnitName,
        WarehouseName,
        ProductionLineName,
        ProductionTerminalName,
        InitialQty,
        SerialBufferType,
        EmployeeName,
        CreationTime
    }
}
