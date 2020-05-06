using AccuPay.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AccuPay.Data.Helpers.UserConstants;

namespace AccuPay.Data.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        public int? RowID { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Column("UserID")]
        public string Username { get; set; }

        public string Password { get; set; }

        public int OrganizationID { get; set; }

        public int PositionID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? Created { get; set; }

        public int? LastUpdBy { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public string Status { get; set; }

        public string EmailAddress { get; set; }

        //public char? AllowLimitedAccess { get; set; }

        //public char? InSession { get; set; }

        public int UserLevel { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }

        #region Functions

        public bool IsActive
        {
            get
            {
                return Status == ACTIVE_STATUS;
            }
        }

        public void SetStatus(UserStatus userStatus)
        {
            Status = UserStatusToString(userStatus);
        }

        public static User NewUser(int organizationId, int userId)
        {
            return new User() { OrganizationID = organizationId, CreatedBy = userId, Status = ACTIVE_STATUS };
        }

        #endregion Functions
    }
}