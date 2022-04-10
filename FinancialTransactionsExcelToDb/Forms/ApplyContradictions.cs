using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialTransactionsExcelToDb.Forms
{
    public partial class ApplyContradictions : Form
    {
        string token = "";
        List<StockTakingVarianceResult> stockTakingVariaces;
        List<string> errorSerials;


        public ApplyContradictions()
        {
            InitializeComponent();

            stockTakingVariaces = new List<StockTakingVarianceResult>();
            errorSerials = new List<string>();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            token = await Common.Common.LoginUser("Machine", "MachineParlar");

            btnLogin.Enabled = true;
        }

        private async void btnGetIds_Click(object sender, EventArgs e)
        {
            btnGetIds.Enabled = false;

            var stuffCodes = new string[]
            {
            };

            var serials = new string[]
            {
            };

            var stuffIds = new List<int>();

            foreach (var stuffCode in stuffCodes)
            {
                GetStuffsInput getStuffsInput = new GetStuffsInput(null, StuffSortType.Code, System.Data.SqlClient.SortOrder.Ascending)
                {
                    AdvanceSearchItems = new AdvanceSearchItem[] { },
                    Code = stuffCode
                };
                var stuffsJson = await Common.Common.Post(token: token, requestUri: "api/SaleManagement/GetStuffs", getStuffsInput);
                var stuffsResult = JsonConvert.DeserializeObject<ResultList<StockTakingVarianceResult>>(stuffsJson);
                var stuffId = stuffsResult.Data.FirstOrDefault()?.Id;

                if (stuffId != null)
                    stuffIds.Add(stuffId.Value);
            }

            GetStockTakingVariancesInput input = new GetStockTakingVariancesInput(null, StockTakingVarianceSortType.StockCheckingId, System.Data.SqlClient.SortOrder.Ascending)
            {
                AdvanceSearchItems = new AdvanceSearchItem[] { },
                GroupBySerial = true,
                StockCheckingId = 24,
                TagTypeId = 1,
                WarehouseId = 83,
                //StuffIds = stuffIds.ToArray(),
                //Serials = serials,
                Statuses = new StockCheckingTagStatus[]
                    {
                        StockCheckingTagStatus.None,
                        StockCheckingTagStatus.NotCounted,
                        StockCheckingTagStatus.CorrectCounting,
                        StockCheckingTagStatus.Contradiction,
                        StockCheckingTagStatus.NonSerialTag,
                        StockCheckingTagStatus.NotInventory
                    }
            };

            var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/GetStockTakingVariances", input);
            var result = JsonConvert.DeserializeObject<ResultList<StockTakingVarianceResult>>(json);

            var resultData = result.Data.Where(i =>
                i.ContradictionAmount > 0 &&
                (i.QtyCorrectionRequestStatus == null || i.QtyCorrectionRequestStatus != QtyCorrectionRequestStatus.Accepted));
            stockTakingVariaces.AddRange(resultData);

            btnGetIds.Enabled = true;
        }

        private async void btnAddRialInvoice_Click(object sender, EventArgs e)
        {
            btnAddRialInvoice.Enabled = false;

            stockTakingVariaces = stockTakingVariaces.OrderBy(i => i.StuffCode).ToList();

            var counter = 0;
            foreach (var stockTakingVariance in stockTakingVariaces)
            {
                CorrectWarehouseInventoriesInput input = new CorrectWarehouseInventoriesInput
                {
                    Serials = new GetStuffSerialInput[]
                    {
                        new GetStuffSerialInput
                        {
                            Serial = stockTakingVariance.Serial,
                            StuffId = stockTakingVariance.StuffId,
                            Code = stockTakingVariance.StuffSerialCode
                        }
                    },
                    StockCheckingId = stockTakingVariance.StockCheckingId,
                    WarehouseId = stockTakingVariance.WarehouseId,
                    TagTypeId = stockTakingVariance.TagTypeId
                };
                var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/CorrectWarehouseInventories", input);
                var result = JsonConvert.DeserializeObject<Result>(json);

                counter++;
                richTextBox1.AppendText($"\nCounter: {counter} of {stockTakingVariaces.Count()} - {DateTime.Now} - Serial: {stockTakingVariance.Serial} - Result: {json}");

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnAddRialInvoice.Enabled = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            stockTakingVariaces = stockTakingVariaces.OrderBy(i => i.StuffCode).ToList();

            var counter = 0;
            foreach (var stockTakingVariance in stockTakingVariaces)
            {
                CorrectSerialWarehouseInventoryInput input = new CorrectSerialWarehouseInventoryInput
                {
                    Serial = stockTakingVariance.Serial,
                    StuffSerialStuffId = stockTakingVariance.StuffId,
                    StuffSerialCode = stockTakingVariance.StuffSerialCode,
                    StockCheckingId = stockTakingVariance.StockCheckingId,
                    WarehouseId = stockTakingVariance.WarehouseId,
                    TagTypeId = stockTakingVariance.TagTypeId,
                    ContradictionAmount = stockTakingVariance.ContradictionAmount,
                    StockCheckingTagId = stockTakingVariance.Id,
                    StockSerialAmount = stockTakingVariance.StockSerialAmount,
                    TagAmount = stockTakingVariance.TagAmount,
                    UnitId = stockTakingVariance.UnitId
                };
                var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/CorrectSerialWarehouseInventory", input);
                var result = JsonConvert.DeserializeObject<Result>(json);

                if (!result.Success)
                    errorSerials.Add(stockTakingVariance.Serial);

                counter++;
                richTextBox1.AppendText($"\n\nCounter: {counter} of {stockTakingVariaces.Count()} - {DateTime.Now} - Serial: {stockTakingVariance.Serial} - Result: {json}");

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            richTextBox1.AppendText($"\n\n----------------------- Error Serials -------------------");
            foreach (var errorSerial in errorSerials)
            {
                richTextBox1.AppendText($"\n\n{errorSerial}");
            }


            button1.Enabled = true;
        }
    }
}
