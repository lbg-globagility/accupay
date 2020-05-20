using System;

namespace AccuPay.Web.OfficialBusinesses
{
    public interface ICrudOfficialBusinessDto
    {
        string Status { get; set; }

        DateTime StartDate { get; set; }

        DateTime EndTime { get; set; }

        DateTime StartTime { get; set; }

        string Reason { get; set; }

        string Comments { get; set; }
    }
}
