using AccuPay.Core.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("permission")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool ForDesktopOnly { get; set; }

        public bool IsAllowance => Name == PermissionConstant.ALLOWANCE;
        public bool IsLoan => Name == PermissionConstant.LOAN;
        public bool IsOfficialBusiness => Name == PermissionConstant.OFFICIALBUSINESS;
        public bool IsOvertime => Name == PermissionConstant.OVERTIME;
        public bool IsLeave => Name == PermissionConstant.LEAVE;
        public bool IsShift => Name == PermissionConstant.SHIFT;
    }
}
