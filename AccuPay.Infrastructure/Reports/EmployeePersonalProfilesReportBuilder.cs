using AccuPay.Core.Interfaces;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Reports;
using AccuPay.Core.Services.Reports.Employees_Personal_Information;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using static AccuPay.Infrastructure.Reports.ExcelReportColumn;

namespace AccuPay.Infrastructure.Reports
{
    public class EmployeePersonalProfilesReportBuilder : ExcelFormatReport, IEmployeePersonalProfilesReportBuilder
    {
        private const string REPORT_NAME = "EmployeePersonalInfo";
        private readonly IOrganizationRepository _organizationRepository;
        private readonly EmployeePersonalProfilesExcelFormatReportDataService _reportDataService;
        private readonly IPolicyHelper _policy;
        private readonly IReadOnlyCollection<ExcelReportColumn> _reportColumns;
        protected new const float FontSize = 11;

        public EmployeePersonalProfilesReportBuilder(
            IOrganizationRepository organizationRepository,
            EmployeePersonalProfilesExcelFormatReportDataService reportDataService,
            IPolicyHelper policy)
        {
            _organizationRepository = organizationRepository;
            _reportDataService = reportDataService;
            _policy = policy;
            _reportColumns = GetReportColumns();
        }

        private ReadOnlyCollection<ExcelReportColumn> GetReportColumns()
        {
            var bpiInsuranceColumn = new ExcelReportColumn("BPI Insurance", "BPIInsurance", ColumnType.Numeric);
            var agencyColumn = new ExcelReportColumn("Agency Name", "AgencyName", ColumnType.Text);

            var reportColumns = new List<ExcelReportColumn>()
            {
                new ExcelReportColumn("Employee ID", "EmployeeNumber", ColumnType.Text),
                new ExcelReportColumn("Last Name", "LastName", ColumnType.Text),
                new ExcelReportColumn("First Name", "FirstName", ColumnType.Text),
                new ExcelReportColumn("Middle Name", "MiddleName", ColumnType.Text),
                new ExcelReportColumn("Salutation", "Salutation", ColumnType.Text),
                new ExcelReportColumn("Employee Type", "EmployeeType", ColumnType.Text),
                new ExcelReportColumn("Birth Date", "BirthDate", ColumnType.Text),
                new ExcelReportColumn("Gender", "Gender", ColumnType.Text),
                new ExcelReportColumn("Marital Status", "MaritalStatus", ColumnType.Text),
                new ExcelReportColumn("Email Address", "EmailAddress", ColumnType.Text),
                new ExcelReportColumn("Work Phone No.", "WorkPhone", ColumnType.Text),
                new ExcelReportColumn("Home Phone No.", "HomePhone", ColumnType.Text),
                new ExcelReportColumn("Mobile Phone No.", "MobilePhone", ColumnType.Text),
                new ExcelReportColumn("Home Address", "HomeAddress", ColumnType.Text),
                new ExcelReportColumn("TIN", "TinNo", ColumnType.Text),
                new ExcelReportColumn("SSS No.", "SssNo", ColumnType.Text),
                new ExcelReportColumn("HDMF No.", "HdmfNo", ColumnType.Text),
                new ExcelReportColumn("PhilHealth No.", "PhilHealthNo", ColumnType.Text),
                new ExcelReportColumn("Employment Date", "StartDate", ColumnType.Text),
                new ExcelReportColumn("Employment Status", "EmploymentStatus", ColumnType.Text),
                new ExcelReportColumn("Termination Date", "TerminationDate", ColumnType.Text),
                new ExcelReportColumn("Position", "PositionName", ColumnType.Text),
                // new ExcelReportColumn("Vacation Leave Balance", "LeaveBalance", ColumnType.Numeric),
                // new ExcelReportColumn("Sick Leave Balance", "SickLeaveBalance", ColumnType.Numeric),
                new ExcelReportColumn("Vacation Leave Allowance", "VacationLeaveAllowance", ColumnType.Numeric),
                new ExcelReportColumn("Sick Leave Allowance", "SickLeaveAllowance", ColumnType.Numeric),
                new ExcelReportColumn("Work Days Per Year", "WorkDaysPerYear", ColumnType.Numeric),
                new ExcelReportColumn("Rest Day", "DayOfRest", ColumnType.Text),
                new ExcelReportColumn("ATM No./Account No.", "AtmNo", ColumnType.Text),
                new ExcelReportColumn("Bank Name", "BankName", ColumnType.Text),
                new ExcelReportColumn("Legal Holiday Eligibile", "CalcHoliday", ColumnType.Text),
                new ExcelReportColumn("Special Holiday Eligibile", "CalcSpecialHoliday", ColumnType.Text),
                new ExcelReportColumn("Night Diff. Eligibile", "CalcNightDiff", ColumnType.Text),
                new ExcelReportColumn("RestDay Eligibile", "CalcRestDay", ColumnType.Text),
                new ExcelReportColumn("Date Regularized", "DateRegularized", ColumnType.Text),
                new ExcelReportColumn("Date Evaluated", "DateEvaluated", ColumnType.Text),
                new ExcelReportColumn("Late Grace Period", "LateGracePeriod", ColumnType.Numeric),
                agencyColumn,
                new ExcelReportColumn("Branch Name", "BranchName", ColumnType.Text),
                bpiInsuranceColumn
            };

            if (!_policy.UseBPIInsurance)
            {
                reportColumns.Remove(bpiInsuranceColumn);
            }

            if (!_policy.UseAgency)
            {
                reportColumns.Remove(agencyColumn);
            }

            return new ReadOnlyCollection<ExcelReportColumn>(reportColumns);
        }

        public async Task CreateReport(int organizationId, string saveFilePath)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
                throw new Exception("Organization does not exists.");

            var employees = await _reportDataService.GetData(organizationId);

            if (employees.Count <= 0)
                throw new Exception("No employees.");

            var newFile = new FileInfo(saveFilePath);

            using (var excel = new ExcelPackage(newFile))
            {
                var worksheet = excel.Workbook.Worksheets.Add(REPORT_NAME);
                worksheet.Cells.Style.Font.Size = FontSize;

                int rowIndex = 1;

                RenderColumnHeaders(worksheet, rowIndex, _reportColumns);

                rowIndex += 1;

                RenderWorksheet(worksheet, rowIndex, employees);

                worksheet.Cells.AutoFitColumns();

                excel.Save();
            }
        }

        private void RenderWorksheet(ExcelWorksheet worksheet, int rowIndex, ICollection<EmployeeRow> allEmployees)
        {
            foreach (var employee in allEmployees)
            {
                var letters = GenerateAlphabet().GetEnumerator();

                foreach (var reportColumn in _reportColumns)
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

                rowIndex += 1;
            }
        }

        private object GetCellValue(EmployeeRow employee, string sourceName)
        {
            return employee?.GetType()?.GetProperty(sourceName)?.GetValue(employee, null);
        }
    }
}
