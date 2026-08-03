namespace AccuPay.Web.Approvals.Models
{
    public class EmployeeApproverTokenDto
    {
        public int EmployeeApproverId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeNo { get; set; }

        public int ApproverId { get; set; }

        public string ApproverName { get; set; }

        public string ApproverEmail { get; set; }
    }
}
