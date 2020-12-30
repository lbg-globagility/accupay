using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeepreviousemployer")]
    public class PreviousEmployer : EmployeeDataEntity
    {
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
