using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("useractivityitem")]
    public class UserActivityItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int UserActivityId { get; set; }
        public int EntityId { get; set; }
        public string Description { get; set; }
        public int? ChangedEmployeeId { get; set; }
        public int? ChangedUserId { get; set; }

        [ForeignKey("ChangedEmployeeId")]
        public Employee ChangedEmployee { get; set; }

        [ForeignKey("ChangedUserId")]
        public AspNetUser ChangedUser { get; set; }
    }
}