using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class ProductionOrderResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? ProductionScheduleId { get; set; }
        public string ProductionScheduleCode { get; set; }
        public string StuffName { get; set; }
        public string StuffCode { get; set; }
        public int StuffId { get; set; }
        public int BillOfMaterialVersion { get; set; }
        public string BillOfMaterialTitle { get; set; }
        public int WorkPlanId { get; set; }
        public string WorkPlanTitle { get; set; }
        public int WorkPlanStepId { get; set; }
        public string WorkPlanStepTitle { get; set; }
        public double Qty { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public double ProducedQty { get; set; }
        public double? InProductionQty { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public long Duration { get; set; }
        public int WorkPlanVersion { get; set; }
        public DateTime ToDateTime { get; set; } // => StartDateTime.AddSeconds(Duration);
        public string OrderCode { get; set; }
        public string ProductionPlanCode { get; set; }
        public string ProductionRequestCode { get; set; }
        public int ProductionStepId { get; set; }
        public int ProductionLineId { get; set; }
        public string ProductionStepName { get; set; }
        public string ProductionLineName { get; set; }
        public ProductionOrderStatus Status { get; set; }
        public int ConsumeWarehouseId { get; set; }
        public string ConsumeWarehouseName { get; set; }
        public int ProductWarehouseId { get; set; }
        public string ProductWarehouseName { get; set; }
        public float UnitConversionRatio { get; set; }
        public float BillOfMaterialUnitConversionRatio { get; set; }
        public int BillOfMaterialUnitId { get; set; }
        public double BillOfMaterialValue { get; set; }
        public string Barcode { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
