using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace AccuPay.Data.Entities
{
    [Table("employeepreviousemployer")]
    public class PreviousEmployer
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

        public string Name { get; set; }

        public string TradeName { get; set; }

        public string MainPhone { get; set; }

        public string FaxNumber { get; set; }

        public string JobTitle { get; set; }

        [Column("ExperienceFromTo")]
        public DateTime ExperienceFrom { get; set; }

        public DateTime ExperienceTo { get; set; }

        public string BusinessAddress { get; set; }

        public string ContactName { get; set; }

        public string EmailAddress { get; set; }

        public string AltEmailAddress { get; set; }

        public string AltPhone { get; set; }

        public string URL { get; set; }

        public string TINNo { get; set; }

        public string JobFunction { get; set; }

        public string OrganizationType { get; set; }
    }
}
