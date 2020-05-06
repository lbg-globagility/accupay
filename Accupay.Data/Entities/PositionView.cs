using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("position_view")]
    public class PositionView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public int? PositionID { get; set; }
        public int? ViewID { get; set; }
        public char Creates { get; set; }
        public int? OrganizationID { get; set; }
        public char ReadOnly { get; set; }
        public char Updates { get; set; }
        public char Deleting { get; set; }
        //public char AllowedToAccess { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        [ForeignKey("ViewID")]
        public virtual Privilege View { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }

        #region Functions

        public static PositionView NewPositionView(int organizationId, int userId)
        {
            return new PositionView() { OrganizationID = organizationId, CreatedBy = userId, ReadOnly = 'Y', Updates = 'N', Deleting = 'N', Creates = 'N' };
        }

        #endregion Functions
    }
}