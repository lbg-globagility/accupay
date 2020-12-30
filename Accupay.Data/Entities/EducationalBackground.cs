using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeeeducation")]
    public class EducationalBackground : EmployeeDataEntity
    {
        public string Name { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string School { get; set; }

        public string Degree { get; set; }

        public string Course { get; set; }

        [Column("EducationType")]
        public string Type { get; set; }

        [Column("Minor")]
        public string Major { get; set; }

        public string Remarks { get; set; }
    }
}
