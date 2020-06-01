namespace AccuPay.Web.Payroll
{
    public class PaystubDto
    {
        public int? Id { get; set; }

        public int PayperiodId { get; set; }

        public decimal NetPay { get; set; }

        public EmployeeDto Employee { get; set; }

        public PaystubDto()
        {
            Employee = new EmployeeDto();
        }

        public class EmployeeDto
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }
        }
    }
}
