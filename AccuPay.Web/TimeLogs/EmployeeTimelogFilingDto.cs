using AccuPay.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Web.TimeLogs
{
    public class EmployeeTimelogFilingDto
    {
        public int Id { get; set; }
        public string EntryType { get; set; }

        public DateTime LogDate { get; set; }

        public virtual EmployeeDto Employee { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string ApproverEmail { get; set; }

        public string TimeStamp { get; set; }

        public bool IsNotifyEmail { get; set; }
        public DateTime? NotifyEmailSentAt { get; set; }
        public class EmployeeDto
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }

    }
}
