using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; private set; }

        public int? CreatedBy { get; set; }

        public int? LastUpdBy { get; set; }

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
