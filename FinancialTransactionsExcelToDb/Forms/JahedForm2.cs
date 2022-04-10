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
    public partial class JahedForm2 : Form
    {
        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel._Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();

        string _detailExcelFile;

        public JahedForm2()
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

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;

            string sessionId = await Login("Amir", "AmirParlar");


            string[] filePaths = Directory.GetFiles(@"C:\Users\Amir\source\repos\FinancialTransactionsExcelToDb\FinancialTransactionsExcelToDb\bin\Debug\Newfolder");

            for (var i = 0; i < 1; i++)
            {
                var filePath = filePaths[i];
                string extenstion = Path.GetExtension(filePath);
                if (extenstion != ".xlsx") continue;

                //var fileName2 = Path.GetFileName(filePath);
                //string cfaCode = GetCfaCode(fileName2);


                //string fileName = detailExcelFile;
                OpenFile(filePath, isFromParlar: false);

                int maxCol = 50;
                int maxRow = 1000;

                //var firstCell = GetFirstCellLocation(maxRow, maxCol);
                int startCol = 1;
                int startRow = 2;

                for (int row = startRow + 1; row <= maxRow; row++)
                {
                    AddFinancialDocumentInput2 input = new AddFinancialDocumentInput2();

                    #region Row

                    var rowCell = _xlRange.Cells[row, startCol];
                    if (rowCell != null && rowCell.Value2 != null)
                    {
                        var rowValue = (string) rowCell.Value2.ToString();
                        _sb.AppendLine("Row: " + rowValue);
                    }

                    #endregion

                    #region Date

                    var date = _xlRange.Cells[row, startCol + 1];
                    if (date != null && date.Value2 != null)
                    {
                        var persianDate = (string) date.Value2.ToString();

                        //string[] dateParts = dateValue.Split('/');
                        //string year = dateParts[0];
                        //string month = dateParts[1];
                        //string day = dateParts[2];
                        //string persianDate = $"13{year}/{month}/{day}";

                        _sb.AppendLine("Date: " + persianDate);

                        try
                        {
                            input.DocumentDate = ConvertPersianToGregorianCalendar(persianDate);
                        }
                        catch (Exception ex)
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

                    #region Amount

                    var amountCell = _xlRange.Cells[row, startCol + 2];
                    if (amountCell != null && amountCell.Value2 != null)
                    {
                        var amountValue = (string) amountCell.Value2.ToString();
                        _sb.AppendLine("Amount: " + amountValue);

                        try
                        {
                            input.DebitAmount = double.Parse(amountValue);
                        }
                        catch (Exception ex)
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

                    #region Row

                    int financialAccountId = 0;
                    var faCodeCell = _xlRange.Cells[row, startCol + 3];
                    if (faCodeCell != null && faCodeCell.Value2 != null)
                    {
                        var faCodeValue = (string) faCodeCell.Value2.ToString();
                        _sb.AppendLine("FinancialAccountCode: " + faCodeValue);


                        financialAccountId = await GetFinancialAccountId(sessionId, faCodeValue);
                        if (financialAccountId == 0) continue;
                    }

                    #endregion

                    //input.FinancialAccountId = 951; // salmon 
                    //input.FinancialAccountId = 947; // tony 0.13
                    //input.FinancialAccountId = 2736; // tony 1.12
                    //input.FinancialAccountId = 1302; // janet 
                    //input.FinancialAccountId = 950; // ray 
                    //input.FinancialAccountId = 546;

                    input.FinancialAccountId = financialAccountId;

                    input.Type = FinancialDocumentType.Deposit;
                    input.FileKey = "cb097466-8435-4779-be36-e227faa95cc2";
                    input.Description = "طبق فایل اکسل پیوست نامه شماره 203939 در اتوماسیون";

                    //string fileNameToGainDate = Path.GetFileNameWithoutExtension(fileName);

                    if (input.DebitAmount > 0)
                    {
                        var message = await Post(sessionId, input);
                        _sb.AppendLine(message);
                        _sb.AppendLine("\n");

                        richTextBox1.Text = _sb.ToString();

                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();
                    }

                    _sb.AppendLine("-----------------------------------------------------------------");
                }

                CloseFile();

                string logFileName = Path.GetDirectoryName(filePath) + "\\" +
                                     Path.GetFileNameWithoutExtension(filePath) + "_" + GetTimestamp(DateTime.Now) +
                                     ".txt";

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
