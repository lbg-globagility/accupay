using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("useractivity")]
    public class UserActivity
    {
        public enum ChangedType
        {
            Employee,
            User,
            Organization,
            Division,
            Position
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int UserId { get; set; }
        public int OrganizationID { get; set; }
        public AspNetUser User { get; set; }
        public string EntityName { get; set; }
        public string RecordType { get; set; }
        public virtual ICollection<UserActivityItem> ActivityItems { get; set; }
    }
}
