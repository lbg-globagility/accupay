using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("listofval")]
    public class ListOfValue
    {
        public const string ActiveYesOption = "Yes";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? OrganizationID { get; set; }

        public string LIC { get; set; }

        public string DisplayValue { get; set; }

        public string Type { get; set; }

        public string ParentLIC { get; set; }

        public string Active { get; set; }

        public string Description { get; set; }

        public int? OrderBy { get; set; }

        public static ListOfValue NewPolicy(string value, string lic, string type, int? organizationId, int currentlyLoggedInUserId, int? orderBy = null)
        {
            return new ListOfValue()
            {
                DisplayValue = value,
                LIC = lic,
                Type = type,
                Active = ActiveYesOption,
                OrganizationID = organizationId,
                CreatedBy = currentlyLoggedInUserId,
                LastUpdBy = currentlyLoggedInUserId,
                Created = DateTime.Now,
                LastUpd = DateTime.Now
            };
        }
    }
}