using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Reports.Customize;
using AccuPay.Core.ValueObjects;
using AccuPay.Infrastructure.Reports;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccuPay.Core.Entities.Paystub;

namespace AccuPay.Infrastructure.Data.Reports.Customize
{
    public class RGIPayslip : ExcelFormatReport, IRGIPayslip
    {

        private readonly IOrganizationRepository _organizationRepository;
        private readonly ISystemOwnerService _systemOwnerService;
        private readonly IListOfValueRepository _listOfValueRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaystubRepository _paystubRepository;

        protected readonly PayrollContext _context;

        public RGIPayslip(
            IOrganizationRepository organizationRepository,
            ISystemOwnerService systemOwnerService,
            IListOfValueRepository listOfValueRepository,
            IEmployeeRepository employeeRepository,
            IPaystubRepository paystubRepository,
            PayrollContext context) 
        {
            _organizationRepository = organizationRepository;
            _systemOwnerService = systemOwnerService;
            _listOfValueRepository = listOfValueRepository;
            _employeeRepository = employeeRepository;
            _paystubRepository = paystubRepository;
            _context = context;
        }

        public async Task CreateReport(int organizationId, int payPeriodId, int[] employeeIds, bool isActual, string saveFilePath)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);

            if (organization == null)
                throw new BusinessLogicException("Organization does not exists.");

            List<EmployeePayslip> employeePayslips = new List<EmployeePayslip>();

            var timePeriod = GetTimePeriod(organizationId, payPeriodId);


            var payPeriodDate = timePeriod.End < new DateTime(timePeriod.End.Year, timePeriod.End.Month, 15) ? new DateTime(timePeriod.End.Year, timePeriod.End.Month, 15) : new DateTime(timePeriod.End.Year, timePeriod.End.Month, 1).AddMonths(1).AddDays(-1);


            foreach (var employeeId in employeeIds)
            {
                var employee = await _employeeRepository.GetByIdAsync(employeeId);

                var paystub = await GetPayStubAsync(employeeId, payPeriodId);

                
                var salary = await _employeeRepository.GetCurrentSalaryAsync(employee.RowID.Value, timePeriod.End);

                employeePayslips.Add(new EmployeePayslip()
                {
                    employee = employee,
                    salary = salary,
                    paystub = paystub,
                });
            }

            var newFile = new FileInfo(saveFilePath);

