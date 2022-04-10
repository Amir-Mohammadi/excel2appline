using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinancialTransactionsExcelToDb.Common;
using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;

namespace FinancialTransactionsExcelToDb.Forms
{
    public partial class AddQtyCorrectionRequest : Form
    {
        public AddQtyCorrectionRequest()
        {
            InitializeComponent();
        }
        private string token;
        string fileName = "Remove_WHSerials_00_11_18_02";


        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel._Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            token = await Common.Common.LoginUser("Machine", "MachineParlar");

            btnLogin.Enabled = true;
        }
        private async Task<int> GetStuffId(GetStuffsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/SaleManagement/GetStuffs", input);
            var result = JsonConvert.DeserializeObject<ResultList<StuffResult>>(json);

            return result.Data[0].Id;
        }
        private async Task<WarehouseInventoryResult> GetStuffSerialInventory(GetWarehouseInventoriesInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/GetWarehouseInventories", input);
            var result = JsonConvert.DeserializeObject<ResultList<WarehouseInventoryResult>>(json);

            return result.Data[0];
        }

        private async Task<QtyCorrectionRequestResult> AddSerialQtyCorrectionRequest(AddQtyCorrectionRequestInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/AddQtyCorrectionRequest", input);
            var result = JsonConvert.DeserializeObject<Result<QtyCorrectionRequestResult>>(json);

            return result.Data;
        }

        private async Task<Result> AcceptQtyCorrectionRequest(AcceptQtyCorrectionRequestInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/AcceptQtyCorrectionRequest", input);
            var result = JsonConvert.DeserializeObject<Result>(json);

            return result;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            btnLoadFile.Enabled = false;

            fileName = Application.StartupPath + "\\New folder\\" + fileName;

            //fileName = @"‪D:\excel2line\FinancialTransactionsExcelToDb\bin\Debug\New folder\Remove_WHSerials_00_11_18_02.xls";
            OpenFile(fileName);

            btnLoadFile.Enabled = true;
        }
        private void OpenFile(string fileName)
        {
            _xlApp = new Excel.Application();

            _xlWorkbook = _xlApp.Workbooks.Open(fileName);
            _xlWorksheet = _xlWorkbook.Sheets[1];
            _xlRange = _xlWorksheet.UsedRange;
        }

        private void CloseFile()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(_xlRange);
            Marshal.ReleaseComObject(_xlWorksheet);

            //close and release
            _xlWorkbook.Close();
            Marshal.ReleaseComObject(_xlWorkbook);

            //quit and release
            _xlApp.Quit();
            Marshal.ReleaseComObject(_xlApp);
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            btnInsert.Enabled = false;
            //2
            for (int row = 1260; row < 2594; row++)
            {
                _sb.AppendLine("Row: " + row);

                GetWarehouseInventoriesInput getWarehouseInventoriesInput = new GetWarehouseInventoriesInput(pagingInput: null, sortType: WarehouseInventorySortType.StuffCode, sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                getWarehouseInventoriesInput.AdvanceSearchItems = new AdvanceSearchItem[0];

                var serial = _xlRange.Cells[row, 1];
                var description = _xlRange.Cells[row, 2];
                description = (string)description.Value2.ToString();
                description = "طبق نامه شماره 288747" + "\n" + description;
                if (serial != null)
                {
                    serial = (string)serial.Value2.ToString();
                    _sb.AppendLine("serial: " + serial);
                    getWarehouseInventoriesInput.Serial = serial;
                    WarehouseInventoryResult stuffSerialInventory = null;
                    try
                    {
                        stuffSerialInventory = await GetStuffSerialInventory(getWarehouseInventoriesInput);
                    }
                    catch
                    {
                        _sb.AppendLine("failed for get inventory:" + serial);
                        _sb.AppendLine("\n");
                        richTextBox1.Text = _sb.ToString();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();
                        continue;
                    }
                    AddQtyCorrectionRequestInput addQtyCorrectionRequestInput = new AddQtyCorrectionRequestInput();
                    addQtyCorrectionRequestInput.Serial = serial;
                    addQtyCorrectionRequestInput.WarehouseId = 1084;
                    addQtyCorrectionRequestInput.StuffId = stuffSerialInventory.StuffId;
                    addQtyCorrectionRequestInput.UnitId = stuffSerialInventory.UnitId;
                    addQtyCorrectionRequestInput.Qty = stuffSerialInventory.TotalAmount.Value;
                    addQtyCorrectionRequestInput.Type = QtyCorrectionRequestType.DecreaseAmount;
                    addQtyCorrectionRequestInput.Description = description;
                    QtyCorrectionRequestResult serialQtyCorrectionRequest = null;
                    try
                    {
                        serialQtyCorrectionRequest = await AddSerialQtyCorrectionRequest(addQtyCorrectionRequestInput);
                        var addResultJson = JsonConvert.SerializeObject(serialQtyCorrectionRequest);
                        _sb.AppendLine(addResultJson);
                        _sb.AppendLine("\n");

                        richTextBox1.Text = _sb.ToString();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();
                    }
                    catch
                    {
                        _sb.AppendLine("failed for add Correction Request:" + serial);
                        _sb.AppendLine("\n");
                        richTextBox1.Text = _sb.ToString();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();

                        continue;
                    }
                    try
                    {
                        AcceptQtyCorrectionRequestInput acceptQtyCorrectionRequestInput = new AcceptQtyCorrectionRequestInput();
                        acceptQtyCorrectionRequestInput.Id = serialQtyCorrectionRequest.Id;
                        acceptQtyCorrectionRequestInput.RowVersion = serialQtyCorrectionRequest.RowVersion;
                        acceptQtyCorrectionRequestInput.Description = description;
                        var acceptQtyCorrectionRequest = await AcceptQtyCorrectionRequest(acceptQtyCorrectionRequestInput);
                        var addResultJson2 = JsonConvert.SerializeObject(acceptQtyCorrectionRequest);
                        _sb.AppendLine(addResultJson2);
                        _sb.AppendLine("\n");
                        richTextBox1.Text = _sb.ToString();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();
                    }
                    catch
                    {
                        _sb.AppendLine("failed for accept Correction Request:" + serial);
                        _sb.AppendLine("\n");
                        richTextBox1.Text = _sb.ToString();
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();

                        continue;
                    }
                }
                _sb.AppendLine("\n");
                _sb.AppendLine("\n");
                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            CloseFile();

            string logFileName = fileName.Split('.')[0] + "_" + GetTimestamp(DateTime.Now) + ".txt";

            using (FileStream fs = File.Create(logFileName))
            {
                byte[] title = new UTF8Encoding(true).GetBytes(_sb.ToString());
                fs.Write(title, 0, title.Length);
            }

            richTextBox1.Text = _sb.ToString();

            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();

            btnInsert.Enabled = true;
        }

        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
