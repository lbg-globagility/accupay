using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccuPay.Data.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int OrganizationID { get; set; }
        public int UserLevel { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
