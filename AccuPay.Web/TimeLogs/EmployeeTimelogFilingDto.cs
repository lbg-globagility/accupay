using AccuPay.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Web.TimeLogs
{
    public class EmployeeTimelogFilingDto
    {
        public int Id { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime? CheckIn { get; set; }

        public DateTime? LunchOut { get; set; }

        public DateTime? LunchIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public virtual EmployeeDto Employee { get; set; }

        public int EmployeeID { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string DecidedBy { get; set; }

        public bool IsNotifyEmail { get; set; }

        public DateTime? NotifyEmailSentAt { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? CreatedBy { get; set; }

        public int? LastUpdBy { get; set; }
        public class EmployeeDto
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }

    }
}
