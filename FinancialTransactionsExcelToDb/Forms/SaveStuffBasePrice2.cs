using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace FinancialTransactionsExcelToDb.Forms
{
    public partial class SaveStuffBasePrice2 : Form
    {
        private string token;
        string fileName = "BasicPrice_99_10_27.xlsx";

        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel._Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();

        public SaveStuffBasePrice2()
        {
            InitializeComponent();
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


        private async Task<int> GetStuffId(GetStuffsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/SaleManagement/GetStuffs", input);
            var result = JsonConvert.DeserializeObject<ResultList<StuffResult>>(json);

            return result.Data[0].Id;
        }

        private async Task<Result> AddConstantStuffsBasePrice(AddConstantStuffBasePriceInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/Supplies/AddConstantStuffsBasePrice", input);
            var result = JsonConvert.DeserializeObject<Result>(json);

            return result;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            token = await Common.Common.LoginUser("Machine", "sghlsghl");

            btnLogin.Enabled = true;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            btnLoadFile.Enabled = false;

            fileName = Application.StartupPath + "\\New folder\\" + fileName;

            OpenFile(fileName);

            btnLoadFile.Enabled = true;
        }

        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }


        private async void btnInsert_Click(object sender, EventArgs e)
        {
            btnInsert.Enabled = false;

            for (int row = 2; row < 543; row++)
            {
                //if (row == 10) break;

                _sb.AppendLine("Row: " + row);

                GetStuffsInput getStuffsInput = 
                    new GetStuffsInput(pagingInput: null, sortType: StuffSortType.Code, sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                getStuffsInput.AdvanceSearchItems = new AdvanceSearchItem[0];

                #region StuffCode
                var stuffCodeCell = _xlRange.Cells[row, 1];
                if (stuffCodeCell != null && stuffCodeCell.Value2 != null)
                {
                    var stuffCodeValue = (string)stuffCodeCell.Value2.ToString().PadLeft(4, '0');
                    _sb.AppendLine("StuffCode: " + stuffCodeValue);
                    getStuffsInput.Code = stuffCodeValue;
                }
                #endregion

                int stuffId = await GetStuffId(getStuffsInput);
                richTextBox1.Text = _sb.ToString();

                AddConstantStuffBasePriceInput addConstantStuffBasePriceInput = new AddConstantStuffBasePriceInput();
                addConstantStuffBasePriceInput.StuffIds = new int[1];
                addConstantStuffBasePriceInput.StuffIds[0] = stuffId;

                #region Price
                var priceCell = _xlRange.Cells[row, 3];
                if (priceCell != null && priceCell.Value2 != null)
                {
                    var priceValue = (string)priceCell.Value2.ToString();
                    _sb.AppendLine("Price: " + priceValue);
                    addConstantStuffBasePriceInput.Price = double.Parse(priceValue);
                }
                #endregion

                #region Currency
                var currencyCell = _xlRange.Cells[row, 4];
                if (currencyCell != null && currencyCell.Value2 != null)
                {
                    var currencyNameValue = (string)currencyCell.Value2.ToString();
                    _sb.AppendLine("Currency: " + currencyNameValue);

                    switch (currencyNameValue)
                    {
                        case "ریال":
                            addConstantStuffBasePriceInput.CurrencyId = 1;
                            break;

                        case "یوان":
                            addConstantStuffBasePriceInput.CurrencyId = 2;
                            break;

                        case "دلار":
                            addConstantStuffBasePriceInput.CurrencyId = 3;
                            break;

                        case "لیر":
                            addConstantStuffBasePriceInput.CurrencyId = 5;
                            break;
                    }
                }
                #endregion

                var addResult = await AddConstantStuffsBasePrice(addConstantStuffBasePriceInput);
                var addResultJson = JsonConvert.SerializeObject(addResult);
                _sb.AppendLine(addResultJson);

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

        private void SaveStuffBasePrice2_Load(object sender, EventArgs e)
        {

        }
    }
}
