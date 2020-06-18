using AccuPay.Data.Entities;

namespace AccuPay.Data.Services
{
    public class PaystubData
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PayPeriodId { get; set; }
        public decimal BasicRate { get; set; }
        public Paystub Paystub { get; set; }

        public PaystubData(Paystub paystub, Salary currentSalary)
        {
            if (paystub?.RowID == null || paystub?.Employee?.RowID == null) return;

            Paystub = paystub;
            BasicRate = paystub.Employee.IsDaily ? currentSalary.BasicSalary : currentSalary.BasicSalary / 2;

            Id = Paystub.RowID.Value;
            EmployeeId = Paystub.Employee.RowID.Value;
        }
    }
}