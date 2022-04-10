using Newtonsoft.Json;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace FinancialTransactionsExcelToDb
{
    public partial class Form1 : Form
    {
        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel._Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();

        string _detailExcelFile;

        public Form1()
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
                fileName = Application.StartupPath + fileName;
            }

            _xlWorkbook = _xlApp.Workbooks.Open(fileName);
            _xlWorksheet = _xlWorkbook.Sheets[1];
            _xlRange = _xlWorksheet.UsedRange;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;


            string fileName = "detail of payment_96-97-98_98_07_07.xlsx";
            OpenFile("\\New folder\\" + fileName, isFromParlar: true);
            string sessionId = await Login("Amir", "AmirParlar");

            for (int row = 2; row < 200; row++)
            {
                AddFinancialDocumentInput input = new AddFinancialDocumentInput();
                var transfert = new AddFinancialDocumentTransferInput();

                #region Row
                var rowCell = _xlRange.Cells[row, 1];
                if (rowCell != null && rowCell.Value2 != null)
                {
                    var rowValue = (string)rowCell.Value2.ToString();
                    _sb.AppendLine("Row: " + rowValue);
                }
                #endregion

                #region Currency
                var currencyCell = _xlRange.Cells[row, 2];
                if (currencyCell != null && currencyCell.Value2 != null)
                {
                    var currencyValue = (string)currencyCell.Value2.ToString();

                    switch (currencyValue.Trim().ToLower())
                    {
                        case "rmb":
                            input.CurrencyId = 2;
                            break;

                        case "usd":
                            input.CurrencyId = 3;
                            break;

                        case "lir":
                            input.CurrencyId = 5;
                            break;

                        case "eur":
                            input.CurrencyId = 4;
                            break;
                    }

                    _sb.AppendLine("Currency: " + currencyValue);
                }
                #endregion

                #region Date
                var dateOfTransactionCell = _xlRange.Cells[row, 8];
                if (dateOfTransactionCell != null && dateOfTransactionCell.Value2 != null)
                {
                    var dateOfTransactionValue = (string)dateOfTransactionCell.Value2.ToString();
                    DateTime conv = ConvertPersianToGregorianCalendar(dateOfTransactionValue);
                    _sb.AppendLine("Date: " + conv.ToLongDateString());
                    input.Date = conv;
                }
                else
                {
                    //sb.AppendLine("------------------------------ERROR------------------------------");
                    _sb.AppendLine();
                    continue;
                }
                #endregion

                #region ToFinancialAccountId
                var beneficiaryCell = _xlRange.Cells[row, 7];
                if (beneficiaryCell != null && beneficiaryCell.Value2 != null)
                {
                    var beneficiaryValue = (string)beneficiaryCell.Value2.ToString().ToLower().Trim();
                    _sb.AppendLine("CooperatorName: " + beneficiaryValue);

                    input.CooperatorName = beneficiaryValue;
                }
                else
                {
                    //sb.AppendLine("------------------------------ERROR------------------------------");
                    _sb.AppendLine();
                    continue;
                }
                #endregion

                #region Amount
                var amountCell = _xlRange.Cells[row, 5];
                if (amountCell != null && amountCell.Value2 != null)
                {
                    var amountValue = (string)amountCell.Value2.ToString();
                    _sb.AppendLine("Amount: " + amountValue);

                    input.Amount = double.Parse(amountValue);
                }
                else
                {
                    //sb.AppendLine("------------------------------ERROR------------------------------");
                    _sb.AppendLine();
                    continue;
                }
                #endregion

                #region ToAmount
                var toAmountCell = _xlRange.Cells[row, 3];
                if (toAmountCell != null && toAmountCell.Value2 != null)
                {
                    var toAmountValue = (string)toAmountCell.Value2.ToString();
                    _sb.AppendLine("ToAmount: " + toAmountValue);

                    transfert.ToAmount = double.Parse(toAmountValue);
                }
                else
                {
                    //sb.AppendLine("------------------------------ERROR------------------------------");
                    _sb.AppendLine();
                    continue;
                }
                #endregion

                input.Type = FinancialDocumentType.Transfer;
                input.FinancialAccountId = 2; // from account پارلار پارلار
                input.FileKey = "869b7e2d-1ad1-4b75-bdd9-496bb91b1d9e";
                input.Description = "";
                input.IsFromParlar = true;

                input.FinancialDocumentTransfer = transfert;

                var message = await Post(sessionId, input);
                _sb.AppendLine(message);

                //var objectMessage = JsonConvert.DeserializeObject<ResultMessage>(message);
                //if (!objectMessage.Success)
                //{
                //    //xlRange.Cells[row, 13].Value = "ERROR";
                //    //xlRange = (Excel.Range)xlWorksheet.Cells[row, 13];
                //    //xlRange.Value = "ERROR";
                //}

                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            CloseFile();


            string logFileName = AppDomain.CurrentDomain.BaseDirectory + "\\New folder\\" + fileName.Split('.')[0] + "_" + GetTimestamp(DateTime.Now) + ".txt";

            using (FileStream fs = File.Create(logFileName))
            {
                byte[] title = new UTF8Encoding(true).GetBytes(_sb.ToString());
                fs.Write(title, 0, title.Length);
            }

            richTextBox1.Text = _sb.ToString();

            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();

            button1.Enabled = true;
        }



        private DateTime ConvertToDate(string excelDate)
        {
            double d = double.Parse(excelDate);
            DateTime conv = DateTime.FromOADate(d);
            return conv;
        }

        private async Task<string> Post(string sessionId, AddFinancialDocumentInput input)
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

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            int minCol = 1;

            int minRow = 2;
            int maxRow = 100;

            string fileName = _detailExcelFile;
            OpenFile(fileName, isFromParlar: false);
            string sessionId = await Login("Amir", "AmirParlar");

            for (int row = minRow; row <= maxRow; row++)
            {
                AddFinancialDocumentInput input = new AddFinancialDocumentInput();
                var transfert = new AddFinancialDocumentTransferInput();

                #region Row
                var rowCell = _xlRange.Cells[row, minCol];
                if (rowCell != null && rowCell.Value2 != null)
                {
                    var rowValue = (string)rowCell.Value2.ToString();
                    _sb.AppendLine("Row: " + rowValue);
                }
                #endregion

                #region ToFinancialAccountId
                var beneficiaryCell = _xlRange.Cells[row, minCol + 1];
                if (beneficiaryCell != null && beneficiaryCell.Value2 != null)
                {
                    var beneficiaryValue = (string)beneficiaryCell.Value2.ToString().ToLower().Trim();
                    _sb.AppendLine("Cooperator Name: " + beneficiaryValue);

                    input.CooperatorName = beneficiaryValue;
                }
                else
                {
                    //sb.AppendLine("------------------------------ERROR------------------------------");
                    _sb.AppendLine();
                    continue;
                }
                #endregion

                #region Amount
                var amountCell = _xlRange.Cells[row, minCol + 2];
                if (amountCell != null && amountCell.Value2 != null)
                {
                    var amountValue = (string)amountCell.Value2.ToString();
                    _sb.AppendLine("Amount: " + amountValue);
                    _sb.AppendLine("ToAmount: " + amountValue);

                    try
                    {
                        input.Amount = double.Parse(amountValue);
                        transfert.ToAmount = input.Amount;
                    }
                    catch(Exception ex)
                    {
                        _sb.AppendLine("------------------------------ERROR------------------------------");
                        _sb.AppendLine();
                        continue;
                    }
                }
                else
                {
                    //sb.AppendLine("------------------------------ERROR------------------------------");
                    _sb.AppendLine();
                    continue;
                }
                #endregion

                #region Currency
                var currencyCell = _xlRange.Cells[row, 4];
                if (currencyCell != null && currencyCell.Value2 != null)
                {
                    var currencyValue = (string)currencyCell.Value2.ToString();

                    switch (currencyValue.Trim().ToLower())
                    {
                        case "rmb":
                            input.CurrencyId = 2;
                            break;

                        case "usd":
                            input.CurrencyId = 3;
                            break;

                        case "lir":
                            input.CurrencyId = 5;
                            break;

                        case "eur":
                            input.CurrencyId = 4;
                            break;
                    }

                    _sb.AppendLine("Currency: " + currencyValue);
                }
                #endregion

                //input.FinancialAccountId = 951; // salmon 
                //input.FinancialAccountId = 947; // tony 0.13
                input.FinancialAccountId = 2736; // tony 1.12
                //input.FinancialAccountId = 1302; // janet 
                //input.FinancialAccountId = 950; // ray 

                input.Type = FinancialDocumentType.Transfer;
                input.FileKey = "869b7e2d-1ad1-4b75-bdd9-496bb91b1d9e";
                input.Description = "";
                input.IsFromParlar = false;

                string fileNameToGainDate = Path.GetFileNameWithoutExtension(fileName);
                string[] dateParts = fileNameToGainDate.Split('_');

                string year = dateParts[0];
                string month = dateParts[1];
                string day = dateParts[2];
                string persianDate = $"13{year}/{month}/{day}";

                input.Date = ConvertPersianToGregorianCalendar(persianDate);

                input.FinancialDocumentTransfer = transfert;

                var message = await Post(sessionId, input);
                _sb.AppendLine(message);
                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            CloseFile();

            string logFileName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_" + GetTimestamp(DateTime.Now) + ".txt";

            using (FileStream fs = File.Create(logFileName))
            {
                byte[] title = new UTF8Encoding(true).GetBytes(_sb.ToString().Trim());
                fs.Write(title, 0, title.Length);
            }

            richTextBox1.Text = _sb.ToString().Trim();

            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();

            button2.Enabled = true;
        }

        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
