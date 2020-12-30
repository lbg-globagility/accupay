using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeedisciplinaryaction")]
    public class DisciplinaryAction : EmployeeDataEntity
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int FindingID { get; set; }

        [ForeignKey("FindingID")]
        public virtual Product Finding { get; set; }

        public string FindingDescription { get; set; }

        public string Action { get; set; }

        public string Penalty { get; set; }

        public string Comments { get; set; }

        public string FindingName => Finding?.PartNo;
    }
}
