using AccuPay.Core.Entities.LeaveReset;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("cashoutleave")]
    public class CashoutUnusedLeave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; internal set; }

        public int OrganizationID { get; internal set; }
        public int EmployeeID { get; internal set; }
        public int LeaveLedgerID { get; internal set; }
        public int? PaystubID { get; set; }
        public int PayPeriodID { get; internal set; }
        public decimal LeaveHours { get; set; }
        public decimal Amount { get; set; }
        public virtual Employee Employee { get; internal set; }
        public virtual LeaveLedger LeaveLedger { get; internal set; }
        public virtual Paystub Paystub { get; internal set; }
        public virtual PayPeriod PayPeriod { get; internal set; }
        public bool IsVacation => LeaveLedger != null ? LeaveLedger?.Product?.PartNo == LeaveTypeEnum.Vacation.Type : false;
        public bool IsSick => LeaveLedger != null ? LeaveLedger?.Product?.PartNo == LeaveTypeEnum.Sick.Type : false;
        public bool IsOthers => LeaveLedger != null ? LeaveLedger?.Product?.PartNo == LeaveTypeEnum.Others.Type : false;
        public bool IsParental => LeaveLedger != null ? LeaveLedger?.Product?.PartNo == LeaveTypeEnum.Parental.Type : false;
        public static CashoutUnusedLeave NewCashoutUnusedLeave(int organizationId,
            int employeeId,
            int leaveLedgerId,
            int? paystubId,
            int payPeriodId,
            decimal leaveHours,
            decimal amount)
        {
            return new CashoutUnusedLeave() {
                OrganizationID = organizationId,
                EmployeeID = employeeId,
                LeaveLedgerID = leaveLedgerId,
                PaystubID = paystubId,
                PayPeriodID = payPeriodId,
                LeaveHours = leaveHours,
                Amount = amount,
            };
        }
    }
}
