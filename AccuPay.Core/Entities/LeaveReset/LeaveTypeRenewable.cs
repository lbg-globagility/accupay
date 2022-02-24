using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities.LeaveReset
{
    [Table("leavetyperenewable")]
    public class LeaveTypeRenewable
    {
        public int LeaveResetId { get; internal set; }
        public int LeaveTypeId { get; internal set; }
        public bool IsSupported { get; internal set; }
        public bool IsUnusedLeaveCashable { get; internal set; }
        public BasisStartDateEnum BasisStartDate { get; internal set; }

        public virtual LeaveReset LeaveReset { get; set; }

        [ForeignKey("LeaveTypeId")]
        public virtual Product Product { get; set; }

        public string LeaveName => Product?.PartNo ?? string.Empty;

        public bool IsBasedOnStartDate => BasisStartDate == BasisStartDateEnum.StartDate;

        public bool IsBasedOnDateRegularized => BasisStartDate == BasisStartDateEnum.DateRegularized;
    }
}
