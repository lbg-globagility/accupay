using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("listofval")]
    public class ListOfValue : AuditableEntity
    {
        public const string ActiveYesOption = "Yes";

        public int? OrganizationID { get; set; }

        public string LIC { get; set; }

        public string DisplayValue { get; set; }

        public string Type { get; set; }

        public string ParentLIC { get; set; }

        public string Active { get; set; }

        public string Description { get; set; }

        public int? OrderBy { get; set; }

        public static ListOfValue NewPolicy(string value, string lic, string type, int? organizationId)
        {
            return new ListOfValue()
            {
                DisplayValue = value,
                LIC = lic,
                Type = type,
                Active = ActiveYesOption,
                OrganizationID = organizationId,
            };
        }
    }
}
