using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Common
{
    public static class Common
    {
        //public static string Ip = "http://192.168.0.13:1111";
        //public static string Ip = "http://192.168.0.13:7000";
        public static string Ip = "http://localhost:3004";

        public static async Task<string> LoginUser(string username, string password)
        {
            string url = $"{Ip}/api/UserManagement/Login";
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

        public async static Task<string> Post(string token, string requestUri, object input)
        {
            Uri baseAddress = new Uri(Ip);
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("session-id", token));
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress, Timeout = TimeSpan.FromHours(1) };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonContent = JsonConvert.SerializeObject(input);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, content);
            var resultJson = await response.Content.ReadAsStringAsync();
            return resultJson;
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
