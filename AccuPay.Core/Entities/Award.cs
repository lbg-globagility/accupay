using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeeawards")]
    public class Award : EmployeeDataEntity
    {
        public string AwardType { get; set; }
        public string AwardDescription { get; set; }
        public DateTime AwardDate { get; set; }
    }
}
