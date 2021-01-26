using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public int? CategoryID { get; set; }

        public string CategoryName { get; set; }

        public int OrganizationID { get; set; }

        public string Catalog { get; set; }

        public int? CatalogID { get; set; }

        public DateTime? LastUpd { get; set; }

        public Organization Organization { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}
