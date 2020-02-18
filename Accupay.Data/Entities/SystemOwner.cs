using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accupay.Data.Entities
{
    [Table("systemowner")]
    internal class SystemOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        internal string Name { get; set; }
        internal string IsCurrentOwner { get; set; }
    }
}