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
    public partial class AddWarehouseIssue : Form
    {
        string token = "";
        List<WarehouseInventoryResult> warehouseInventoriesList;

        public AddWarehouseIssue()
        {
            InitializeComponent();

            warehouseInventoriesList = new List<WarehouseInventoryResult>();
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

            var warehouseIds = new int[]
            {
                3, //مصرف DIP 1	
                44, // مصرف DIP 2	
                81, // مصرف DIP 3	
                82, // مصرف DIP 4	
                1086,  // مصرف DIP5	

                12, // محصول Packing 1	
                13, // مصرف Packing 1
                46, // مصرف Packing 2	
                47, // محصول Packing 2	
                75,  // مصرف Packing 3	
                76, // مصرف Packing 4	
                79, // محصول Packing 3
                80, // محصول Packing 4	
                84, // مصرف packing مرجوعی	
                85, // محصول مرجوعی
                1087, // مصرف Packing 5	
                1088, // محصول Packing 5

                // 69, // مصرف Assembly 1	
                71, // مصرف Assembly 2	
                72, // مصرف Assembly 3	
                73 // مصرف Assembly 4	
            };

            foreach (var warehouseId in warehouseIds)
            {
                GetWarehouseInventoriesInput input = new GetWarehouseInventoriesInput(null, WarehouseInventorySortType.Serial, System.Data.SqlClient.SortOrder.Ascending)
                {
                    AdvanceSearchItems = new AdvanceSearchItem[] { },
                    WarehouseId = warehouseId,
                    GroupBySerial = true
                };

                var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/GetWarehouseInventories", input);
                var result = JsonConvert.DeserializeObject<ResultList<WarehouseInventoryResult>>(json);

                warehouseInventoriesList.AddRange(result.Data);
            }

            warehouseInventoriesList = warehouseInventoriesList.Where(i => i.TotalAmount.HasValue).ToList();

            richTextBox1.AppendText($"\nTotal Serials Count: { warehouseInventoriesList.Count() }");

            btnGetIds.Enabled = true;
        }

        private async void btnAddRialInvoice_Click(object sender, EventArgs e)
        {
            btnAddRialInvoice.Enabled = false;

            var description = "طبق نامه 247614، جهت تسهیل امور رفع مغایرت انبارگردانی";

            var counter = 0;
            foreach (var warehouseInventory in warehouseInventoriesList)
            {
                if (warehouseInventory.BlockedAmount > 0)
                {
                    GetWarehouseIssuesInput getWarehouseIssuesInput = new GetWarehouseIssuesInput(null, WarehouseIssueSortType.Id, System.Data.SqlClient.SortOrder.Ascending)
                    {
                        AdvanceSearchItems = new AdvanceSearchItem[] { },
                        Serial = warehouseInventory.Serial,
                        Status = WarehouseIssueStatusType.Waiting
                    };

                    var getWarehouseIssuesJson = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/GetWarehouseIssues", getWarehouseIssuesInput);
                    var getWarehouseIssuesResult = JsonConvert.DeserializeObject<ResultList<WarehouseIssueResult>>(getWarehouseIssuesJson);
                    var warehouseIssue = getWarehouseIssuesResult.Data.FirstOrDefault();

                    if (warehouseIssue != null)
                    {
                        RejectWarehouseIssueInput rejectWarehouseIssueInput = new RejectWarehouseIssueInput()
                        {
                            Id = warehouseIssue.Id,
                            FromWarehouseId = warehouseIssue.FromWarehouseId,
                            ToWarehouseId = warehouseIssue.ToWarehouseId,
                            Description = description,
                            RowVersion = warehouseIssue.RowVersion
                        };

                        var rejectWarehouseIssueJson = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/RejectWarehouseIssue", rejectWarehouseIssueInput);
                        var rejectWarehouseIssueResult = JsonConvert.DeserializeObject<Result>(rejectWarehouseIssueJson);
                    }
                }

                AddWarehouseIssueInput input = new AddWarehouseIssueInput
                {
                    AddWarehouseIssueItems = new AddWarehouseIssueItemInput[]
                    {
                        new AddWarehouseIssueItemInput
                        {
                            Serial = warehouseInventory.Serial,
                            Amount = warehouseInventory.TotalAmount ?? 0,
                            StuffId = warehouseInventory.StuffId,
                            UnitId = warehouseInventory.UnitId,
                            Description = description
                        }
                    },
                    FromWarehouseId = warehouseInventory.WarehouseId,
                    ToWarehouseId = 69,
                    Description = description
                };
                var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/AddWarehouseIssue", input);
                var result = JsonConvert.DeserializeObject<Result>(json);


                counter++;
                richTextBox1.AppendText($"\nCounter: {counter} of {warehouseInventoriesList.Count()} - {DateTime.Now} - Serial: {warehouseInventory.Serial} - Result: {json}");

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnAddRialInvoice.Enabled = true;
        }
    }
}
