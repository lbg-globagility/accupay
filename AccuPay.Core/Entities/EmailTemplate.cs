using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("emailtemplate")]
    public class EmailTemplate : OrganizationalEntity
    {
        public const string TimeLogFilingApprovalCode = "TimeLogFilingApproval";

        public string Code { get; set; }

        public string Subject { get; set; }

        public string HtmlBody { get; set; }

        public string TextBody { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
