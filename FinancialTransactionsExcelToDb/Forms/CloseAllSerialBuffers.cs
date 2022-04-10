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
    public partial class CloseAllSerialBuffers : Form
    {
        string token = "";
        ResultList<SerialBufferResult> serialBufferResults;

        public CloseAllSerialBuffers()
        {
            InitializeComponent();
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

            GetSerialBuffersInput input = new GetSerialBuffersInput(null, SerialBufferSortType.CreationTime, System.Data.SqlClient.SortOrder.Ascending)
            {
                AdvanceSearchItems = new AdvanceSearchItem[] { }
            };
            var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/GetSerialBuffers", input);
            serialBufferResults = JsonConvert.DeserializeObject<ResultList<SerialBufferResult>>(json);

            btnGetIds.Enabled = true;
        }

        private async void btnAddRialInvoice_Click(object sender, EventArgs e)
        {
            btnAddRialInvoice.Enabled = false;

            var counter = 0;
            foreach (var serialBuffer in serialBufferResults.Data)
            {

                CloseSerialBufferInput input = new CloseSerialBufferInput
                {
                    Serial = serialBuffer.Serial,
                    WarehouseId = serialBuffer.WarehouseId
                };
                var json = await Common.Common.Post(token: token, requestUri: "api/WarehouseManagement/CloseSerialBuffer", input);
                var result = JsonConvert.DeserializeObject<Result>(json);


                counter++;
                richTextBox1.AppendText($"\nCounter: {counter} of {serialBufferResults.Data.Count} - {DateTime.Now} - Serial: {serialBuffer.Serial} - Result: {json}");

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnAddRialInvoice.Enabled = true;
        }
    }
}
