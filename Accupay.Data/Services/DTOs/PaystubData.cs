using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;

namespace AccuPay.Data.Services
{
    public class PaystubData
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PayPeriodId { get; set; }
        public decimal BasicRate { get; set; }
        public decimal DailyRate { get; set; }
        public decimal HourlyRate { get; set; }
        public Paystub Paystub { get; set; }
        public Salary Salary { get; set; }

        public PaystubData(Paystub paystub, Salary currentSalary)
        {
            if (paystub?.RowID == null || paystub?.Employee?.RowID == null) return;

            Paystub = paystub;
            Salary = currentSalary;

            decimal basicSalary = currentSalary?.BasicSalary ?? 0;
            BasicRate = paystub.Employee.IsDaily ? basicSalary : basicSalary / 2;

            DailyRate = PayrollTools.GetDailyRate(currentSalary, paystub.Employee);
            HourlyRate = PayrollTools.GetHourlyRateByDailyRate(DailyRate);

            Id = Paystub.RowID.Value;
            EmployeeId = Paystub.Employee.RowID.Value;
        }
    }
}