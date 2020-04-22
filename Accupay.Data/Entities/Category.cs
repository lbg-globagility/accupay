using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int RowID { get; set; }

        public virtual int? CategoryID { get; set; }

        public virtual string CategoryName { get; set; }

        public virtual int OrganizationID { get; set; }

        public virtual string Catalog { get; set; }

        public virtual int? CatalogID { get; set; }

        public virtual DateTime? LastUpd { get; set; }
    }
}
