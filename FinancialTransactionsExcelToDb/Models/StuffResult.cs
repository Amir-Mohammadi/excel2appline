using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb
{
    public class StuffResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Noun { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int StuffCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeName { get; set; }
        public int ProjectHeaderId { get; set; }
        public string ProjectHeaderName { get; set; }
        public int StockSafety { get; set; }
        public double FaultyPercentage { get; set; }
        public bool NeedToQualityControl { get; set; }
        public bool IsTraceable { get; set; }
        public int? QualityControlDepartmentId { get; set; }
        public string QualityControlDepartmentName { get; set; }
        public int? QualityControlEmployeeId { get; set; }
        public string QualityControlEmployeeFullName { get; set; }
        public double Tolerance { get; set; }
        public double? Volume { get; set; }
        public double? Weight { get; set; }
        public int? StuffHsGroupId { get; set; }
        public string StuffHsGroupCode { get; set; }
        public string StuffHsGroupTitle { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
