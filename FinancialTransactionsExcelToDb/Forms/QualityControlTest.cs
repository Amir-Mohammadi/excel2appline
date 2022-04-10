using FinancialTransactionsExcelToDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace FinancialTransactionsExcelToDb.Forms
{
    public partial class QualityControlTest : Form
    {
        private string token;
        string fileName = "1400-02-18.xlsx";

        Excel.Application _xlApp;
        Excel.Range _xlRange;
        Excel.Workbook _xlWorkbook;
        Excel.Worksheet _xlWorksheet;
        StringBuilder _sb = new StringBuilder();
        List<QualityControlTestResult> qualityControlTestResults = new List<QualityControlTestResult>();



        public QualityControlTest()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            token = await Common.Common.LoginUser("Machine", "MachineParlar");

            btnLogin.Enabled = true;
        }

        private async Task<TestConditionResult> GetTestConditions(GetTestConditionsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/GetTestConditions", input);
            var result = JsonConvert.DeserializeObject<ResultList<TestConditionResult>>(json);
            if (result.Data.Count == 0) return null;

            return result.Data[0];
        }

        private async Task<QualityControlTestResult> GetQualityControlTest(GetQualityControlTestsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/GetQualityControlTests", input);
            var result = JsonConvert.DeserializeObject<ResultList<QualityControlTestResult>>(json);
            if (result.Data.Count == 0) return null;

            return result.Data[0];
        }

        private async Task<StuffResult> GetStuff(GetStuffsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/SaleManagement/GetStuffs", input);
            var result = JsonConvert.DeserializeObject<ResultList<StuffResult>>(json);
            if (result.Data.Count == 0) return null;

            return result.Data[0];
        }

        private async Task<QualityControlTestUnitResult> GetQualityControlTestUnit(GetQualityControlTestUnitsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/GetQualityControlTestUnits", input);
            var result = JsonConvert.DeserializeObject<ResultList<QualityControlTestUnitResult>>(json);
            if (result.Data.Count == 0) return null;

            return result.Data[0];
        }

        private void btnLoadFile_Click_1(object sender, EventArgs e)
        {
            btnLoadFile.Enabled = false;

            fileName = Application.StartupPath + "\\New folder\\" + fileName;

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
            if (_xlRange == null) return;

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


            for (int row = 2; row < 550; row++)
            {
                _sb.AppendLine("Row: " + row);

                string qualityControlTestDescriptionValue = null;

                var qualityControlTestDescriptionCell = _xlRange.Cells[row, 3];
                if (qualityControlTestDescriptionCell != null && qualityControlTestDescriptionCell.Value2 != null)
                {
                    qualityControlTestDescriptionValue = (string)qualityControlTestDescriptionCell.Value2.ToString();
                    _sb.AppendLine("qualityControlTestDescription: " + qualityControlTestDescriptionValue);
                }


                var lineCell = _xlRange.Cells[row, 4];
                if (lineCell != null && lineCell.Value2 != null)
                {
                    var lineValue = (string)lineCell.Value2.ToString();
                    _sb.AppendLine("Line: " + lineValue);

                    GetQualityControlTestsInput getQualityControlTestsInput = new GetQualityControlTestsInput(pagingInput: null, sortType: QualityControlTestSortType.Name, sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getQualityControlTestsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getQualityControlTestsInput.Name = lineValue;

                    var qualityControlTest = await GetQualityControlTest(getQualityControlTestsInput);

                    #region AddMode
                    if (qualityControlTest == null)
                    {
                        AddQualityControlTestInput addQualityControlTestInput = new AddQualityControlTestInput();
                        addQualityControlTestInput.Code = "QCT" + row;
                        addQualityControlTestInput.Name = lineValue;
                        addQualityControlTestInput.Description = qualityControlTestDescriptionValue;

                        var addResult = await AddQualityControlTest(addQualityControlTestInput);
                        var addResultJson = JsonConvert.SerializeObject(addResult);
                        _sb.AppendLine("AddResult: " + addResultJson);
                    }
                    #endregion
                    #region EditMode
                    else
                    {
                        if (qualityControlTestDescriptionValue != null)
                        {
                            EditQualityControlTestInput editQualityControlTestInput = new EditQualityControlTestInput();

                            editQualityControlTestInput.Id = qualityControlTest.Id;
                            editQualityControlTestInput.Name = qualityControlTest.Name;
                            editQualityControlTestInput.RowVersion = qualityControlTest.RowVersion;
                            editQualityControlTestInput.Description = qualityControlTestDescriptionValue;
                            editQualityControlTestInput.Code = qualityControlTest.Code;
                            editQualityControlTestInput.AddTestConditions = new AddTestConditionInput[0];
                            editQualityControlTestInput.DeleteTestConditions = new DeleteTestConditionInput[0];

                            var editResult = await EditQualityControlTest(editQualityControlTestInput);
                            var editResultJson = JsonConvert.SerializeObject(editResult);
                            _sb.AppendLine("EditResult: " + editResultJson);
                        }
                    }
                    #endregion
                }


                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }


            btnInsert.Enabled = true;
        }

        public async Task<Result> SaveStuffQualityControlTests(SaveStuffQualityControlTestsInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/SaveStuffQualityControlTests", input);
            var result = JsonConvert.DeserializeObject<Result>(json);

            return result;
        }

        public async Task<Result> AddTestCondition(AddTestConditionInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/AddTestCondition", input);
            var result = JsonConvert.DeserializeObject<Result>(json);

            return result;
        }

        public async Task<Result> AddQualityControlTest(AddQualityControlTestInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/AddQualityControlTest", input);
            var result = JsonConvert.DeserializeObject<Result>(json);

            return result;
        }

        public async Task<Result> EditQualityControlTest(EditQualityControlTestInput input)
        {
            var json = await Common.Common.Post(token: token, requestUri: "api/QualityControl/EditQualityControlTest", input);
            var result = JsonConvert.DeserializeObject<Result>(json);

            return result;
        }

       

        private async void btnInsert2_Click(object sender, EventArgs e)
        {
            btnInsert2.Enabled = false;

            #region Insert to list
            for (int row = 2; row < 550; row++)
            {
                _sb.AppendLine("Row: " + row);

                QualityControlTestResult qualityControlTestResult = new QualityControlTestResult();
                TestConditionResult testConditionResult = new TestConditionResult();

                #region Line
                var lineCell = _xlRange.Cells[row, 4];
                if (lineCell != null && lineCell.Value2 != null)
                {
                    var lineValue = (string)lineCell.Value2.ToString();
                    _sb.AppendLine("Line: " + lineValue);

                    GetQualityControlTestsInput getQualityControlTestsInput = new GetQualityControlTestsInput(pagingInput: null, sortType: QualityControlTestSortType.Name, sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getQualityControlTestsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getQualityControlTestsInput.Name = lineValue;

                    qualityControlTestResult = await GetQualityControlTest(getQualityControlTestsInput);
                }
                #endregion

                #region TestCondition
                var testCondictionCell = _xlRange.Cells[row, 5];
                if (testCondictionCell != null && testCondictionCell.Value2 != null)
                {
                    var testCondictionValue = (string)testCondictionCell.Value2.ToString();
                    _sb.AppendLine("Condition: " + testCondictionValue);

                    GetTestConditionsInput getTestConditionsInput = new GetTestConditionsInput(pagingInput: null, sortType: TestConditionSortType.Id, sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getTestConditionsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getTestConditionsInput.Condition = testCondictionValue;

                    testConditionResult = await GetTestConditions(getTestConditionsInput);
                }
                #endregion

                if (testConditionResult == null || qualityControlTestResult == null) continue;

                qualityControlTestResult.TestConditionId = testConditionResult.Id;
                qualityControlTestResult.TestConditionCondition = testConditionResult.Condition;

                qualityControlTestResults.Add(qualityControlTestResult);

                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }
            #endregion

            #region Insert from list
            qualityControlTestResults = qualityControlTestResults.GroupBy(qct =>
               new
               {
                   qct.Code,
                   qct.Description,
                   qct.Id,
                   qct.Name,
                   qct.TestConditionCondition,
                   qct.TestConditionId
               })
               .Select(grp => new QualityControlTestResult
               {
                   Code = grp.Key.Code,
                   Description = grp.Key.Description,
                   Id = grp.Key.Id,
                   Name = grp.Key.Name,
                   TestConditionCondition = grp.Key.TestConditionCondition,
                   TestConditionId = grp.Key.TestConditionId
               })
               .ToList();

            var results =
                 qualityControlTestResults.Distinct()
                     .GroupBy(qct => new
                     {
                         qct.Id,
                         qct.Name,
                         qct.Code,
                         qct.Description
                     })
                     .Select(grp => new
                     {
                         QualityControlTestId = grp.Key.Id,
                         QualityControlTestName = grp.Key.Name,
                         Code = grp.Key.Code,
                         Description = grp.Key.Description,
                         Conditions = grp.ToList(),
                     });


            foreach (var item in results)
            {
                EditQualityControlTestInput editQualityControlTestInput = new EditQualityControlTestInput();
                editQualityControlTestInput.AddTestConditions = item.Conditions.Select(i =>
                new AddTestConditionInput
                {
                    Id = i.TestConditionId
                }
                ).ToArray();
                editQualityControlTestInput.DeleteTestConditions = new DeleteTestConditionInput[0];
                editQualityControlTestInput.Code = item.Code;
                editQualityControlTestInput.Description = item.Description;
                editQualityControlTestInput.Id = item.QualityControlTestId;
                editQualityControlTestInput.Name = item.QualityControlTestName;

                _sb.AppendLine("QualityControlTestId: " + item.QualityControlTestId);

                var editResult = await EditQualityControlTest(editQualityControlTestInput);
                var editResultJson = JsonConvert.SerializeObject(editResult);
                _sb.AppendLine("EditResult: " + editResultJson);

                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }
            #endregion

            btnInsert2.Enabled = true;
        }

        private async void btnAddTestCondition_Click(object sender, EventArgs e)
        {
            btnAddTestCondition.Enabled = false;

            for (int row = 2; row < 550; row++)
            {
                _sb.AppendLine("Row: " + row);

                string testConditionValue = null;
                var testConditionCell = _xlRange.Cells[row, 5];
                if (testConditionCell != null && testConditionCell.Value2 != null)
                {
                    testConditionValue = (string)testConditionCell.Value2.ToString();
                    _sb.AppendLine("qualityControlTestDescription: " + testConditionValue);
                }
                else continue;

                AddTestConditionInput addTestConditionInput = new AddTestConditionInput();
                addTestConditionInput.Condition = testConditionValue;

                var addTestConditionResult = await AddTestCondition(addTestConditionInput);
                var addTestConditionJson = JsonConvert.SerializeObject(addTestConditionResult);
                _sb.AppendLine("AddTestConditionResult: " + addTestConditionJson);

                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnAddTestCondition.Enabled = true;
        }

        private async void btnSaveStuffQualityControlTests_Click(object sender, EventArgs e)
        {
            btnSaveStuffQualityControlTests.Enabled = false;

            for (int row = 2; row < 550; row++)
            {
                _sb.AppendLine("Row: " + row);

                #region StuffCode
                string stuffCodeValue = null;
                var stuffCodeCell = _xlRange.Cells[row, 1];
                if (stuffCodeCell != null && stuffCodeCell.Value2 != null)
                {
                    stuffCodeValue = (string)stuffCodeCell.Value2.ToString();
                    _sb.AppendLine("StuffCode: " + stuffCodeValue);
                }
                else continue;

                GetStuffsInput getStuffsInput = new GetStuffsInput(
                    pagingInput: null,
                    sortType: StuffSortType.Code,
                    sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                getStuffsInput.Code = stuffCodeValue;
                getStuffsInput.AdvanceSearchItems = new AdvanceSearchItem[0];

                var stuffResult = await GetStuff(getStuffsInput);
                _sb.AppendLine("StuffId: " + stuffResult.Id);

                if (stuffResult == null) continue;
                #endregion

                #region Line
                QualityControlTestResult qualityControlTestResult = null;
                var lineCell = _xlRange.Cells[row, 4];
                if (lineCell != null && lineCell.Value2 != null)
                {
                    var lineValue = (string)lineCell.Value2.ToString();
                    _sb.AppendLine("Line: " + lineValue);

                    GetQualityControlTestsInput getQualityControlTestsInput = new GetQualityControlTestsInput(
                        pagingInput: null,
                        sortType: QualityControlTestSortType.Name,
                        sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getQualityControlTestsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getQualityControlTestsInput.Name = lineValue;

                    qualityControlTestResult = await GetQualityControlTest(getQualityControlTestsInput);
                }

                if (qualityControlTestResult == null) continue;
                #endregion

                List<AddStuffQualityControlTestDocument> addQualityControlTestInputs = new List<AddStuffQualityControlTestDocument>()
                {
                    new AddStuffQualityControlTestDocument
                    {
                        QualityControlTestId = qualityControlTestResult.Id,
                        AddStuffQualityControlTestConditionInputs = new AddStuffQualityControlTestConditionInput[0]
                    }
                };

                SaveStuffQualityControlTestsInput saveStuffQualityControlTestsInput = new SaveStuffQualityControlTestsInput();

                saveStuffQualityControlTestsInput.AddQualityControlTestInputs = addQualityControlTestInputs.ToArray();
                saveStuffQualityControlTestsInput.DeleteQualityControlTestIds = new long[0];
                saveStuffQualityControlTestsInput.EditQualityControlTestInputs = new EditStuffQualityControlTestDocument[0];
                saveStuffQualityControlTestsInput.StuffId = stuffResult.Id;
                var saveStuffQualityControlTestsResult = await SaveStuffQualityControlTests(saveStuffQualityControlTestsInput);
                var saveStuffQualityControlTestsJson = JsonConvert.SerializeObject(saveStuffQualityControlTestsResult);

                _sb.AppendLine("SaveStuffQualityControlTestsResult: " + saveStuffQualityControlTestsJson);


                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnSaveStuffQualityControlTests.Enabled = true;
        }

        private void QualityControlTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseFile();
        }

        private async void btnLinkStuffQcTestToTestCondition_Click(object sender, EventArgs e)
        {
            btnLinkStuffQcTestToTestCondition.Enabled = false;

            for (int row = 2; row < 550; row++)
            {
                _sb.AppendLine("Row: " + row);

                #region StuffCode
                string stuffCodeValue = null;
                var stuffCodeCell = _xlRange.Cells[row, 1];
                if (stuffCodeCell != null && stuffCodeCell.Value2 != null)
                {
                    stuffCodeValue = (string)stuffCodeCell.Value2.ToString();
                    _sb.AppendLine("StuffCode: " + stuffCodeValue);
                }
                else continue;

                GetStuffsInput getStuffsInput = new GetStuffsInput(
                    pagingInput: null,
                    sortType: StuffSortType.Code,
                    sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                getStuffsInput.Code = stuffCodeValue;
                getStuffsInput.AdvanceSearchItems = new AdvanceSearchItem[0];

                var stuffResult = await GetStuff(getStuffsInput);
                _sb.AppendLine("StuffId: " + stuffResult.Id);

                if (stuffResult == null) continue;
                #endregion

                #region Line
                QualityControlTestResult qualityControlTestResult = null;
                var lineCell = _xlRange.Cells[row, 4];
                if (lineCell != null && lineCell.Value2 != null)
                {
                    var lineValue = (string)lineCell.Value2.ToString();
                    _sb.AppendLine("Line: " + lineValue);

                    GetQualityControlTestsInput getQualityControlTestsInput = new GetQualityControlTestsInput(
                        pagingInput: null,
                        sortType: QualityControlTestSortType.Name,
                        sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getQualityControlTestsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getQualityControlTestsInput.Name = lineValue;

                    qualityControlTestResult = await GetQualityControlTest(getQualityControlTestsInput);
                }

                if (qualityControlTestResult == null) continue;
                #endregion

                #region Unit
                QualityControlTestUnitResult qualityControlTestUnitResult = null;
                var unitCell = _xlRange.Cells[row, 8];
                if (unitCell != null && unitCell.Value2 != null)
                {
                    var unitValue = (string)unitCell.Value2.ToString();
                    _sb.AppendLine("Line: " + unitValue);

                    GetQualityControlTestUnitsInput getQualityControlTestUnitsInput = new GetQualityControlTestUnitsInput(
                        pagingInput: null,
                        sortType: QualityControlTestUnitSortType.Name,
                        sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getQualityControlTestUnitsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getQualityControlTestUnitsInput.Name = unitValue;

                    qualityControlTestUnitResult = await GetQualityControlTestUnit(getQualityControlTestUnitsInput);
                }

                if (qualityControlTestUnitResult == null) continue;
                #endregion

                #region TestCondition
                TestConditionResult testConditionResult = null;

                var testConditionCell = _xlRange.Cells[row, 5];
                if (testConditionCell != null && testConditionCell.Value2 != null)
                {
                    var testCondictionValue = (string)testConditionCell.Value2.ToString();
                    _sb.AppendLine("Condition: " + testCondictionValue);

                    GetTestConditionsInput getTestConditionsInput = new GetTestConditionsInput(
                        pagingInput: null,
                        sortType: TestConditionSortType.Id,
                        sortOrder: System.Data.SqlClient.SortOrder.Ascending);
                    getTestConditionsInput.AdvanceSearchItems = new AdvanceSearchItem[0];
                    getTestConditionsInput.Condition = testCondictionValue;

                    testConditionResult = await GetTestConditions(getTestConditionsInput);
                }

                if (testConditionResult == null) continue;

                #endregion

                #region AcceptanceLimit
                string acceptanceLimit = null;
                var acceptanceLimitCell = _xlRange.Cells[row, 10];
                if (acceptanceLimitCell != null && acceptanceLimitCell.Value2 != null)
                {
                    var acceptanceLimitValue = (string)acceptanceLimitCell.Value2.ToString();
                    acceptanceLimit = acceptanceLimitValue;
                    _sb.AppendLine("AcceptanceLimit: " + acceptanceLimit);
                }

                #endregion

                #region Description
                string description = null;
                var descriptionCell = _xlRange.Cells[row, 11];
                if (descriptionCell != null && descriptionCell.Value2 != null)
                {
                    var descriptionValue = (string)descriptionCell.Value2.ToString();
                    description = descriptionValue;
                    _sb.AppendLine("Description: " + description);
                }
                #endregion

                #region Min
                double min = 0;
                var minCell = _xlRange.Cells[row, 6];
                if (minCell != null && minCell.Value2 != null)
                {
                    var minValue = (string)minCell.Value2.ToString();
                    min = double.Parse(minValue);
                    _sb.AppendLine("Min: " + min);
                }
                #endregion

                #region Max
                double max = 0;
                var maxCell = _xlRange.Cells[row, 7];
                if (maxCell != null && maxCell.Value2 != null)
                {
                    var maxValue = (string)maxCell.Value2.ToString();
                    max = double.Parse(maxValue);
                    _sb.AppendLine("Max: " + max);
                }
                #endregion

                #region ToleranceType
                ToleranceType toleranceType = ToleranceType.Descriptive;
                var toleranceTypeCell = _xlRange.Cells[row, 9];
                if (toleranceTypeCell != null && toleranceTypeCell.Value2 != null)
                {
                    var toleranceTypeValue = (string)toleranceTypeCell.Value2.ToString();
                    _sb.AppendLine("ToleranceTypeValue: " + toleranceTypeValue);

                    switch (toleranceTypeValue)
                    {
                        case "وصفی":
                            toleranceType = ToleranceType.Descriptive;
                            break;

                        case "یک طرفه راست":
                            toleranceType = ToleranceType.OneSidedRight;
                            break;

                        case "یک طرفه چپ":
                            toleranceType = ToleranceType.OneSidedLeft;
                            break;

                        case "دو طرفه":
                            toleranceType = ToleranceType.TwoSided;
                            break;

                        default:
                            continue;
                    }
                }
                #endregion


                List<EditStuffQualityControlTestDocument> editQualityControlTestInputs = new List<EditStuffQualityControlTestDocument>()
                {
                    new EditStuffQualityControlTestDocument
                    {
                        QualityControlTestId = qualityControlTestResult.Id,
                        AddStuffQualityControlTestConditionInputs = new List<AddStuffQualityControlTestConditionInput>
                        {
                            new AddStuffQualityControlTestConditionInput
                            {
                                AcceptanceLimit = acceptanceLimit,
                                Min = min,
                                Max = max,
                                Description = description,
                                QualityControlConditionTestConditionId = testConditionResult.Id,
                                QualityControlTestConditionQualityControlTestId = qualityControlTestResult.Id,
                                QualityControlTestUnitId = qualityControlTestUnitResult.Id,
                                QualityControlTestId = qualityControlTestResult.Id,
                                StuffId = stuffResult.Id,
                                ToleranceType = toleranceType,
                            }
                        }.ToArray(),
                        DeleteStuffQualityControlTestConditionInputs = new DeleteStuffQualityControlTestConditionInput[0]
                    }
                };

                SaveStuffQualityControlTestsInput saveStuffQualityControlTestsInput = new SaveStuffQualityControlTestsInput();
                saveStuffQualityControlTestsInput.AddQualityControlTestInputs = new AddStuffQualityControlTestDocument[0];
                saveStuffQualityControlTestsInput.DeleteQualityControlTestIds = new long[0];
                saveStuffQualityControlTestsInput.EditQualityControlTestInputs = editQualityControlTestInputs.ToArray();
                saveStuffQualityControlTestsInput.StuffId = stuffResult.Id;

                var saveStuffQualityControlTestsResult = await SaveStuffQualityControlTests(saveStuffQualityControlTestsInput);
                var saveStuffQualityControlTestsJson = JsonConvert.SerializeObject(saveStuffQualityControlTestsResult);

                _sb.AppendLine("SaveStuffQualityControlTestsResult: " + saveStuffQualityControlTestsJson);

                _sb.AppendLine("\n");

                richTextBox1.Text = _sb.ToString();

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

            btnLinkStuffQcTestToTestCondition.Enabled = true;
        }
    }
}