            using (var excel = new ExcelPackage(newFile))
            {
                foreach (var employeePayslip in employeePayslips)
                {
                    var worksheet = excel.Workbook.Worksheets.Add(employeePayslip.employee.FullNameLastNameFirst);

                    if (_systemOwnerService.GetCurrentSystemOwner() == SystemOwner.RGI)
                    {
                        worksheet.Protection.IsProtected = true;
                        worksheet.Protection.SetPassword(_listOfValueRepository.GetExcelPassword());
                    }

                    RenderWorksheet(worksheet, employeePayslip, timePeriod, payPeriodDate);
                }

                excel.Save();
            }

        }

        private void RenderWorksheet(ExcelWorksheet worksheet, EmployeePayslip employeePayslip, TimePeriod timePeriod, DateTime payPeriodDate)
        {
            worksheet.Cells["B2:K19"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells["B2:K2"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["B3:K3"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["I4"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["B5:K5"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["B7:H7"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["B9:H9"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["I14:K14"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["I15:K15"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["B18:K18"].Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;


            worksheet.Cells["B10:B18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["C10:C18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["D8:D18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["E10:E18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["F8:F18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["G10:G18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;
            worksheet.Cells["H4:H18"].Style.Border.Right.Style = ExcelBorderStyle.Dotted;


            worksheet.Cells["B9:H9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            decimal adjustments = 0;
            decimal deductions = 0;

            #region Column Width And Row Height

            worksheet.Column(1).Width = 8.43;
            worksheet.Column(2).Width = 9.57;
            worksheet.Column(3).Width = 9.86;
            worksheet.Column(4).Width = 9.14;
            worksheet.Column(5).Width = 16.57;
            worksheet.Column(6).Width = 14;
            worksheet.Column(7).Width = 18.14;
            worksheet.Column(8).Width = 15.14;
            worksheet.Column(9).Width = 18.14;
            worksheet.Column(10).Width = 18.14;
            worksheet.Column(11).Width = 8.86;

            worksheet.Row(3).Height = 23.25;

            #endregion

            #region Title

            worksheet.Cells["F3"].Style.Font.Size = 16;
            var titleCell = worksheet.Cells["F3"];
            titleCell.Value = "ROCKET GLOBAL INC.";
            titleCell.Style.Font.Bold = true;

            #endregion

            #region Payslip Title

            var payslipTitleCell = worksheet.Cells["B4"];
            payslipTitleCell.Value = "PAYSLIP - SEMI-MONTHLY PAYROLL";
            payslipTitleCell.Style.Font.Bold = true;
            payslipTitleCell.Style.Font.Size = 8;

            #endregion

            #region Payslip Period

            var payslipPeriodCell = worksheet.Cells["F4"];
            payslipPeriodCell.Value = "PERIOD :";
            payslipPeriodCell.Style.Font.Size = 8;

            #endregion

            #region Payslip Period Value From

            var payslipPeriodFromValue = worksheet.Cells["G4"];
            payslipPeriodFromValue.Value = timePeriod.Start.ToString("MMMM dd, yyyy");
            payslipPeriodFromValue.Style.Font.Size = 8;

            #endregion

            #region Payslip Period Value To

            var payslipPeriodToValue = worksheet.Cells["H4"];
            payslipPeriodToValue.Value = timePeriod.End.ToString("MMMM dd, yyyy");
            payslipPeriodToValue.Style.Font.Size = 8;

            #endregion

            #region Total Number of Days Cell

            var numberOfDaysCell = worksheet.Cells["I4"];
            numberOfDaysCell.Value = "TOTAL NO. OF PRESENT DAYS";
            numberOfDaysCell.Style.Font.Size = 8;
            numberOfDaysCell.Style.Font.Bold = true;
            numberOfDaysCell.Style.Font.UnderLine = true;

            #endregion

            #region Total Number of Days Value

            var numberOfDaysValue = worksheet.Cells["K4"];
            numberOfDaysValue.Value = Math.Round( employeePayslip.paystub.RegularHours / 8, 2);
            numberOfDaysValue.Style.Font.Size = 8;

            #endregion

            #region Payout Cell

            var payoutCell = worksheet.Cells["F5"];
            payoutCell.Value = "PAYOUT :";
            payoutCell.Style.Font.Size = 8;

            #endregion

            #region Payout Value

            var payoutValue = worksheet.Cells["G5"];
            payoutValue.Value = payPeriodDate.ToString("MMMM dd, yyyy");
            payoutValue.Style.Font.Size = 8;
            payoutValue.Style.Font.Bold = true ;

            #endregion

            #region Basic Pay Cell

            var basicPayCell = worksheet.Cells["I5"];
            basicPayCell.Value = "BASIC PAY:";
            basicPayCell.Style.Font.Size = 8;
            basicPayCell.Style.Font.Bold = true;

            #endregion

            #region Basic Pay Value

            var basicPayValue = worksheet.Cells["K5"];
            basicPayValue.Value = Math.Round(employeePayslip.paystub.RegularPay, 2);
            basicPayValue.Style.Font.Size = 8;
            basicPayValue.Style.Font.Bold = true;

            #endregion

            #region Employee Cell

            var employeeCell = worksheet.Cells["B6"];
            employeeCell.Value = "EMPLOYEE: ";
            employeeCell.Style.Font.Size = 8;

            #endregion

            #region Employee Value

            var employeeValue = worksheet.Cells["C6"];
            employeeValue.Value = employeePayslip.employee.FullNameLastNameFirst.ToUpper();
            employeeValue.Style.Font.Size = 8;
            employeeValue.Style.Font.Bold = true;

            #endregion

            #region Status Cell

            var statusCell = worksheet.Cells["F6"];
            statusCell.Value = "STATUS:";
            statusCell.Style.Font.Size = 8;

            #endregion

            #region Status Value

            var statusValue = worksheet.Cells["G6"];
            statusValue.Value = employeePayslip.employee.EmploymentStatus.ToUpper();
            statusValue.Style.Font.Size = 8;
            statusValue.Style.Font.Bold = true;

            #endregion

            #region Position Cell

            var positionCell = worksheet.Cells["B7"];
            positionCell.Value = "POSITION:";
            positionCell.Style.Font.Size = 8;

            #endregion

            #region Position Value

            var positionValue = worksheet.Cells["C7"];
            positionValue.Value = employeePayslip.employee.Position.Name.ToUpper();
            positionValue.Style.Font.Size = 8;
            positionValue.Style.Font.Bold = true;

            #endregion

            #region Overtime Cell

            var overtimeCell = worksheet.Cells["I7"];
            overtimeCell.Value = "OVERTIME:";
            overtimeCell.Style.Font.Size = 8;

            #endregion

            #region Overtime Value

            var overtimeValue = worksheet.Cells["K7"];
            overtimeValue.Value = Math.Round(employeePayslip.paystub.OvertimePay,2);
            overtimeValue.Style.Font.Size = 8;

            #endregion

            #region Overtime Column Cell

            var overtimeColumnCell = worksheet.Cells["B9"];
            overtimeColumnCell.Value = "OVERTIME";
            overtimeColumnCell.Style.Font.Size = 8;

            #endregion

            #region Min Column Cell

            var minColumnCell = worksheet.Cells["C9"];
            minColumnCell.Value = "MIN";
            minColumnCell.Style.Font.Size = 8;

            #endregion

            #region Pay Column Cell

            var payColumnCell = worksheet.Cells["D9"];
            minColumnCell.Value = "PAY";
            minColumnCell.Style.Font.Size = 8;

            #endregion

            #region Adjustments + Column Cell

            var adjustmentsColumnCell = worksheet.Cells["E9"];
            adjustmentsColumnCell.Value = "ADJUSTMENTS";
            adjustmentsColumnCell.Style.Font.Size = 8;

            #endregion

            #region Adjustments Amount Column Cell

            var adjustmentsAmountColumnCell = worksheet.Cells["F9"];
            adjustmentsAmountColumnCell.Value = "AMOUNT";
            adjustmentsAmountColumnCell.Style.Font.Size = 8;

            #endregion

            #region Deduction Column Cell

            var deductionColumnCell = worksheet.Cells["G9"];
            deductionColumnCell.Value = "DEDUCTION";
            deductionColumnCell.Style.Font.Size = 8;

            #endregion

            #region Deduction Amount Column Cell

            var deductionAmountColumnCell = worksheet.Cells["H9"];
            deductionAmountColumnCell.Value = "AMOUNT";
            deductionAmountColumnCell.Style.Font.Size = 8;

            #endregion

            #region Overtime Column Value

            var overtimeColumnValue = worksheet.Cells["B10"];
            overtimeColumnValue.Value = Math.Round(employeePayslip.paystub.TotalOvertimeHours,2);
            overtimeColumnValue.Style.Font.Size = 8;

            #endregion

            #region Pay Column Value

            var payColumnValue = worksheet.Cells["D10"];
            payColumnValue.Value = Math.Round(employeePayslip.paystub.OvertimePay, 2);
            payColumnValue.Style.Font.Size = 8;

            #endregion

            #region Adjustments + Column

            var positiveAdjustmentsColumnCell = worksheet.Cells["E10"];
            positiveAdjustmentsColumnCell.Value = "ADJUSTMENT (+)";
            positiveAdjustmentsColumnCell.Style.Font.Size = 8;

            #endregion

            #region Adjustments + Column Value

            var positiveAdjustmentsColumnValue = worksheet.Cells["F10"];
            positiveAdjustmentsColumnValue.Value = employeePayslip.paystub.TotalAdjustments > 0 ? Math.Round(employeePayslip.paystub.TotalAdjustments, 2)  : 0;
            adjustments += employeePayslip.paystub.TotalAdjustments > 0 ? employeePayslip.paystub.TotalAdjustments : 0;
            positiveAdjustmentsColumnValue.Style.Font.Size = 8;

            #endregion

            #region Adjustments - Column

            var negativeAdjustmentsColumnCell= worksheet.Cells["G10"];
            negativeAdjustmentsColumnCell.Value = "ADJUSTMENT (-)";
            negativeAdjustmentsColumnCell.Style.Font.Size = 8;

            #endregion

            #region Adjustments - Column Value

            var negativeAdjustmentsColumnValue = worksheet.Cells["H10"];
            negativeAdjustmentsColumnValue.Value = employeePayslip.paystub.TotalAdjustments < 0 ? Math.Round(employeePayslip.paystub.TotalAdjustments, 2) : 0;
            deductions += employeePayslip.paystub.TotalAdjustments < 0 ? employeePayslip.paystub.TotalAdjustments : 0;
            negativeAdjustmentsColumnValue.Style.Font.Size = 8;

            #endregion

            #region Gross Pay Cell

            var grossPayColumnCell = worksheet.Cells["I10"];
            grossPayColumnCell.Value = "GROSS PAY:";
            grossPayColumnCell.Style.Font.Size = 8;

            #endregion

            #region Gross Pay Value

            var grossPayColumnValue = worksheet.Cells["K10"];
            grossPayColumnValue.Value = Math.Round(employeePayslip.paystub.GrossPay,2);
            grossPayColumnValue.Style.Font.Size = 8;

            #endregion

            #region Paid VL Cell

            var paidVLCell = worksheet.Cells["E11"];
            paidVLCell.Value = "PAID VL";
            paidVLCell.Style.Font.Size = 8;

            #endregion

            #region Paid VL Cell

            var paidVLValue = worksheet.Cells["F11"];
            paidVLValue.Value = Math.Round(employeePayslip.paystub.LeavePay, 2);
            adjustments +=employeePayslip.paystub.LeavePay;
            paidVLValue.Style.Font.Size = 8;

            #endregion

            #region SSS Cell

            var sssCell = worksheet.Cells["G11"];
            sssCell.Value = "SSS";
            sssCell.Style.Font.Size = 8;

            #endregion

            #region SSS Value

            var sssValue = worksheet.Cells["H11"];
            sssValue.Value = Math.Round(employeePayslip.paystub.SssEmployeeShare, 2);
            deductions += employeePayslip.paystub.SssEmployeeShare;
            sssValue.Style.Font.Size = 8;

            #endregion

            #region Paid SL Cell

            var paidSLCell = worksheet.Cells["E12"];
            paidSLCell.Value = "PAID SL";
            paidSLCell.Style.Font.Size = 8;

            #endregion

            #region Paid SL Value

            var paidSLValue = worksheet.Cells["F12"];
            paidSLValue.Value = 0.00;
            paidSLValue.Style.Font.Size = 8;

            #endregion

            #region PhilHealth Cell

            var philHealthCell = worksheet.Cells["G12"];
            philHealthCell.Value = "PHILHEALTH";
            philHealthCell.Style.Font.Size = 8;

            #endregion

            #region PhilHealth Value

            var philHealthValue = worksheet.Cells["H12"];
            philHealthValue.Value = Math.Round(employeePayslip.paystub.PhilHealthEmployeeShare,2);
            deductions += employeePayslip.paystub.PhilHealthEmployeeShare;
            philHealthValue.Style.Font.Size = 8;

            #endregion

            #region Regular H. Pay Cell

            var regularHolidayPayCell = worksheet.Cells["E13"];
            regularHolidayPayCell.Value = "R. HOLIDAY PAY";
            regularHolidayPayCell.Style.Font.Size = 8;

            #endregion

            #region Regular H. Pay Value

            var regularHolidayPayValue = worksheet.Cells["F13"];
            regularHolidayPayValue.Value = Math.Round(employeePayslip.paystub.RegularHolidayPay, 2);
            adjustments += employeePayslip.paystub.RegularHolidayPay;
            regularHolidayPayValue.Style.Font.Size = 8;

            #endregion

            #region PagIbig Cell

            var pagIbigCell = worksheet.Cells["G13"];
            pagIbigCell.Value = "PAG-IBIG";
            pagIbigCell.Style.Font.Size = 8;

            #endregion

            #region PagIbig Value

            var pagIbigValue = worksheet.Cells["H13"];
            pagIbigValue.Value = Math.Round(employeePayslip.paystub.HdmfEmployeeShare, 2);
            deductions += employeePayslip.paystub.HdmfEmployeeShare;
            pagIbigValue.Style.Font.Size = 8;

            #endregion

            #region S. H. Pay Cell

            var specialHolidayPayCell = worksheet.Cells["E14"];
            specialHolidayPayCell.Value = "S. HOLIDAY (30%)";
            specialHolidayPayCell.Style.Font.Size = 8;

            #endregion

            #region S. H. Pay Value

            var specialHolidayPayValue = worksheet.Cells["F14"];
            specialHolidayPayValue.Value = Math.Round(employeePayslip.paystub.SpecialHolidayPay, 2);
            adjustments += employeePayslip.paystub.SpecialHolidayPay;
            specialHolidayPayValue.Style.Font.Size = 8;

            #endregion

            #region Tardiness Cell

            var tardinessCell = worksheet.Cells["G14"];
            tardinessCell.Value = "TARDINESS";
            tardinessCell.Style.Font.Size = 8;

            #endregion

            #region Tardiness Value

            var tardinessValue = worksheet.Cells["H14"];
            tardinessValue.Value = Math.Round(employeePayslip.paystub.LateDeduction, 2);
            deductions += employeePayslip.paystub.LateDeduction;
            tardinessValue.Style.Font.Size = 8;

            #endregion

            #region S. Rate Cell

            var sRateCell = worksheet.Cells["E15"];
            sRateCell.Value = "S. RATE (30%)";
            sRateCell.Style.Font.Size = 8;

            #endregion

            #region S. Rate Value

            var sRateValue = worksheet.Cells["F15"];
            tardinessValue.Value = 0.00;
            deductions += 0;
            tardinessValue.Style.Font.Size = 8;

            #endregion

            #region Employee Deductibles Cell

            var employeeDeductiblesCell = worksheet.Cells["G15"];
            employeeDeductiblesCell.Value = "EMPLOYEE DEDUCTIBLES";
            employeeDeductiblesCell.Style.Font.Size = 8;

            #endregion

            #region Employee Deductibles Value

            var employeeDeductiblesValue = worksheet.Cells["H15"];
            employeeDeductiblesValue.Value = 0.00;
            deductions += 0;
            employeeDeductiblesValue.Style.Font.Size = 8;

            #endregion

            #region NIGHT DIFFERENTIAL Cell

            var nightDifferentialCell = worksheet.Cells["E16"];
            nightDifferentialCell.Value = "NIGHT DIFFERENTIAL";
            nightDifferentialCell.Style.Font.Size = 8;

            #endregion

            #region NIGHT DIFFERENTIAL Value

            var nightDifferentialValue = worksheet.Cells["F16"];
            nightDifferentialValue.Value = Math.Round(employeePayslip.paystub.NightDiffPay,2);
            adjustments += employeePayslip.paystub.NightDiffPay;
            nightDifferentialValue.Style.Font.Size = 8;

            #endregion

            #region Barracks Cell

            var barracksCell = worksheet.Cells["G16"];
            barracksCell.Value = "BARRACKS";
            barracksCell.Style.Font.Size = 8;

            #endregion

            #region Barracks Value

            var barracksValue = worksheet.Cells["H16"];
            barracksValue.Value = 0.00;
            deductions += 0;
            barracksValue.Style.Font.Size = 8;

            #endregion

            #region Allowance Cell

            var allawanceCell = worksheet.Cells["E17"];
            allawanceCell.Value = "ALLOWANCE";
            allawanceCell.Style.Font.Size = 8;

            #endregion

            #region Allowance Value

            var allawanceValue = worksheet.Cells["F17"];
            allawanceValue.Value = Math.Round(employeePayslip.paystub.GrandTotalAllowance ,2);
            adjustments += employeePayslip.paystub.GrandTotalAllowance;
            allawanceValue.Style.Font.Size = 8;

            #endregion

            #region Salary Loans Cell

            var salaryLoansCell = worksheet.Cells["G17"];
            salaryLoansCell.Value = "SALARY LOANS";
            salaryLoansCell.Style.Font.Size = 8;

            #endregion

            #region Salary Loans Value

            var salaryLoansValue = worksheet.Cells["H17"];
            salaryLoansValue.Value = Math.Round(employeePayslip.paystub.TotalLoans, 2);
            deductions += employeePayslip.paystub.TotalLoans;
            salaryLoansValue.Style.Font.Size = 8;

            #endregion

            #region Pay Total Value

            var payTotalValue = worksheet.Cells["D18"];
            payTotalValue.Value = Math.Round(employeePayslip.paystub.OvertimePay,2);
            payTotalValue.Style.Font.Size = 8;

            #endregion

            #region Adjustment Total Value

            var adjustmentTotalValue = worksheet.Cells["F18"];
            adjustmentTotalValue.Value = Math.Round(adjustments);
            adjustmentTotalValue.Style.Font.Size = 8;

            #endregion

            #region Deduction Total Value

            var deductionTotalValue = worksheet.Cells["H18"];
            deductionTotalValue.Value = Math.Round(deductions);
            deductionTotalValue.Style.Font.Size = 8;

            #endregion

            #region Adjustment Cell

            var adjustmentCell = worksheet.Cells["I8"];
            adjustmentCell.Value = "ADJUSTMENT:";
            adjustmentCell.Style.Font.Size = 8;

            #endregion

            #region Adjustment Value

            var adjustmentValue = worksheet.Cells["K8"];
            adjustmentValue.Value = Math.Round(adjustments);
            adjustmentValue.Style.Font.Size = 8;

            #endregion

            #region Deductions Cell

            var deductionsCell = worksheet.Cells["I11"];
            deductionsCell.Value = "DEDUCTION :";
            deductionsCell.Style.Font.Size = 8;

            #endregion

            #region Deductions Value

            var deductionsValue = worksheet.Cells["K11"];
            deductionsValue.Value = Math.Round(deductions);
            deductionsValue.Style.Font.Size = 8;

            #endregion

            #region Net Pay Cell

            var netPayCell = worksheet.Cells["I13"];
            netPayCell.Value = "NET PAY :";
            netPayCell.Style.Font.Size = 8;
            netPayCell.Style.Font.Bold = true;

            #endregion

            #region Net Pay Value

            var netPayValue = worksheet.Cells["K13"];
            netPayValue.Value = Math.Round(employeePayslip.paystub.NetPay,2);
            netPayValue.Style.Font.Size = 8;
            netPayValue.Style.Font.Bold = true;

            #endregion

            #region Signature Cell

            var signatureCell = worksheet.Cells["I15"];
            signatureCell.Value = "SIGNATURE:";
            signatureCell.Style.Font.Size = 10;
            signatureCell.Style.Font.Bold = true;

            #endregion
        }

        private TimePeriod GetTimePeriod(int organizationId, int payPeriodId)
        {
            var payPeriod = _context.PayPeriods
               .Where(p => p.OrganizationID == organizationId)
               .Where(p => p.RowID == payPeriodId).FirstOrDefault();

            return new TimePeriod(start: payPeriod.PayFromDate, end: payPeriod.PayToDate);
        }

        private async Task<Paystub> GetPayStubAsync(int employeeId, int payPeriodId)
        {
            var query = _context.Paystubs
                .AsNoTracking()
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Actual)
                .AsQueryable();

            query = query
               .Where(x => x.EmployeeID == employeeId)
               .Where(x => x.PayPeriodID == payPeriodId);

            return await query.FirstOrDefaultAsync();
        }

        private class EmployeePayslip
        {
            public Employee employee;
            public Salary salary;
            public Paystub paystub;

        }
    }

    
}
