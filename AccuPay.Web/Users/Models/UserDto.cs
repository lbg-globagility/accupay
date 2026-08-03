using System;

namespace AccuPay.Web.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Type { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeType { get; set; }

        public string EmploymentStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public string PositionName { get; set; }
        public string EmployeeNo { get; set; }
        public string Image { get; set; }
    }
}
