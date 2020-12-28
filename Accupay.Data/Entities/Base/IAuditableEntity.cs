using System;

namespace AccuPay.Data.Entities
{
    public interface IAuditableEntity
    {
        DateTime Created { get; }
        int? CreatedBy { get; }
        DateTime? LastUpd { get; }
        int? LastUpdBy { get; }
    }
}
