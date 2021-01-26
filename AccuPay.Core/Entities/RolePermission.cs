using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("rolepermission")]
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        [Column("RoleId")]
        public int RoleId { get; set; }

        public AspNetRole Role { get; set; }

        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }

        public bool Read { get; set; }

        public bool Create { get; set; }

        public bool Update { get; set; }

        public bool Delete { get; set; }
    }
}