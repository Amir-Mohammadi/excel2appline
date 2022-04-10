using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace FinancialTransactionsExcelToDb.Forms
{
    public partial class AddProduction : Form
    {
        public AddProduction()
        {
            InitializeComponent();
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
            string jsonResult = await response.Content.ReadAsStringAsync();

            var loginResult = JsonConvert.DeserializeObject<Result<LoginResult>>(jsonResult);
            return loginResult.Data.Token;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            //string fileName = "detail of payment_96-97-98_98_07_07.xlsx";
            //OpenFile("\\New folder\\" + fileName);

            string token = await Login("Amir", "AmirParlar");

            GetStuffSerialInput input = new GetStuffSerialInput
            {
                Serial = "dlfk60310002066859",
            };

            Uri baseAddress = new Uri("http://localhost:3004/");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", token));
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress };

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/WarehouseManagement/GetStuffSerial", content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(jsonResult);
           
            btnStart.Enabled = true;
        }

        private void AddProductionForSerials(string[] serials)
        {

        }
    }
}
