using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; private set; }

        public int? CreatedBy { get; internal set; }

        public int? LastUpdBy { get; internal set; }

        public void AuditUser(int currentlyLoggedInUserId)
        {
            if (IsNewEntity)
            {
                CreatedBy = currentlyLoggedInUserId;
            }
            else
            {
                LastUpdBy = currentlyLoggedInUserId;
            }
        }
    }
}
