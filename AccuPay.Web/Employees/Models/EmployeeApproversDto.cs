using AccuPay.Core.Entities;
using AccuPay.Web.Appraisers;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace AccuPay.Web.Employees.Models
{
    public class EmployeeApproversDto
    {
        public int Id { get; set; }

        public int ApproverID { get; set; }

        public ApproverDto Approver { get; set; }

    }
}
