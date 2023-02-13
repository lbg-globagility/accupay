using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("resetleavecredit")]
    public class ResetLeaveCredit : OrganizationalEntity
    {
        public int? StartPeriodId { get; set; }
        public virtual PayPeriod PayPeriod { get; set; }
        public virtual ICollection<ResetLeaveCreditItem> ResetLeaveCreditItems { get; set; }
        public bool HasApplications => ResetLeaveCreditItems == null ? false : ResetLeaveCreditItems.Any(x => x.IsApplied);

        public static ResetLeaveCredit NewResetLeaveCredit(
            int organizationId,
            int userId,
            int? periodId,
            PayPeriod period = null) => new ResetLeaveCredit() { OrganizationID = organizationId, CreatedBy = userId, StartPeriodId = periodId, PayPeriod = period };
    }
}
