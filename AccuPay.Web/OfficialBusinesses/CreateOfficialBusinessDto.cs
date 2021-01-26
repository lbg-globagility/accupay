using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.OfficialBusinesses
{
    public class CreateOfficialBusinessDto : CrudOfficialBusinessDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
