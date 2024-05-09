using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Infrastructure.Reports;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccuPay.Infrastructure.Reports.ExcelReportColumn;

namespace AccuPay.Infrastructure.Data.Reports.Customize
{
    public class CertificationOFDTR : ExcelFormatReport, ICertificationOfDTR
    {
        public bool IsHidden { get; set; } = false;

        private readonly IOrganizationRepository _organizationRepository;
        private readonly ISystemOwnerService _systemOwnerService;
        private readonly IListOfValueRepository _listOfValueRepository;

        protected readonly PayrollContext _context;

        public CertificationOFDTR(
            IOrganizationRepository organizationRepository,
            ISystemOwnerService systemOwnerService,
            IListOfValueRepository listOfValueRepository,
            PayrollContext context)
        {
            _organizationRepository = organizationRepository;
            _systemOwnerService = systemOwnerService;
            _listOfValueRepository = listOfValueRepository;
            _context = context;
        }

        private DataTable GetTimeEntries(int employeeId, DateTime payFromDate, DateTime payToDate)
        {

            DataTable dataTable = new DataTable();
            var connection = (MySqlConnection)_context.Database.GetDbConnection();
            var cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText =
                @"DROP TEMPORARY TABLE IF EXISTS `latesttimelogs`;
                        CREATE TEMPORARY TABLE IF NOT EXISTS `latesttimelogs`
                        SELECT RowID, `Date`, EmployeeID, MAX(LastUpd) `UpdatedTimeLog`
                        FROM employeetimeentrydetails
                        WHERE EmployeeID=" + employeeId + @"
                        AND Date BETWEEN '" + payFromDate.ToString("yyyy-MM-dd") + @"' AND '" + payToDate.ToString("yyyy-MM-dd") + @"'
                        GROUP BY `Date`
                        ORDER BY `Date`, LastUpd DESC;

                        SELECT
                            ete.RowID,
                            ete.Date,
                            etd.TimeIn,
                            etd.TimeOut,
                            etd.RowID,
                            IFNULL(shiftschedules.StartTime, NULL) AS ShiftFrom,
                            IFNULL(shiftschedules.EndTime, NULL) AS ShiftTo,
                            ete.RegularHoursWorked,
                            ete.RegularHoursAmount,
                            ete.NightDifferentialHours,
                            ete.NightDiffHoursAmount,
                            ete.OvertimeHoursWorked,
                            ete.OvertimeHoursAmount,
                            ete.NightDifferentialOTHours,
                            ete.NightDiffOTHoursAmount,
                            ete.RestDayHours,
                            ete.RestDayAmount,
                            ete.RestDayOTHours,
                            ete.RestDayOTPay,
                            ete.LeavePayment,
                            ete.HoursLate,
                            ete.HoursLateAmount,
                            ete.UndertimeHours,
                            ete.UndertimeHoursAmount,
                            ete.VacationLeaveHours,
                            ete.SickLeaveHours,
                            ete.OtherLeaveHours,
                            ete.Leavepayment,
                            ete.SpecialHolidayHours,
                            ete.SpecialHolidayPay,
                            ete.SpecialHolidayOTHours,
                            ete.SpecialHolidayOTPay,
                            ete.RegularHolidayHours,
                            ete.RegularHolidayPay,
                            ete.RegularHolidayOTHours,
                            ete.RegularHolidayOTPay,
                            ete.HolidayPayAmount,
                            ete.AbsentHours,
                            ete.Absent,
                            ete.TotalHoursWorked,
                            ete.TotalDayPay,
                            ofb.OffBusStartTime,
                            ofb.OffBusEndTime,
                            ot.OTStartTime,
                            ot.OTEndTIme,
                            etd.TimeStampIn,
                            etd.TimeStampOut,
                            l.LeaveStartTime,
                            l.LeaveEndTime,
                            IFNULL(shiftschedules.IsRestDay, FALSE) `IsRestDay`,
                            ete.BranchID,
                            branch.BranchName
                        FROM employeetimeentry ete
                        LEFT JOIN `latesttimelogs`
                            ON `latesttimelogs`.EmployeeID = ete.EmployeeID AND
                                `latesttimelogs`.Date = ete.Date
                        LEFT JOIN employeetimeentrydetails etd
                            ON etd.Date = ete.Date AND
                                etd.OrganizationID = ete.OrganizationID AND
                                etd.EmployeeID = ete.EmployeeID AND
                                etd.RowID = `latesttimelogs`.RowID
                        LEFT JOIN (
                            SELECT EmployeeID, OffBusStartDate Date, MAX(Created) Created
                            FROM employeeofficialbusiness
                            WHERE OffBusStartDate BETWEEN @DateFrom AND @DateTo
                            GROUP BY EmployeeID, Date
                        ) latestOb
                            ON latestOb.EmployeeID = ete.EmployeeID AND
                                latestOb.Date = ete.Date
                        LEFT JOIN employeeofficialbusiness ofb
                            ON ofb.OffBusStartDate = ete.Date AND
                                ofb.EmployeeID = ete.EmployeeID AND
                                ofb.Created = latestOb.Created
                        LEFT JOIN employeeovertime ot
                            ON ot.OTStartDate = ete.Date AND
                                ot.EmployeeID = ete.EmployeeID AND
                                ot.OTStatus = 'Approved'
                        LEFT JOIN employeeleave l
                            ON l.LeaveStartDate = ete.Date AND
                                l.EmployeeID = ete.EmployeeID AND
                                l.Status = 'Approved'
                        LEFT JOIN shiftschedules
                            ON shiftschedules.EmployeeID = ete.EmployeeID AND
                                shiftschedules.`Date` = ete.`Date`

                        LEFT JOIN branch
                            ON ete.BranchID = branch.RowID

                        WHERE ete.EmployeeID = " + employeeId + @"  AND
                            ete.`Date` BETWEEN '" + payFromDate.ToString("yyyy-MM-dd") + @"' AND '" + payToDate.ToString("yyyy-MM-dd") + @"'
                        ORDER BY ete.`Date`;";

            System.Data.Common.DbDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dataTable);
            return dataTable;
        }

