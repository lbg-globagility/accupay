using AccuPay.Data.Exceptions;
using AccuPay.Data.Interfaces;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Reports;
using AccuPay.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Infrastructure.Reports.ExcelReportColumn;

namespace AccuPay.Infrastructure.Reports
{
    public class EmployeePersonalProfilesReportBuilder : ExcelFormatReport, IEmployeePersonalProfilesReportBuilder
    {
        private const string REPORT_NAME = "EmployeePersonalProfiles";
        private readonly OrganizationRepository _organizationRepository;
        private readonly EmployeePersonalProfilesExcelFormatReportDataService _reportDataService;
        private readonly IReadOnlyCollection<ExcelReportColumn> _reportColumns;
        protected new const float FontSize = 11;

        public EmployeePersonalProfilesReportBuilder(
            OrganizationRepository organizationRepository,
            EmployeePersonalProfilesExcelFormatReportDataService employeePersonalProfilesExcelFormatReportDataService)
        {
            _organizationRepository = organizationRepository;
            _reportDataService = employeePersonalProfilesExcelFormatReportDataService;

            _reportColumns = GetReportColumns();
        }

        private ReadOnlyCollection<ExcelReportColumn> GetReportColumns()
        {
            var reportColumns = new List<ExcelReportColumn>()
            {
                new ExcelReportColumn("Employee ID", "EmployeeID", ColumnType.Text),
                new ExcelReportColumn("Last Name", "LastName", ColumnType.Text),
                new ExcelReportColumn("First Name", "FirstName", ColumnType.Text),
                new ExcelReportColumn("Middle Name", "MiddleName", ColumnType.Text, optional: true),
                new ExcelReportColumn("Salutation", "Salutation", ColumnType.Text, optional: true),
                new ExcelReportColumn("Birth Date", "Birthdate", ColumnType.Text, optional: true),
                new ExcelReportColumn("Nickname", "Nickname", ColumnType.Text, optional: true),
                new ExcelReportColumn("Gender", "Gender", ColumnType.Text, optional: true),
                new ExcelReportColumn("Email Address", "EmailAddress", ColumnType.Text, optional: true),
                new ExcelReportColumn("Work Phone No.", "WorkPhone", ColumnType.Text, optional: true),
                new ExcelReportColumn("Home Phone No.", "HomePhone", ColumnType.Text, optional: true),
                new ExcelReportColumn("Mobile Phone No.", "MobilePhone", ColumnType.Text, optional: true),
                new ExcelReportColumn("Home Address", "HomeAddress", ColumnType.Text, optional: true),
                new ExcelReportColumn("TIN", "TINNo", ColumnType.Text, optional: true),
                new ExcelReportColumn("SSS No.", "SSSNo", ColumnType.Text, optional: true),
                new ExcelReportColumn("HDMF No.", "HDMFNo", ColumnType.Text, optional: true),
                new ExcelReportColumn("PhilHealth No.", "PhilHealthNo", ColumnType.Text, optional: true),
                new ExcelReportColumn("Employment Status", "EmploymentStatus", ColumnType.Text, optional: true),
                new ExcelReportColumn("Employee Type", "EmployeeType", ColumnType.Text, optional: true),
                new ExcelReportColumn("Marital Status", "MaritalStatus", ColumnType.Text, optional: true),
                new ExcelReportColumn("Employment Date", "StartDate", ColumnType.Text, optional: true),
                new ExcelReportColumn("Termination Date", "TerminationDate", ColumnType.Text, optional: true),
                new ExcelReportColumn("Job Name", "PositionName", ColumnType.Text, optional: true),
                new ExcelReportColumn("Pay Frequency", "PayFrequencyType", ColumnType.Text, optional: true),
                new ExcelReportColumn("Vacation Leave Balance", "LeaveBalance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Sick Leave Balance", "SickLeaveBalance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Maternal/Paternal Leave Balance", "MaternityLeaveBalance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Other Leave Balance", "OtherLeaveBalance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Vacation Leave Allowance", "LeaveAllowance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Sick Leave Allowance", "SickLeaveAllowance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Maternity Leave Allowance", "MaternityLeaveAllowance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Other Leave Allowance", "OtherLeaveAllowance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Vacation Leave Per PayPeriod", "LeavePerPayPeriod", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Sick Leave Per Pay Period", "SickLeavePerPayPeriod", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Maternal/Paternal Leave Per Pay Period", "MaternityLeavePerPayPeriod", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Other Leave Per Pay Period", "OtherLeavePerPayPeriod", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Work Days Per Year", "WorkDaysPerYear", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Day Of Rest", "DayOfRest", ColumnType.Text, optional: true),
                new ExcelReportColumn("ATM No.", "ATMNo", ColumnType.Text, optional: true),
                new ExcelReportColumn("Bank Name", "BankName", ColumnType.Text, optional: true),
                new ExcelReportColumn("Legal Holiday Eligibile", "CalcHoliday", ColumnType.Text),
                new ExcelReportColumn("Special Holiday Eligibile", "CalcSpecialHoliday", ColumnType.Text),
                new ExcelReportColumn("Night Diff. Eligibile", "CalcNightDiff", ColumnType.Text),
                new ExcelReportColumn("Night Diff. OT Eligibile", "CalcNightDiffOT", ColumnType.Text),
                new ExcelReportColumn("RestDay Eligibile", "CalcRestDay", ColumnType.Text),
                new ExcelReportColumn("RestDay OT Eligibile", "CalcRestDayOT", ColumnType.Text),
                new ExcelReportColumn("Date Regularized", "DateRegularized", ColumnType.Text, optional: true),
                new ExcelReportColumn("Date Evaluated", "DateEvaluated", ColumnType.Text, optional: true),
                new ExcelReportColumn("Late Grace Period", "LateGracePeriod", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Agency Name", "AgencyName", ColumnType.Text, optional: true),
                new ExcelReportColumn("Offset Balance", "OffsetBalance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Branch Name", "BranchName", ColumnType.Text, optional: true),
                new ExcelReportColumn("Minimum Overtime", "MinimumOvertime", ColumnType.Text, optional: true),
                new ExcelReportColumn("Advancement Points", "AdvancementPoints", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("BPI Insurance", "BPIInsurance", ColumnType.Numeric, optional: true),
                new ExcelReportColumn("Employment Policy Name", "EmploymentPolicyName", ColumnType.Text, optional: true)
            };

            return new ReadOnlyCollection<ExcelReportColumn>(reportColumns);
        }

        public async Task CreateReport(int organizationId, string saveFilePath)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                throw new BusinessLogicException("Organization does not exists.");

            var employeeTable = _reportDataService.GetData(organizationId);

            var allEmployees = employeeTable.Rows.OfType<DataRow>().ToList();

            if (allEmployees.Count <= 0)
                throw new BusinessLogicException("No employees.");

            var newFile = new FileInfo(saveFilePath);

            var viewableReportColumns = GetViewableReportColumns(allEmployees);

            using (var excel = new ExcelPackage(newFile))
            {
                var worksheet = excel.Workbook.Worksheets.Add(REPORT_NAME);
                worksheet.Cells.Style.Font.Size = FontSize;

                int rowIndex = 1;

                RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns);

                rowIndex += 1;

                RenderWorksheet(worksheet, rowIndex, viewableReportColumns, allEmployees);

                worksheet.Cells.AutoFitColumns();

                excel.Save();
            }
        }

        private void RenderWorksheet(ExcelWorksheet worksheet, int rowIndex, IReadOnlyCollection<ExcelReportColumn> viewableReportColumns, List<DataRow> allEmployees)
        {
            var lastCell = string.Empty;
            var employeesLastIndex = 0;

            foreach (var employee in allEmployees)
            {
                var letters = GenerateAlphabet().GetEnumerator();

                foreach (var reportColumn in viewableReportColumns)
                {
                    letters.MoveNext();

                    var alphabet = letters.Current;

                    var column = $"{alphabet}{rowIndex}";

                    var cell = worksheet.Cells[column];
                    var sourceName = reportColumn.Source;
                    cell.Value = GetCellValue(employee, sourceName);

                    if (reportColumn.Type == ColumnType.Numeric)
                    {
                        cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                    }
                }

                lastCell = letters.Current;

                employeesLastIndex = rowIndex;
                rowIndex += 1;
            }
        }

        private IReadOnlyCollection<ExcelReportColumn> GetViewableReportColumns(List<DataRow> allEmployees)
        {
            var viewableReportColumns = new List<ExcelReportColumn>();
            foreach (var reportColumn in _reportColumns)
            {
                if (reportColumn.Optional)
                {
                    var hasValueQuery = allEmployees
                        .Where(row => !Convert.IsDBNull(row[reportColumn.Source]));

                    if (reportColumn.Type == ColumnType.Numeric)
                    {
                        hasValueQuery = hasValueQuery
                            .Where(row => ObjectUtils.ToDecimal(row[reportColumn.Source]) != 0);
                    }
                    else
                    {
                        hasValueQuery = hasValueQuery
                            .Where(row => !string.IsNullOrWhiteSpace(row[reportColumn.Source].ToString()));
                    }

                    if (hasValueQuery.Any())
                        viewableReportColumns.Add(reportColumn);
                }
                else
                    viewableReportColumns.Add(reportColumn);
            }

            return viewableReportColumns;
        }

        private object GetCellValue(DataRow employee, string sourceName)
        {
            return employee[sourceName];
        }
    }
}