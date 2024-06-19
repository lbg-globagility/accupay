using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ReportModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccuPay.Infrastructure.Reports.Service
{
    internal class AlphalistGenerator
    {
        private delegate void PrintProcedure(ExcelWorksheet worksheet,
            AlphalistModel model,
            int startNo,
            int sequenceNo);

        private const string SCHEDULE1 = "Resources/SCHEDULE1.xlsx";
        private const string SCHEDULE2 = "Resources/SCHEDULE2.xlsx";

        private string[] Schedule1Columns = new[] { "H", "I", "J", "K", "L", "M", "N", "O", "Q", "W", "X", "Y", "Z" }.
            Concat(Enumerable.Range(65, 14).Select(n => new StringBuilder().Append("A").Append(Convert.ToChar(n)).ToString()).ToArray()).
            ToArray();

        private string[] Schedule2Columns = Enumerable.Range(72, 18).Select(n => Convert.ToChar(n).ToString()).ToArray().
            Concat(Enumerable.Range(69, 21).Select(n => new StringBuilder().Append("A").Append(Convert.ToChar(n)).ToString()).ToArray()).
            ToArray();

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private readonly string _saveFilePath;
        private readonly Organization _organization;
        private readonly PayPeriod _endPeriod;
        private readonly IList<AlphalistModel> _alphalistModels;
        private readonly int _year;

        public string OutputDirectory { get; private set; }

        private readonly ISystemOwnerService _systemOwnerService;
        private readonly IListOfValueRepository _listOfValueRepository;

        public AlphalistGenerator(IList<AlphalistModel> models,
            int year,
            DateTime dateFrom,
            DateTime dateTo,
            string saveFileDiretory,
            Organization organization,
            PayPeriod endPeriod,
            ISystemOwnerService systemOwnerService,
            IListOfValueRepository listOfValueRepository)
        {
            _alphalistModels = models;
            _year = year;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _saveFilePath = saveFileDiretory;
            _organization = organization;
            _endPeriod = endPeriod;
            _systemOwnerService = systemOwnerService;
            _listOfValueRepository = listOfValueRepository;
        }

        public void Start()
        {
            var taxableEmployees = _alphalistModels.
                Where(t => t.IsSchedule71 || t.IsSchedule73 || t.IsSchedule74).
                ToList();
            var nonTaxableEmployees = _alphalistModels.
                Where(t => t.IsSchedule75).
                ToList();

            var currentDateTime = DateTime.Now;
            var randomId = $"{currentDateTime:yyMMddHHmmss}";
            var directory = $"{_saveFilePath}/{randomId}";

            OutputDirectory = directory;

            Directory.CreateDirectory(directory);
            CreateReport(SCHEDULE1, $"{directory}/SCHEDULE1", printCallback: PrintRowSchedule1, taxableEmployees, Schedule1Columns);
            CreateReport(SCHEDULE2, $"{directory}/SCHEDULE2", printCallback: PrintRowSchedule2, nonTaxableEmployees, Schedule2Columns);
        }

        private void CreateReport(string template,
            string name,
            PrintProcedure printCallback,
            List<AlphalistModel> models,
            string[] totals)
        {
            var tempId = Guid.NewGuid();

            var fileName = name + ".xlsx";

            System.IO.File.Copy(template, fileName);

            var lastDayOfYear = _dateTo;

            using (ExcelPackage excel = new ExcelPackage(new FileInfo(fileName)))
            {
                var worksheet = excel.Workbook.Worksheets.FirstOrDefault();

                if(_systemOwnerService.GetCurrentSystemOwner() == SystemOwner.RGI)
                {
                    worksheet.Protection.IsProtected = true;
                    worksheet.Protection.SetPassword(_listOfValueRepository.GetExcelPassword());
                }

                var isFirstHalf = _endPeriod.IsFirstHalf;
                var year = _endPeriod.Year;
                var month = _endPeriod.Month;
                var lastDayOfMonth = DateTime.DaysInMonth(year, month);
                var datePresentation = isFirstHalf ? new DateTime(year, month, 15) : new DateTime(year, month, lastDayOfMonth);
                worksheet.Cells["A3"].Value = $"AS OF {datePresentation.ToString(format: "MMMM dd, yyyy")}";
                worksheet.Cells["A5"].Value = $"TIN: {_organization.Tinno}";
                worksheet.Cells["A7"].Value = $"WITHHOLDING AGENT'S NAME: {_organization.Name}";

                var startNo = 17;
                var sequenceNo = 1;
                var offsetNo = 0;

                foreach (var model in models)
                {
                    printCallback(worksheet, model, startNo, sequenceNo);
                    sequenceNo += 1;
                }

                var totalRowIndex = startNo + sequenceNo + 1;
                var endNo = startNo + sequenceNo;

                foreach (var c in totals)
                    worksheet.Cells[c + totalRowIndex].Formula = string.Format("=SUM({0}{1}:{0}{2})", c, startNo, endNo);

                excel.Save();
            }
        }

        private void PrintRowSchedule1(ExcelWorksheet worksheet, AlphalistModel model, int startNo, int sequenceNo)
        {
            var amountWithheld = Math.Max(model.TaxDue - model.TotalTaxWithheld, 0M);
            var overWithheld = Math.Max(model.TotalTaxWithheld - model.TaxDue, 0M);
            var adjustedAmountWithheld = model.TotalTaxWithheld - overWithheld;

            var index = startNo + (sequenceNo - 1);

            worksheet.InsertRow(index, 1, index + 1);

            worksheet.Cells["A" + index].Value = sequenceNo;
            worksheet.Cells["B" + index].Value = model.FullNameLastNameFirst;
            worksheet.Cells["C" + index].Value = string.Empty;
            worksheet.Cells["D" + index].Value = model.EmploymentStatusLegend;
            worksheet.Cells["E" + index].Value = model.StartDate;
            worksheet.Cells["F" + index].Value = model.EndDate;
            worksheet.Cells["G" + index].Value = model.SeparationReason;

            //worksheet.Cells["H" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["H" + index].Formula = $"=M{index}+Q{index}";

            worksheet.Cells["I" + index].Value = model.NonTaxable13thMonthPay;
            worksheet.Cells["J" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["K" + index].Value = model.TotalEmployeePremium;
            worksheet.Cells["L" + index].Value = model.OtherFormsOfCompensation250K; //model.TotalNonTaxableAllowance
            worksheet.Cells["M" + index].Formula = $"=I{index}+J{index}+K{index}+L{index}";

            worksheet.Cells["N" + index].Value = model.TaxableIncome;
            worksheet.Cells["O" + index].Value = model.Taxable13thMonthPay;
            worksheet.Cells["P" + index].Value = model.TotalTaxableAllowance;
            worksheet.Cells["Q" + index].Formula = $"=N{index}+O{index}+P{index}";

            worksheet.Cells["R" + index].Value = model.TinNo;
            worksheet.Cells["S" + index].Value = null;
            worksheet.Cells["T" + index].Value = null;
            worksheet.Cells["U" + index].Value = null;
            worksheet.Cells["V" + index].Value = null;

            worksheet.Cells["W" + index].Formula = $"=AB{index}+AF{index}";

            worksheet.Cells["AB" + index].Formula = $"=X{index}+Y{index}+Z{index}+AA{index}";

            worksheet.Cells["AF" + index].Formula = $"=AC{index}+AD{index}+AE{index}";

            worksheet.Cells["AG" + index].Formula = $"=AF{index}+Q{index}";

            worksheet.Cells["AH" + index].Value = model.TaxDue;

            //worksheet.Cells["AK" + index].Formula = $"=AH{index}-(AI{index}+AJ{index})";
            //worksheet.Cells["AL" + index].Formula = $"=(AI{index}+AJ{index})-AH{index}";
        }

        private void PrintRowSchedule2(ExcelWorksheet worksheet, AlphalistModel model, int startNo, int sequenceNo)
        {
            var index = startNo + (sequenceNo - 1);

            worksheet.InsertRow(index, 1, index + 1);

            worksheet.Cells["A" + index].Value = sequenceNo;
            worksheet.Cells["B" + index].Value = model.FullNameLastNameFirst;
            worksheet.Cells["C" + index].Value = model.EmploymentStatusLegend;
            worksheet.Cells["D" + index].Value = string.Empty;
            worksheet.Cells["E" + index].Value = model.StartDate;
            worksheet.Cells["F" + index].Value = model.EndDate;
            worksheet.Cells["G" + index].Value = model.SeparationReason;
            //worksheet.Cells["H" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["H" + index].Formula = $"V{index}+Y{index}";
            worksheet.Cells["I" + index].Value = model.MinimumWagePerDay;
            worksheet.Cells["J" + index].Value = model.MonthlyRate;
            worksheet.Cells["K" + index].Value = model.AnnualRate;
            worksheet.Cells["L" + index].Value = model.WorkDaysPerYear;
            worksheet.Cells["M" + index].Value = model.NonTaxableBasicSalary;
            worksheet.Cells["N" + index].Value = model.GrandTotalHolidayPay;
            worksheet.Cells["O" + index].Value = model.GrandTotalOvertimePay;
            worksheet.Cells["P" + index].Value = model.GrandTotalNightDiffPay;
            worksheet.Cells["Q" + index].Value = 0M;
            worksheet.Cells["R" + index].Value = model.NonTaxable13thMonthPay;
            worksheet.Cells["S" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["T" + index].Value = model.TotalEmployeePremium;
            worksheet.Cells["U" + index].Value = model.OtherFormsOfCompensation250K;
            //worksheet.Cells["V" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["V" + index].Formula = $"=R{index}+S{index}+T{index}+U{index}";
            worksheet.Cells["W" + index].Value = 0M;
            worksheet.Cells["X" + index].Value = 0M;
            worksheet.Cells["Y" + index].Formula = $"=W{index}+X{index}";
            worksheet.Cells["Z" + index].Value = model.TinNo;
            worksheet.Cells["AA" + index].Value = model.EmploymentStatusLegend;
            worksheet.Cells["AB" + index].Value = model.NightDiffPay;
            worksheet.Cells["AC" + index].Value = model.HazardPay;
            worksheet.Cells["AD" + index].Value = model.NonTaxable13thMonthPay;
            worksheet.Cells["AE" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["AO" + index].Formula = $"=SUM(AF{index}:AN{index})";

            worksheet.Cells["AB" + index].Value = null;
            worksheet.Cells["AC" + index].Value = null;
            worksheet.Cells["AD" + index].Value = string.Empty;
            worksheet.Cells["AE" + index].Formula = $"=AO{index}+AR{index}";
            worksheet.Cells["AR" + index].Formula = $"=AP{index}+AQ{index}";
        }
    }
}
