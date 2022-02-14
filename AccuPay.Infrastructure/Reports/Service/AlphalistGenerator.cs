using AccuPay.Core.ReportModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AccuPay.Infrastructure.Reports.Service
{
    internal class AlphalistGenerator
    {
        private delegate void PrintProcedure(ExcelWorksheet worksheet,
            AlphalistModel model,
            int startNo,
            int sequenceNo);

        private const string SCHEDULE_7_1 = "Resources/schedule-7.1.xlsx";
        private const string SCHEDULE_7_3 = "Resources/schedule-7.3.xlsx";
        private const string SCHEDULE_7_4 = "Resources/schedule-7.4.xlsx";
        private const string SCHEDULE_7_5 = "Resources/schedule-7.5.xlsx";

        private string[] Schedule71Columns = new[] { "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "Q", "R", "S", "T", "U", "V", "W", "X" };

        private string[] Schedule73Columns = new[] { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "O", "P", "Q", "R", "S", "T", "U", "V" };

        private string[] Schedule74Columns = new[] { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG" };

        private string[] Schedule75Columns = new[] { "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "U", "V", "W", "X", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU" };

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private readonly string _saveFilePath;
        private readonly IList<AlphalistModel> _alphalistModels;
        private readonly int _year;

        public string OutputDirectory { get; private set; }

        public AlphalistGenerator(IList<AlphalistModel> models,
            int year,
            DateTime dateFrom,
            DateTime dateTo,
            string saveFileDiretory)
        {
            _alphalistModels = models;
            _year = year;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _saveFilePath = saveFileDiretory;
        }

        public void Start()
        {
            var taxableEmployees = _alphalistModels.
                Where(t => t.IsSchedule73).
                ToList();
            var nonTaxableEmployees = _alphalistModels.
                Where(t => t.IsSchedule75).
                ToList();
            var previouslyEmployed = _alphalistModels.
                Where(t => t.IsSchedule74).
                ToList();
            var terminatedEarly = _alphalistModels.
                Where(t => t.IsSchedule71).
                ToList();

            //foreach (var model in _alphalistModels)
            //{
            //    if (model.Category == "Schedule 7.1")
            //        terminatedEarly.Add(model);
            //    else if (model.Category != "Schedule 7.3")
            //        taxableEmployees.Add(model);
            //    else if (model.Category == "Schedule 7.4")
            //        previouslyEmployed.Add(model);
            //    else if (model.Category != "Schedule 7.5")
            //        nonTaxableEmployees.Add(model);
            //}

            //var randomId = Guid.NewGuid();
            var currentDateTime = DateTime.Now;
            var randomId = $"{currentDateTime:yyMMddHHmmss}";
            //var directoryName = "schedules";
            var directory = $"{_saveFilePath}/{randomId}";

            OutputDirectory = directory;

            Directory.CreateDirectory(directory);
            CreateReport(SCHEDULE_7_1, $"{directory}/schedule7.1", PrintRow71, terminatedEarly, Schedule71Columns);
            CreateReport(SCHEDULE_7_3, $"{directory}/schedule7.3", PrintRow73, taxableEmployees, Schedule73Columns);
            CreateReport(SCHEDULE_7_4, $"{directory}/schedule7.4", PrintRow74, previouslyEmployed, Schedule74Columns);
            CreateReport(SCHEDULE_7_5, $"{directory}/schedule7.5", PrintRow75, nonTaxableEmployees, Schedule75Columns);
        }

        private void CreateReport(string template,
            string name,
            PrintProcedure printCallback,
            List<AlphalistModel> models,
            string[] totals)
        {
            var tempId = Guid.NewGuid();

            var fileName = name + ".xlsx";

            File.Copy(template, fileName); // "C:\Users\GLOBAL-D\AppData\Local\Temp\/92a51e03-1f8c-4583-90fd-e9cb876a248f/schedules/schedule7.1.xlsx"

            var lastDayOfYear = _dateTo;

            using (ExcelPackage excel = new ExcelPackage(new FileInfo(fileName)))
            {
                var worksheet = excel.Workbook.Worksheets.FirstOrDefault();

                worksheet.Cells["A3"].Value = "AS OF " + lastDayOfYear.ToString("MMMM dd yyyy");

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

        private void PrintRow71(ExcelWorksheet worksheet, AlphalistModel model, int startNo, int sequenceNo)
        {
            var index = startNo + (sequenceNo - 1);

            worksheet.InsertRow(index, 1, index + 1);

            worksheet.Cells["A" + index].Value = sequenceNo;
            worksheet.Cells["B" + index].Value = model.TinNo;
            worksheet.Cells["C" + index].Value = model.FullNameLastNameFirst;
            worksheet.Cells["D" + index].Value = model.StartDate;
            worksheet.Cells["E" + index].Value = model.EndDate;
            worksheet.Cells["F" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["G" + index].Value = model._13thMonthPay;
            worksheet.Cells["H" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["I" + index].Value = model.GovernmentInsurance;
            worksheet.Cells["J" + index].Value = model.SalariesAndOtherCompensation;
            worksheet.Cells["K" + index].Value = model.TotalNonTaxableIncome;

            worksheet.Cells["L" + index].Value = model.TaxableBasicSalary;
            worksheet.Cells["M" + index].Value = model.Taxable13thMonthPay;
            worksheet.Cells["N" + index].Value = 0M;
            worksheet.Cells["O" + index].Value = model.TotalTaxableIncome;
            worksheet.Cells["P" + index].Value = model.ExemptionCode;
            worksheet.Cells["Q" + index].Value = model.TotalExemptions;
            worksheet.Cells["R" + index].Value = model.PremiumPaidOnHealth;
            worksheet.Cells["S" + index].Value = model.NetTaxableIncome;
            worksheet.Cells["T" + index].Value = model.TaxDue;
            worksheet.Cells["U" + index].Value = model.TotalTaxWithheld;

            var amountWithheld = Math.Max(model.TaxDue - model.TotalTaxWithheld, 0M);
            var overWithheld = Math.Max(model.TotalTaxWithheld - model.TaxDue, 0M);
            var adjustedAmountWithheld = model.TotalTaxWithheld - overWithheld;

            worksheet.Cells["V" + index].Value = amountWithheld;
            worksheet.Cells["W" + index].Value = overWithheld;
            worksheet.Cells["X" + index].Value = adjustedAmountWithheld;
            worksheet.Cells["Y" + index].Value = null;
        }

        private void PrintRow73(ExcelWorksheet worksheet, AlphalistModel model, int startNo, int sequenceNo)
        {
            var index = startNo + (sequenceNo - 1);

            worksheet.InsertRow(index, 1, index + 1);

            worksheet.Cells["A" + index].Value = sequenceNo;
            worksheet.Cells["B" + index].Value = model.TinNo;
            worksheet.Cells["C" + index].Value = model.FullNameLastNameFirst;
            worksheet.Cells["D" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["E" + index].Value = model._13thMonthPay;
            worksheet.Cells["F" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["G" + index].Value = model.GovernmentInsurance;
            worksheet.Cells["H" + index].Value = model.SalariesAndOtherCompensation;
            worksheet.Cells["I" + index].Value = model.TotalNonTaxableIncome;

            worksheet.Cells["J" + index].Value = model.TaxableBasicSalary;
            worksheet.Cells["K" + index].Value = model.Taxable13thMonthPay;
            worksheet.Cells["L" + index].Value = 0M;
            worksheet.Cells["M" + index].Value = model.TotalTaxableIncome;
            worksheet.Cells["N" + index].Value = model.ExemptionCode;
            worksheet.Cells["O" + index].Value = model.TotalExemptions;
            worksheet.Cells["P" + index].Value = model.PremiumPaidOnHealth;
            worksheet.Cells["Q" + index].Value = model.NetTaxableIncome;
            worksheet.Cells["R" + index].Value = model.TaxDue;
            worksheet.Cells["S" + index].Value = model.TotalTaxWithheld;

            var amountWithheld = Math.Max(model.TaxDue - model.TotalTaxWithheld, 0M);
            var overWithheld = Math.Max(model.TotalTaxWithheld - model.TaxDue, 0M);
            var adjustedAmountWithheld = model.TotalTaxWithheld - overWithheld;

            worksheet.Cells["T" + index].Value = amountWithheld;
            worksheet.Cells["U" + index].Value = overWithheld;
            worksheet.Cells["V" + index].Value = adjustedAmountWithheld;
            worksheet.Cells["W" + index].Value = null;
        }

        private void PrintRow74(ExcelWorksheet worksheet, AlphalistModel model, int startNo, int sequenceNo)
        {
            var index = startNo + (sequenceNo - 1);

            worksheet.InsertRow(index, 1, index + 1);

            worksheet.Cells["A" + index].Value = sequenceNo;
            worksheet.Cells["B" + index].Value = model.TinNo;
            worksheet.Cells["C" + index].Value = model.FullNameLastNameFirst;
            worksheet.Cells["D" + index].Value = 0M;
            worksheet.Cells["E" + index].Value = 0M;
            worksheet.Cells["F" + index].Value = 0M;
            worksheet.Cells["G" + index].Value = 0M;
            worksheet.Cells["H" + index].Value = 0M;
            worksheet.Cells["I" + index].Value = 0M;
            worksheet.Cells["J" + index].Value = 0M;
            worksheet.Cells["K" + index].Value = 0M;
            worksheet.Cells["L" + index].Value = 0M;
            worksheet.Cells["M" + index].Value = 0M;
            worksheet.Cells["N" + index].Value = model._13thMonthPay;
            worksheet.Cells["O" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["P" + index].Value = model.GovernmentInsurance;
            worksheet.Cells["Q" + index].Value = model.SalariesAndOtherCompensation;
            worksheet.Cells["R" + index].Value = model.TotalNonTaxableIncome;
            worksheet.Cells["S" + index].Value = model.TaxableBasicSalary;
            worksheet.Cells["T" + index].Value = model.Taxable13thMonthPay;
            worksheet.Cells["U" + index].Value = model.SalariesAndOtherCompensation;
            worksheet.Cells["V" + index].Formula = string.Format("=S{0} + T{0} + U{0}", index);
            worksheet.Cells["W" + index].Formula = string.Format("=M{0} + V{0}", index);
            worksheet.Cells["X" + index].Value = model.ExemptionCode;
            worksheet.Cells["Y" + index].Value = model.TotalExemptions;
            worksheet.Cells["Z" + index].Value = model.PremiumPaidOnHealth;
            worksheet.Cells["AA" + index].Value = model.NetTaxableIncome;
            worksheet.Cells["AB" + index].Value = model.TaxDue;
            worksheet.Cells["AC" + index].Value = model.PreviousTaxWithheld;
            worksheet.Cells["AD" + index].Value = model.PresentTaxWithheld;

            var amountWithheld = Math.Max(model.TaxDue - model.TotalTaxWithheld, 0M);
            var overWithheld = Math.Max(model.TotalTaxWithheld - model.TaxDue, 0M);
            var adjustedAmountWithheld = model.TotalTaxWithheld - overWithheld;

            worksheet.Cells["AE" + index].Value = amountWithheld;
            worksheet.Cells["AF" + index].Value = overWithheld;
            worksheet.Cells["AG" + index].Value = adjustedAmountWithheld;
        }

        private void PrintRow75(ExcelWorksheet worksheet, AlphalistModel model, int startNo, int sequenceNo)
        {
            var index = startNo + (sequenceNo - 1);

            worksheet.InsertRow(index, 1, index + 1);

            worksheet.Cells["A" + index].Value = sequenceNo;
            worksheet.Cells["B" + index].Value = model.TinNo;
            worksheet.Cells["C" + index].Value = model.FullNameLastNameFirst;
            worksheet.Cells["D" + index].Value = "REG";
            worksheet.Cells["E" + index].Value = 0M;
            worksheet.Cells["F" + index].Value = 0M;
            worksheet.Cells["G" + index].Value = 0M;
            worksheet.Cells["H" + index].Value = 0M;
            worksheet.Cells["I" + index].Value = 0M;
            worksheet.Cells["J" + index].Value = 0M;
            worksheet.Cells["K" + index].Value = 0M;
            worksheet.Cells["L" + index].Value = 0M;
            worksheet.Cells["M" + index].Value = 0M;
            worksheet.Cells["N" + index].Value = 0M;
            worksheet.Cells["O" + index].Value = 0M;
            worksheet.Cells["P" + index].Value = 0M;
            worksheet.Cells["Q" + index].Value = 0M;
            worksheet.Cells["R" + index].Value = 0M;
            worksheet.Cells["S" + index].Value = model.StartDate;
            worksheet.Cells["T" + index].Value = model.EndDate;
            worksheet.Cells["U" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["V" + index].Value = model.MinimumWagePerDay;
            worksheet.Cells["W" + index].Value = ParseDecimal(model.MinimumWagePerMonth);
            worksheet.Cells["X" + index].Value = ParseDecimal(model.MinimumWagePerMonth) * 12;
            worksheet.Cells["Y" + index].Value = model.WorkDaysPerYear;
            worksheet.Cells["Z" + index].Value = model.HolidayPay;
            worksheet.Cells["AA" + index].Value = model.OvertimePay;
            worksheet.Cells["AB" + index].Value = model.NightDiffPay;
            worksheet.Cells["AC" + index].Value = model.HazardPay;
            worksheet.Cells["AD" + index].Value = model._13thMonthPay;
            worksheet.Cells["AE" + index].Value = model.DeMinimisBenefits;
            worksheet.Cells["AF" + index].Value = model.GovernmentInsurance;
            worksheet.Cells["AG" + index].Value = model.SalariesAndOtherCompensation;
            worksheet.Cells["AH" + index].Value = model.Taxable13thMonthPay;
            worksheet.Cells["AI" + index].Value = model.SalariesAndOtherCompensation;
            worksheet.Cells["AJ" + index].Value = model.GrossCompensationIncome;
            worksheet.Cells["AK" + index].Value = model.GrossTaxableIncome;
            worksheet.Cells["AL" + index].Value = model.ExemptionCode;
            worksheet.Cells["AM" + index].Value = model.TotalExemptions;
            worksheet.Cells["AN" + index].Value = model.PremiumPaidOnHealth;
            worksheet.Cells["AO" + index].Value = model.NetTaxableIncome;
            worksheet.Cells["AP" + index].Value = model.TaxDue;
            worksheet.Cells["AQ" + index].Value = model.PreviousTaxWithheld;
            worksheet.Cells["AR" + index].Value = model.PresentTaxWithheld;

            var amountWithheld = Math.Max(model.TaxDue - model.TotalTaxWithheld, 0M);
            var overWithheld = Math.Max(model.TotalTaxWithheld - model.TaxDue, 0M);
            var adjustedAmountWithheld = model.TotalTaxWithheld - overWithheld;

            worksheet.Cells["AS" + index].Value = amountWithheld;
            worksheet.Cells["AT" + index].Value = overWithheld;
            worksheet.Cells["AU" + index].Value = adjustedAmountWithheld;
        }

        private decimal ParseDecimal(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? 0M : System.Convert.ToDecimal(value);
        }
    }
}
