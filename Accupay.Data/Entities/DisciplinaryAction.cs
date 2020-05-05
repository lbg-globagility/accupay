using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccuPay.Data.Entities
{
    [Table("employeedisciplinaryaction")]
    public class DisciplinaryAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int EmployeeID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
        
        public int FindingID { get; set; }

        [ForeignKey("FindingID")]
        public virtual Product Finding { get; set; }

        public string FindingDescription { get; set; }

        public string Action { get; set; }

        public string Penalty { get; set; }

        public string Comments { get; set; }

        public string FindingName
        {
            get
            {
                return Finding?.Name;
            }
        }
    }
}