        public async Task CreateReport(int organizationId, int payPeriodId, int[] employeeIds, bool isActual, string saveFilePath)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);

            if (organization == null)
                throw new BusinessLogicException("Organization does not exists.");

            var payPeriod = _context.PayPeriods
               .Where(p => p.OrganizationID == organizationId)
               .Where(p => p.RowID == payPeriodId).FirstOrDefault();

            var payPeriodDate = payPeriod.PayToDate < new DateTime(payPeriod.PayToDate.Year, payPeriod.PayToDate.Month, 15) ? new DateTime(payPeriod.PayToDate.Year, payPeriod.PayToDate.Month, 15) : new DateTime(payPeriod.PayToDate.Year, payPeriod.PayToDate.Month, 1).AddMonths(1).AddDays(-1);

            List<Tuple<Employee, DataTable>> dataTables = new List<Tuple<Employee, DataTable>>();

            foreach (var employeeId in employeeIds)
            {
                var timeEntries = GetTimeEntries(employeeId, payPeriod.PayFromDate, payPeriod.PayToDate);
                dataTables.Add(new Tuple<Employee, DataTable>(_context.Employees.Find(employeeId), timeEntries));
            }

            var newFile = new FileInfo(saveFilePath);

            using (var excel = new ExcelPackage(newFile))
            {
                foreach(var employee in dataTables)
                {
                    var worksheet = excel.Workbook.Worksheets.Add(employee.Item1.FullName);

                    if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwner.RGI)
                    {
                        worksheet.Protection.IsProtected = true;
                        worksheet.Protection.SetPassword(_listOfValueRepository.GetExcelPassword());
                    }

                    RenderWorksheet(worksheet, employee, payPeriod.PayFromDate, payPeriod.PayToDate, payPeriodDate);
                }

                excel.Save();
            }
        }

        private void RenderWorksheet(ExcelWorksheet worksheet, Tuple<Employee, DataTable> employee, DateTime payFromDate, DateTime payToDate, DateTime payPeriodDate)
        {

            worksheet.Cells["B1:M33"].Style.Border.BorderAround(ExcelBorderStyle.Thick);

            #region Header

            #region Title
            worksheet.Cells["B1:M1"].Merge = true;
            worksheet.Cells["B1:M1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B1:M1"].Style.Font.Size = 19;
            var titleCell = worksheet.Cells[1, 2];
            titleCell.Value = "CERTIFICATION OF DATE TIME RECORD";
            titleCell.Style.Font.Bold = true;
            #endregion

            #region Employee Name
            worksheet.Cells["C2:D2"].Merge = true;
            worksheet.Cells["C2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            var employeeNameCell = worksheet.Cells[2, 3];
            employeeNameCell.Value = "EMPLOYEE NAME:";
            employeeNameCell.Style.Font.Bold = true;

            worksheet.Cells["E2:F2"].Merge = true;
            worksheet.Cells["E2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            var employeeNameValueCell = worksheet.Cells[2, 5];
            employeeNameValueCell.Value = employee.Item1.FullName;
            employeeNameValueCell.Style.Font.Bold = true;
            #endregion

            #region Covered Period
            worksheet.Cells["H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            var coveredPayPeriod = worksheet.Cells[2, 8];
            coveredPayPeriod.Value = "COVERED PERIOD:";
            coveredPayPeriod.Style.Font.Bold = true;

            worksheet.Cells["I2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            var coveredPayPeriodFromCell = worksheet.Cells[2, 9];
            coveredPayPeriodFromCell.Value = payFromDate.ToString("MMMM d, yyyy");
            coveredPayPeriodFromCell.Style.Font.Bold = true;

            worksheet.Cells["J2:K2"].Merge = true;
            worksheet.Cells["J2:K2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            var coveredPayPeriodToCell = worksheet.Cells[2, 10];
            coveredPayPeriodToCell.Value = payToDate.ToString("MMMM d, yyyy");
            coveredPayPeriodToCell.Style.Font.Bold = true;
            #endregion

            #region Employement Status
            worksheet.Cells["C3:D3"].Merge = true;
            worksheet.Cells["C3:D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            var employementStatusCell = worksheet.Cells[3, 3];
            employementStatusCell.Value = "EMPLOYEMENT STATUS:";
            employementStatusCell.Style.Font.Bold = true;

            worksheet.Cells["E3:F3"].Merge = true;
            worksheet.Cells["E3:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            var employementStatusValueCell = worksheet.Cells[3, 5];
            employementStatusValueCell.Value = employee.Item1.EmploymentStatus;
            employementStatusValueCell.Style.Font.Bold = true;
            #endregion

            #region Payout Date
            worksheet.Cells["H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            var payoutDateCell = worksheet.Cells[3, 8];
            payoutDateCell.Value = "PAYOUT DATE:";
            payoutDateCell.Style.Font.Bold = true;

            worksheet.Cells["I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            var payoutDateValueCell = worksheet.Cells[3, 9];
            payoutDateValueCell.Value = payPeriodDate.ToString("MMMM d, yyyy");
            payoutDateValueCell.Style.Font.Bold = true;
            #endregion

            #endregion

            #region Table Body
            var tableRange = worksheet.Cells["C5:L22"];

            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            tableRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            #region No. Column
            worksheet.Cells["C5:C6"].Merge = true;
            var columnNoTitleCell = worksheet.Cells[5, 3];
            columnNoTitleCell.Value = "NO.";
            columnNoTitleCell.Style.Font.Bold = true;

            for(int i =7; i <=22; i++)
            {
                worksheet.Cells[i, 3].Value = i - 6;
                worksheet.Cells[i, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            #endregion

            #region Date Column
            worksheet.Cells["D5:D6"].Merge = true;
            var columnDateTitleCell = worksheet.Cells[5, 4];
            columnDateTitleCell.Value = "DATE";
            columnDateTitleCell.Style.Font.Bold = true;
            #endregion

            #region Check In Column
            worksheet.Cells["E5:E6"].Merge = true;
            var columnCheckInTitleCell = worksheet.Cells[5, 5];
            columnCheckInTitleCell.Value = "CHECK IN";
            columnCheckInTitleCell.Style.Font.Bold = true;
            #endregion

            #region Check Out Column
            worksheet.Cells["F5:F6"].Merge = true;
            var columnCheckOutTitleCell = worksheet.Cells[5, 6];
            columnCheckOutTitleCell.Value = "CHECK Out";
            columnCheckOutTitleCell.Style.Font.Bold = true;
            #endregion

            #region Overtime In Column
            worksheet.Cells["G5:G6"].Merge = true;
            var columnOvertimeInTitleCell = worksheet.Cells[5, 7];
            columnOvertimeInTitleCell.Value = "OVERTIME IN";
            columnOvertimeInTitleCell.Style.Font.Bold = true;
            #endregion

            #region Overtime Out Column
            worksheet.Cells["H5:H6"].Merge = true;
            var columnOvertimeOutTitleCell = worksheet.Cells[5, 8];
            columnOvertimeOutTitleCell.Value = "OVERTIME OUT";
            columnOvertimeOutTitleCell.Style.Font.Bold = true;
            #endregion

            #region Late Column
            worksheet.Cells["I5:I6"].Merge = true;
            var columnLateTitleCell = worksheet.Cells[5, 9];
            columnLateTitleCell.Value = "LATE";
            columnLateTitleCell.Style.Font.Bold = true;
            #endregion

            #region Rendered OT Column
            worksheet.Cells["J5:K5"].Merge = true;
            var columnRenderedOTTitleCell = worksheet.Cells[5, 10];
            columnRenderedOTTitleCell.Value = "RENDERED OT";
            columnRenderedOTTitleCell.Style.Font.Bold = true;

            var columnRenderedOTPremiumTitleCell = worksheet.Cells[6, 10];
            columnRenderedOTPremiumTitleCell.Value = "w/ premium";
            columnRenderedOTPremiumTitleCell.Style.Font.Bold = true;
            columnRenderedOTPremiumTitleCell.Style.WrapText = true;
            columnRenderedOTPremiumTitleCell.Style.Font.Size = 9;

            var columnRenderedOTNonPremiumTitleCell = worksheet.Cells[6, 11];
            columnRenderedOTNonPremiumTitleCell.Value = "w/o premium";
            columnRenderedOTNonPremiumTitleCell.Style.Font.Bold = true;
            columnRenderedOTNonPremiumTitleCell.Style.WrapText = true;
            columnRenderedOTNonPremiumTitleCell.Style.Font.Size = 9;
            #endregion

            #region No. Of Days Column
            worksheet.Cells["L5:L6"].Merge = true;
            var columnNoOfDaysTitleCell = worksheet.Cells[5, 12];
            columnNoOfDaysTitleCell.Value = "NO. OF DAYS";
            columnNoOfDaysTitleCell.Style.Font.Bold = true;
            #endregion

            #region Populate Table

            double lateTotal = 0;
            double withPremiumTotal = 0;
            double withoutPremiumTotal = 0;
            double noOfDaysTotal = 0;
            int row = 7;
            foreach(DataRow data in employee.Item2.Rows)
            {
                worksheet.Cells[row, 4].Value = DateTime.Parse(data["Date"].ToString()).ToString("MMMM dd, yyyy");
                if(data["TimeStampIn"].ToString() != "")
                {
                    worksheet.Cells[row, 5].Value = DateTime.ParseExact(data["TimeIn"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    worksheet.Cells[row, 6].Value = DateTime.ParseExact(data["TimeOut"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    if (data["OvertimeHoursWorked"].ToString() != "0.00")
                    {
                        //worksheet.Cells[row, 7].Value = "Temporary";
                        //worksheet.Cells[row, 8].Value = DateTime.ParseExact(data["TimeOut"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");

                        worksheet.Cells[row, 7].Value = DateTime.ParseExact(data["OTStartTime"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                        worksheet.Cells[row, 8].Value = DateTime.ParseExact(data["OTEndTIme"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");

                        var withPremium = double.Parse(data["OvertimeHoursWorked"].ToString());
                        worksheet.Cells[row, 10].Value = withPremium;
                        withPremiumTotal += withPremium;
                    }

                    var late = double.Parse(data["HoursLate"].ToString()) * 60;
                    worksheet.Cells[row, 9].Value = late;
                    lateTotal += late;

                    var noOfDays = Math.Round((double.Parse(data["TotalHoursWorked"].ToString()) / 4)) / 2;
                    worksheet.Cells[row, 12].Value = noOfDays;
                    noOfDaysTotal += noOfDays;
                }
                else
                {
                    worksheet.Cells[row, 9].Value = 0;
                    worksheet.Cells[row, 12].Value = 0;
                }

                row++;
            }
            worksheet.Cells[23, 9].Value = lateTotal;
            worksheet.Cells[23, 10].Value = withPremiumTotal;
            worksheet.Cells[23, 11].Value = withoutPremiumTotal;
            worksheet.Cells[23, 12].Value = noOfDaysTotal;

            worksheet.Cells["I23:L23"].Style.Font.Color.SetColor(Color.Red);
            worksheet.Cells["I23:L23"].Style.Font.Bold = true;
            worksheet.Cells["I23:L23"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["I23:L23"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            #endregion

            #endregion

            #region Footer
            worksheet.Cells["C25:L25"].Merge = true;
            worksheet.Cells["C25:L25"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells["C25:L25"].Style.Font.Size = 10;
            var footer1Cell = worksheet.Cells[25, 3];
            footer1Cell.Value = "I hereby certify that the information recorded in my Daily Time Record (DTR) for the period mentioned above is true and accurate to the best of my knowledge. I understand that the DTR serves as a record of my actual work hours, breaks, and attendance. I acknowledge that any inaccuracies or discrepancies in my DTR have been discussed, and necessary corrections or adjustments have been made as appropriate.";
            footer1Cell.Style.Font.Italic = true;
            footer1Cell.Style.WrapText = true;

            worksheet.Cells["C26:L26"].Merge = true;
            worksheet.Cells["C26:L26"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells["C26:L26"].Style.Font.Size = 10;
            var footer2Cell = worksheet.Cells[26, 3];
            footer2Cell.Value = "I also understand that the DTR is a legal document used for payroll processing, and I am responsible for the hours and attendance recorded therein. Any intentional falsification or misrepresentation of work hours is subject to disciplinary action in accordance with company policies and applicable labor laws.";
            footer2Cell.Style.Font.Italic = true;
            footer2Cell.Style.WrapText = true;

            worksheet.Cells["C28:E28"].Merge = true;
            worksheet.Cells["C28:E28"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["C29:E29"].Value = "Prepared by";
            worksheet.Cells["C29:E29"].Merge = true;
            worksheet.Cells["C29:E29"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C29:E29"].Style.Font.Bold = true;
            worksheet.Cells["C29:E29"].Style.Font.Italic = true;

            worksheet.Cells["C31:E31"].Merge = true;
            worksheet.Cells["C31:E31"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["C32:E32"].Value = "Employee's Signature";
            worksheet.Cells["C32:E32"].Merge = true;
            worksheet.Cells["C32:E32"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C32:E32"].Style.Font.Bold = true;
            worksheet.Cells["C32:E32"].Style.Font.Italic = true;

            worksheet.Cells["I31:K31"].Merge = true;
            worksheet.Cells["I31:K31"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["I32:K32"].Value = "Approved by Operation Manager";
            worksheet.Cells["I32:K32"].Merge = true;
            worksheet.Cells["I32:K32"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["I32:K32"].Style.Font.Bold = true;
            worksheet.Cells["I32:K32"].Style.Font.Italic = true;
            #endregion

            #region Column Width And Row Height
            worksheet.Column(2).Width = 3.14;
            worksheet.Column(3).Width = 6.14;
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).Width = 15.43;
            worksheet.Column(6).Width = 15.43;
            worksheet.Column(7).Width = 15.43;
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).AutoFit();
            worksheet.Column(10).Width = 8.14;
            worksheet.Column(11).Width = 8.14;
            worksheet.Column(12).Width = 15.43;
            worksheet.Column(13).Width = 3.14;

            worksheet.Row(25).Height = 51;
            worksheet.Row(26).Height = 38.25;
            #endregion
        }
    }
}
