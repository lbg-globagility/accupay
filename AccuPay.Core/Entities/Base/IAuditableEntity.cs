using System;

namespace AccuPay.Core.Entities
{
    public interface IAuditableEntity
    {
        DateTime Created { get; }
        int? CreatedBy { get; }
        DateTime? LastUpd { get; }
        int? LastUpdBy { get; }

        void AuditUser(int currentlyLoggedInUserId);
    }
}
