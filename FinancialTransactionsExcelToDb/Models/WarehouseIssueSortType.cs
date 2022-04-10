using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public enum WarehouseIssueSortType
    {
        Id,
        Code,
        DateTime,
        FromWarehouseId,
        FromWarehouseName,
        ToWarehouseId,
        ToWarehouseName,
        Status,
        UserName,
        EmployeeFullName,
        ResponseUserName,
        ResponseDateTime,
        ResponseEmployeeFullName,
        ToDepartmentName,
        ToEmployeeFullName,
        ConfirmDescription
    }
}
