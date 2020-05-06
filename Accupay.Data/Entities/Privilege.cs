using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("view")]
    public class Privilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public string ViewName { get; set; }
        public int? OrganizationID { get; set; }

        //[ForeignKey("OrganizationID")]
        //public virtual Organization Organization { get; set; }
    }
}