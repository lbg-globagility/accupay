using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("listofval")]
    public class ListOfValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? OrganizationID { get; set; }

        public string LIC { get; set; }

        public string DisplayValue { get; set; }

        public string Type { get; set; }

        public string ParentLIC { get; set; }

        public string Active { get; set; }

        public string Description { get; set; }
    }
}