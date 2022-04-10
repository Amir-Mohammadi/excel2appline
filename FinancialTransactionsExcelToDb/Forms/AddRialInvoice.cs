using FinancialTransactionsExcelToDb.Common;
using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialTransactionsExcelToDb.Forms
{
    public partial class AddRialInvoice : Form
    {
        string token = "";
        IEnumerable<int> receiptIds;

        public AddRialInvoice()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            token = await Common.Common.LoginUser("Machine", "MachineParlar");

            btnLogin.Enabled = true;
        }

        private async void btnGetIds_Click(object sender, EventArgs e)
        {
            btnGetIds.Enabled = false;

            GetReceiptsInput input = new GetReceiptsInput(null, ReceiptSortType.Id, System.Data.SqlClient.SortOrder.Ascending)
            {
                //FromDate = new DateTime(2017, 3, 21),
                //ToDate = new DateTime(2018, 3, 21),
                //HasRialInvoice = false,
                AdvanceSearchItems = new AdvanceSearchItem[] { }
            };
            //var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/GetReceipts", input);
            //var result = JsonConvert.DeserializeObject<ResultList<ReceiptResult>>(json);

            //receiptIds = result.Data.Select(r => r.Id);

            receiptIds = new int[]
            {
            };

            btnGetIds.Enabled = true;
        }

        private async void btnAddRialInvoice_Click(object sender, EventArgs e)
        {
            btnAddRialInvoice.Enabled = false;

            var counter = 0;
            foreach (var receiptId in receiptIds)
            {

                AddRialInvoiceInput input = new AddRialInvoiceInput
                {
                    ReceiptId = receiptId
                };
                var json = await Common.Common.Post(token: token, requestUri: "api/Accounting/AddRialInvoice", input);
                var result = JsonConvert.DeserializeObject<Result>(json);


                counter++;
                richTextBox1.AppendText($"\nCounter: {counter} of {receiptIds.Count()} - {DateTime.Now} - ReceiptId: {receiptId} - Result: {json}");

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnAddRialInvoice.Enabled = true;
        }
    }
}
