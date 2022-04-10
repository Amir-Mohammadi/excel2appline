using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class SaveStuffBasePrice : Form
    {
        string token = "";

        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel._Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();

        public SaveStuffBasePrice()
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

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            string fileName = "StuffBasePrice_98_07_09.xls";
            fileName = Application.StartupPath + "\\New folder\\" + fileName;

            OpenFile(fileName);
            string sessionId = await Login("Amir", "AmirParlar");

            for (int row = 10; row < 1000; row++)
            {
                //if (row == 10)
                //{
                //    break;
                //}

                GetStuffsInput getStuffsInput =
                    new GetStuffsInput(pagingInput: null, sortType: Models.StuffSortType.Code, sortOrder: System.Data.SqlClient.SortOrder.Ascending);

                #region StuffCode
                var stuffCodeCell = _xlRange.Cells[row, 1];
                if (stuffCodeCell != null && stuffCodeCell.Value2 != null)
                {
                    var stuffCodeValue = (string)stuffCodeCell.Value2.ToString();
                    _sb.AppendLine("StuffCode: " + stuffCodeValue);
                    getStuffsInput.Code = stuffCodeValue;
                }
                #endregion

                int stuffId = await GetStuffId(sessionId, getStuffsInput);
                richTextBox1.Text = _sb.ToString();

                AddConstantStuffBasePriceInput addConstantStuffBasePriceInput = new AddConstantStuffBasePriceInput();
                addConstantStuffBasePriceInput.StuffIds = new int[1];
                addConstantStuffBasePriceInput.StuffIds[0] = stuffId;


                #region Price
                var priceCell = _xlRange.Cells[row, 4];
                if (priceCell != null && priceCell.Value2 != null)
                {
                    var priceValue = (string)priceCell.Value2.ToString();
                    _sb.AppendLine("Price: " + priceValue);
                    addConstantStuffBasePriceInput.Price = double.Parse(priceValue);
                }
                #endregion

                #region Currency
                var currencyCell = _xlRange.Cells[row, 5];
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

                string addResult = await AddConstantStuffsBasePrice(sessionId, addConstantStuffBasePriceInput);
                _sb.AppendLine(addResult);

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

            button1.Enabled = true;
        }

        public string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
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

        private async Task<string> AddConstantStuffsBasePrice(string sessionId, AddConstantStuffBasePriceInput input)
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
            var response = await client.PostAsync("api/Supplies/AddConstantStuffsBasePrice", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return jsonResult;
        }

        private async Task<int> GetStuffId(string sessionId, GetStuffsInput input)
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
            var response = await client.PostAsync("api/SaleManagement/GetStuffs", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(jsonResult);

            return result.Data[0].Id;
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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            token = await Common.Common.LoginUser("Machine", "123456");

            btnLogin.Enabled = true;
        }
    }
}
