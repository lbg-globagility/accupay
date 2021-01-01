using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeecertification")]
    public class Certification : EmployeeDataEntity
    {
        public string CertificationType { get; set; }

        public string IssuingAuthority { get; set; }

        public string CertificationNo { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string Comments { get; set; }
    }
}
