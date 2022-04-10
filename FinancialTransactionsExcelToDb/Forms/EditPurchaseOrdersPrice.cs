using FinancialTransactionsExcelToDb.Models;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace FinancialTransactionsExcelToDb
{
    public partial class EditPurchaseOrdersPrice : Form
    {
        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel._Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();

        string _detailExcelFile;

        public EditPurchaseOrdersPrice()
        {
            InitializeComponent();

            AllowDrop = true;
            DragEnter += new DragEventHandler(Form1_DragEnter);
            DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            _detailExcelFile = files[0];
            label1.Text = _detailExcelFile;
        }

        private void OpenFile(string fileName, bool isFromParlar)
        {
            _xlApp = new Excel.Application();

            if (isFromParlar)
            {
                fileName = System.Windows.Forms.Application.StartupPath + fileName;
            }

            _xlWorkbook = _xlApp.Workbooks.Open(fileName);
            _xlWorksheet = _xlWorkbook.Sheets[1];
            _xlRange = _xlWorksheet.UsedRange;
        }

        private DateTime ConvertToDate(string excelDate)
        {
            double d = double.Parse(excelDate);
            DateTime conv = DateTime.FromOADate(d);
            return conv;
        }

        private async Task<string> Post(string sessionId, EditPurchaseOrderInput input)
        {
            Uri baseAddress = new Uri("http://localhost:3004/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", sessionId));
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Accounting/EditPurchaseOrder", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return jsonResult;
        }

        private async Task<string> Post(string sessionId, AddFinancialDocumentInput2 input)
        {
            Uri baseAddress = new Uri("http://localhost:3004/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", sessionId));
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Accounting/AddFinancialDocument", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return jsonResult;
        }

        private async Task<int> GetFinancialAccountId(string sessionId, string financialAccountCode)
        {
            GetFinancialAccountSummaryInput input = new GetFinancialAccountSummaryInput
            {
                FinancialAccountCode = financialAccountCode,
            };

            Uri baseAddress = new Uri("http://localhost:3004/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", sessionId));
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Accounting/GetFinancialAccountSummary", content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(jsonResult);
            try
            {
                return obj.Data[0].Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private async Task<string> Login(string username, string password)
        {
            string url = "http://localhost:3004/api/UserManagement/Login";
            Uri address = new Uri(url);
            var login = new LoginInput
            {
                UserName = username,
                Password = password
            };

            string jsonContent = JsonConvert.SerializeObject(login);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var cookieJar = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler)
            {
                BaseAddress = address
            };

            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();

            Uri uri = new Uri(url);
            var responseCookies = cookieJar.GetCookies(uri);

            string sessionId = "";
            foreach (Cookie cookie in responseCookies)
            {
                string cookieName = cookie.Name;
                string cookieValue = cookie.Value;

                if (cookieName == "session-id")
                {
                    sessionId = cookieValue;
                    break;
                }
            }

            return sessionId;
        }

        private DateTime ConvertPersianToGregorianCalendar(string persianCal)
        {
            string[] userDateParts = persianCal.Split(new[] { "/" }, StringSplitOptions.None);

            if (userDateParts[0].Length < 4)
                userDateParts[0] = "13" + userDateParts[0];

            int userYear = int.Parse(userDateParts[0]);
            int userMonth = int.Parse(userDateParts[1]);
            int userDay = int.Parse(userDateParts[2]);


            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(userYear, userMonth, userDay, pc);

            return dt;
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

        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private CellLocation GetFirstCellLocation(int maxRow, int maxCol)
        {
            for (int i = 1; i < maxRow; i++)
            {
                for (int j = 1; j < maxCol; j++)
                {
                    var rowCell = _xlRange.Cells[i, j];
                    if (rowCell != null && rowCell.Value2 != null)
                    {
                        return new CellLocation
                        {
                            Row = i,
                            Column = j
                        };
                    }
                }
            }

            return null;
        }

        private string GetCfaCode(string fileName)
        {
            var splittedBySpace = fileName.Split(' ');

            foreach (var item in splittedBySpace)
            {
                if (item.Contains("cfa"))
                {
                    var splittedByDot = item.Split('.');

                    foreach (var item2 in splittedByDot)
                    {
                        if (item2.Contains("cfa"))
                        {
                            return item2;
                        }
                    }
                }
            }

            return "";
        }

        private async Task<PurchaseOrderResult> GetPurchaseOrder(string sessionId, string purchaseOrderCode)
        {
            GetPurchaseOrderInput input = new GetPurchaseOrderInput
            {
                Code = purchaseOrderCode,
            };

            Uri baseAddress = new Uri("http://localhost:3004/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", sessionId));
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Supplies/GetPurchaseOrder", content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            var purchaseOrderResult = JsonConvert.DeserializeObject<Result<PurchaseOrderResult>>(jsonResult);
            var result = purchaseOrderResult?.Data;

            return result;
        }

        private async Task<Result<PurchaseOrderResult>> EditPurchaseOrder(string sessionId, EditPurchaseOrderInput input)
        {
            Uri baseAddress = new Uri("http://localhost:3004/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", sessionId));
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Supplies/EditPurchaseOrder", content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            var purchaseOrderResult = JsonConvert.DeserializeObject<Result<PurchaseOrderResult>>(jsonResult);

            return purchaseOrderResult;
        }

        private async void button3_Click(object sender, EventArgs e)
        {

            button3.Enabled = false;

            string sessionId = await Login("Amir", "AmirParlar");


            string[] filePaths = Directory.GetFiles(@"C:\Users\Amir\source\repos\FinancialTransactionsExcelToDb\FinancialTransactionsExcelToDb\bin\Debug\china_98_10_11");


            foreach (var filePath in filePaths)
            {
                //var filePath = @"‪C:\Users\Amir\source\repos\FinancialTransactionsExcelToDb\FinancialTransactionsExcelToDb\bin\Debug\china_98_10_11\riali chin_98_10_11.xlsx";

                string fileName = Path.GetFileName(filePath);
                if (fileName != "riali chin_98_10_11.xlsx") continue;

                //string extenstion = Path.GetExtension(filePath);
                //if (extenstion != ".xlsx") continue;

                //string fileName = detailExcelFile;
                OpenFile(filePath, isFromParlar: false);


                int maxCol = 20;
                int maxRow = 1200;

                //var firstCell = GetFirstCellLocation(maxRow, maxCol);
                //int startCol = firstCell.Column;
                //int startRow = firstCell.Row;

                int startCol = 1;
                int startRow = 1;


                for (int row = startRow + 1; row <= maxRow; row++)
                {
                    #region GetPurchaseOrder
                    
                    #region Row
                    _sb.AppendLine("Row: " + row);
                    #endregion

                    #region PurchaseOrderCode
                    var purchaseOrderCell = _xlRange.Cells[row, startCol + 11];
                    string purchaseOrderValue = string.Empty;
                    if (purchaseOrderCell != null && purchaseOrderCell.Value2 != null)
                    {
                        purchaseOrderValue = (string)purchaseOrderCell.Value2.ToString();
                        _sb.AppendLine("PurchaseOrderCode: " + purchaseOrderValue);
                    }
                    #endregion

                    PurchaseOrderResult purchaseOrder = await GetPurchaseOrder(
                        sessionId: sessionId,
                        purchaseOrderCode: purchaseOrderValue);

                    if (purchaseOrder == null) continue;

                    #endregion

                    #region New Price
                    var newPriceCell = _xlRange.Cells[row, startCol + 1];
                    string newPriceValue = string.Empty;
                    if (newPriceCell != null && newPriceCell.Value2 != null)
                    {
                        newPriceValue = (string)newPriceCell.Value2.ToString();
                        _sb.AppendLine("New Price: " + newPriceValue);
                    }
                    double dblNewPriceValue = double.Parse(newPriceValue);
                    #endregion

                    #region New Currency
                    var newCurrencyCell = _xlRange.Cells[row, startCol + 0];
                    string newCurrencyValue = string.Empty;
                    if (newCurrencyCell != null && newCurrencyCell.Value2 != null)
                    {
                        newCurrencyValue = (string)newCurrencyCell.Value2.ToString();
                        _sb.AppendLine("New Currency: " + newCurrencyValue);
                    }

                    int newCurrencyId = 0;
                    switch (newCurrencyValue)
                    {
                        case "دلار":
                            newCurrencyId = 3;
                            break;

                        case "لیر":
                            newCurrencyId = 5;
                            break;

                        case "یوان":
                            newCurrencyId = 2;
                            break;
                    }
                    #endregion

                    Result<PurchaseOrderResult> result = new Result<PurchaseOrderResult>();
                    if (purchaseOrder == null)
                    {
                        result.Success = false;
                        result.Message = "سفارش با کد مورد نظر موجود نیست.";
                    }
                    else
                    {
                        if (newCurrencyId != purchaseOrder.CurrencuyId || dblNewPriceValue != purchaseOrder.Price)
                        {
                            EditPurchaseOrderInput editPurchaseOrderInput = new EditPurchaseOrderInput
                            {
                                AddPurchaseOrderFinancings = new AddPurchaseOrderFinancingInput[] { },
                                DeletePurchaseOrderFinancings = new DeletePurchaseOrderFinancingInput[] { },
                                NewAddedPurchaseOrders = new AddPurchaseOrderInput[] { },
                                PurchaseOrderDetail = new PurchaseOrderDetailInput[] { },

                                Id = purchaseOrder.Id,
                                RowVersion = purchaseOrder.RowVersion,
                                BuyDeadline = purchaseOrder.Deadline,
                                Price = dblNewPriceValue,
                                CurrencyId = newCurrencyId,
                                ProviderId = purchaseOrder.ProviderId,
                                Qty = purchaseOrder.Qty,
                                SupplierId = purchaseOrder.SupplierId,
                                UnitId = purchaseOrder.UnitId,
                                PurchaseOrderType = purchaseOrder.PurchaseOrderType
                            };
                            result = await EditPurchaseOrder(sessionId: sessionId, input: editPurchaseOrderInput);
                        }
                        else
                        {
                            result.Success = true;
                            result.Message = "Done Before.";
                        }
                    }

                    _sb.AppendLine("Success: " + result.Success);
                    _sb.AppendLine("Message: " + result.Message);
                    _sb.AppendLine("------------------------------------------------------------");


                    //string cellValue = string.Empty;
                    //if (result.Success)
                    //    cellValue = "OK";
                    //else
                    //    cellValue = "X";

                    //var cell = (Range)xlWorksheet.Cells[row, 15];
                    //cell.Value2 = cellValue;

                    //#region Row
                    //var rowCell = xlRange.Cells[row, startCol];
                    //if (rowCell != null && rowCell.Value2 != null)
                    //{
                    //    var rowValue = (string)rowCell.Value2.ToString();
                    //    sb.AppendLine("Row: " + rowValue);
                    //}
                    //#endregion

                    //#region Date
                    //var date = xlRange.Cells[row, startCol + 1];
                    //if (date != null && date.Value2 != null)
                    //{
                    //    var persianDate = (string)date.Value2.ToString();

                    //    //string[] dateParts = dateValue.Split('/');
                    //    //string year = dateParts[0];
                    //    //string month = dateParts[1];
                    //    //string day = dateParts[2];
                    //    //string persianDate = $"13{year}/{month}/{day}";

                    //    sb.AppendLine("Date: " + date);

                    //    try
                    //    {
                    //        //input.DocumentDate = ConvertPersianToGregorianCalendar(persianDate);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        sb.AppendLine("------------------------------ERROR------------------------------");
                    //        sb.AppendLine();
                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                    //    //sb.AppendLine("------------------------------ERROR------------------------------");
                    //    sb.AppendLine();
                    //    continue;
                    //}
                    //#endregion

                    //#region Amount
                    //var amountCell = xlRange.Cells[row, startCol + 2];
                    //if (amountCell != null && amountCell.Value2 != null)
                    //{
                    //    var amountValue = (string)amountCell.Value2.ToString();
                    //    sb.AppendLine("Amount: " + amountValue);

                    //    try
                    //    {
                    //        //input.DebitAmount = double.Parse(amountValue);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        sb.AppendLine("------------------------------ERROR------------------------------");
                    //        sb.AppendLine();
                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                    //    //sb.AppendLine("------------------------------ERROR------------------------------");
                    //    sb.AppendLine();
                    //    continue;
                    //}
                    //#endregion

                    //input.FinancialAccountId = 951; // salmon 
                    //input.FinancialAccountId = 947; // tony 0.13
                    //input.FinancialAccountId = 2736; // tony 1.12
                    //input.FinancialAccountId = 1302; // janet 
                    //input.FinancialAccountId = 950; // ray 

                    //input.FinancialAccountId = 546;

                    //input.Type = FinancialDocumentType.Deposit;
                    //input.FileKey = "c59b3b29-9039-40be-91dc-9c608d19e932";
                    //input.Description = "";

                    ////string fileNameToGainDate = Path.GetFileNameWithoutExtension(fileName);

                    //if (input.DebitAmount > 0)
                    //{
                    //    var message = await Post(sessionId, input);
                    //    sb.AppendLine(message);
                    //    sb.AppendLine("\n");

                    richTextBox1.Text = _sb.ToString();

                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    //}


                    //Thread.Sleep(3000);
                }

                CloseFile();

                string logFileName = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_" + GetTimestamp(DateTime.Now) + ".txt";

                using (FileStream fs = File.Create(logFileName))
                {
                    byte[] title = new UTF8Encoding(true).GetBytes(_sb.ToString().Trim());
                    fs.Write(title, 0, title.Length);
                }

                richTextBox1.Text = _sb.ToString().Trim();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            button3.Enabled = true;
        }
    }
}
