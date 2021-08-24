using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class BenchmarkAlphalistReportDataService : IBenchmarkAlphalistReportDataService
    {
        private readonly PayrollContext _context;
        private readonly ISalaryRepository _salaryRepository;

        public BenchmarkAlphalistReportDataService(PayrollContext context, ISalaryRepository salaryRepository)
        {
            _context = context;
            _salaryRepository = salaryRepository;
        }

        public async Task<DataTable> GetData(int organizationId, int year)
        {
            DataTable data = new DataTable();
            data.Columns.Add("DatCol1"); // TIN
            data.Columns.Add("DatCol2"); // Employee Name
            data.Columns.Add("DatCol3"); // Basic
            data.Columns.Add("DatCol4"); // 13th month
            data.Columns.Add("DatCol5"); // Overtime
            data.Columns.Add("DatCol6"); // Allowance
            data.Columns.Add("DatCol7"); // Bonus
            data.Columns.Add("DatCol8"); // Gross Pay
            data.Columns.Add("DatCol9"); // SSS
            data.Columns.Add("DatCol10"); // PhilHealth
            data.Columns.Add("DatCol11"); // HDMF
            data.Columns.Add("DatCol12"); // Total Deduction
            data.Columns.Add("DatCol13"); // Net Pay

            var allPaystubs = await GetPaystubs(
                organizationId: organizationId,
                year: year);
            var allPayPeriodSalaries = await PayPeriodSalary.Fetch(
                _context,
                _salaryRepository,
                organizationId: organizationId,
                year: year);

            var employeePaystubs = allPaystubs
                .GroupBy(p => p.Employee)
                .OrderBy(e => e.Key.LastName)
                .ThenBy(e => e.Key.FirstName);

            foreach (var item in employeePaystubs)
            {
                var employee = item.Key;
                var paystubs = item.ToList();

                var paystubPayPeriods = paystubs.GroupBy(p => p.PayPeriod);

                decimal basicPay = 0;

                foreach (var paystub in paystubs)
                {
                    var payPeriodSalaries = allPayPeriodSalaries
                        .Where(p => p.PayPeriod.RowID.Value == paystub.PayPeriod.RowID.Value)
                        .FirstOrDefault();

                    var salary = payPeriodSalaries.Salaries
                        .FirstOrDefault(s => s.EmployeeID.Value == employee.RowID.Value);

                    basicPay += ComputeBasicPay(employee, (salary?.BasicSalary ?? 0), paystub.BasicPay);
                }

                var alphalistData = new BenchmarkAlphalistData(
                    tinNumber: employee.TinNo,
                    employeeName: $"{employee.LastName}, {employee.FirstName}",
                    basicPay: basicPay,
                    thirteenthMonthAmount: paystubs.Sum(p => p.ThirteenthMonthPay.Amount),
                    overtime: paystubs.Sum(p => p.AdditionalPay),
                    grossPay: paystubs.Sum(p => p.GrossPay),
                    sSSAmount: paystubs.Sum(p => p.SssEmployeeShare),
                    philhealthAmount: paystubs.Sum(p => p.PhilHealthEmployeeShare),
                    hDMFAmount: paystubs.Sum(p => p.HdmfEmployeeShare),
                    netpay: paystubs.Sum(p => p.NetPay));

                data.Rows.Add(alphalistData.ToDataRow(data));
            }

            return data;
        }

        private async Task<List<Paystub>> GetPaystubs(int organizationId, int year)
        {
            return await _context.Paystubs
                .Include(p => p.PayPeriod)
                .Include(p => p.Employee)
                .Include(p => p.ThirteenthMonthPay)
                .Where(p => p.PayPeriod.Year == year)
                .Where(p => p.OrganizationID.Value == organizationId)
                .ToListAsync();
        }

        /// <summary>
        /// This is same as PaystubPayslipModel's ComputeBasicPay. They should be merged.
        /// </summary>
        /// <param name="salary"></param>
        /// <param name="workHours"></param>
        /// <returns></returns>

        private decimal ComputeBasicPay(Employee employee, decimal salary, decimal workHours)
        {
            if (employee.IsMonthly || employee.IsFixed)
            {
                if (employee.PayFrequencyID.Value == (int)PayFrequencyType.Monthly)
                    return salary;
                else if (employee.PayFrequencyID.Value == (int)PayFrequencyType.SemiMonthly)
                    return salary / PayrollTools.SemiMonthlyPayPeriodsPerMonth;
                else
                    throw new Exception("GetBasicPay is implemented on monthly and semimonthly only");
            }
            else if (employee.IsDaily)
                return workHours * (salary / PayrollTools.WorkHoursPerDay);

            return 0;
        }

        public class BenchmarkAlphalistData
        {
            public BenchmarkAlphalistData(string tinNumber, string employeeName, decimal basicPay, decimal thirteenthMonthAmount, decimal overtime, decimal grossPay, decimal sSSAmount, decimal philhealthAmount, decimal hDMFAmount, decimal netpay)
            {
                this.TinNumber = tinNumber;
                this.EmployeeName = employeeName;
                this.BasicPay = basicPay;
                this.ThirteenthMonthAmount = thirteenthMonthAmount;
                this.Overtime = overtime;
                this.GrossPay = grossPay;
                this.SSSAmount = sSSAmount;
                this.PhilhealthAmount = philhealthAmount;
                this.HDMFAmount = hDMFAmount;
                this.Netpay = netpay;
            }

            public string TinNumber { get; set; }
            public string EmployeeName { get; set; }
            public decimal BasicPay { get; set; }
            public decimal ThirteenthMonthAmount { get; set; }

            public decimal Overtime { get; set; }

            public decimal Allowance => 0; // Will be added manually by maam Mely

            public decimal Bonus => 0; // Will be added manually by maam Mely

            public decimal GrossPay { get; set; }
            public decimal SSSAmount { get; set; }
            public decimal PhilhealthAmount { get; set; }
            public decimal HDMFAmount { get; set; }

            public decimal TotalDeduction => SSSAmount + PhilhealthAmount + HDMFAmount;

            public decimal Netpay { get; set; }

            public DataRow ToDataRow(DataTable data)
            {
                var newRow = data.NewRow();

                newRow["DatCol1"] = this.TinNumber; // TIN
                newRow["DatCol2"] = this.EmployeeName; // Employee Name
                newRow["DatCol3"] = FormatNumber(this.BasicPay); // Basic
                newRow["DatCol4"] = FormatNumber(this.ThirteenthMonthAmount); // 13th month
                newRow["DatCol5"] = FormatNumber(this.Overtime); // Overtime
                newRow["DatCol6"] = FormatNumber(this.Allowance); // Allowance
                newRow["DatCol7"] = FormatNumber(this.Bonus); // Bonus
                newRow["DatCol8"] = FormatNumber(this.GrossPay); // Gross Pay
                newRow["DatCol9"] = FormatNumber(this.SSSAmount); // SSS
                newRow["DatCol10"] = FormatNumber(this.PhilhealthAmount); // PhilHealth
                newRow["DatCol11"] = FormatNumber(this.HDMFAmount); // HDMF
                newRow["DatCol12"] = FormatNumber(this.TotalDeduction); // Total Deduction
                newRow["DatCol13"] = FormatNumber(this.Netpay); // Net Pay

                return newRow;
            }

            public string FormatNumber(decimal number)
            {
                if (number == 0)
                    return string.Empty;

                return number.RoundToString(3);
            }
        }

        public class PayPeriodSalary
        {
            public PayPeriodSalary(PayPeriod payPeriod, ICollection<Salary> salaries)
            {
                this.PayPeriod = payPeriod;
                this.Salaries = salaries;
            }

            public PayPeriod PayPeriod { get; set; }

            public ICollection<Salary> Salaries { get; set; }

            public static async Task<List<PayPeriodSalary>> Fetch(
                PayrollContext context,
                ISalaryRepository salaryRepository,
                int organizationId,
                int year)
            {
                List<PayPeriod> payPeriods;

                payPeriods = await context.PayPeriods
                    .Where(p => p.Year == year)
                    .Where(p => p.OrganizationID.Value == organizationId)
                    .Where(p => p.IsSemiMonthly)
                    .ToListAsync();

                var payPeriodSalaries = new List<PayPeriodSalary>();

                foreach (var period in payPeriods)
                {
                    var salaries = await salaryRepository.GetByCutOffAsync(organizationId, period.PayToDate);

                    payPeriodSalaries.Add(new PayPeriodSalary(period, salaries));
                }

                return payPeriodSalaries;
            }
        }
    }
}
