namespace AccuPay.Core.ReportModels
{
    public class CinemaTardinessReportModel : ICinemaTardinessReportModel
    {
        public const int DaysLateLimit = 8;

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Days { get; set; }
        public decimal Hours { get; set; }
        public int NumberOfOffense { get; set; }

        public string NumberOfOffenseOrdinal
        {
            get
            {
                if (NumberOfOffense < 1)
                    return "-";

                switch (NumberOfOffense % 100)
                {
                    case 11:
                    case 12:
                    case 13:
                        return NumberOfOffense + "th";
                }

                switch (NumberOfOffense % 10)
                {
                    case 1:
                        return NumberOfOffense + "st";

                    case 2:
                        return NumberOfOffense + "nd";

                    case 3:
                        return NumberOfOffense + "rd";

                    default:
                        return NumberOfOffense + "th";
                }
            }
        }

        public string Sanction
        {
            get
            {
                if (NumberOfOffense < 1)
                    return "-";
                else if (NumberOfOffense == 1)
                    return "Written Reprimand";
                else if (NumberOfOffense == 2)
                    return "2 day Suspension";
                else if (NumberOfOffense == 3)
                    return "4 days Suspension";
                else if (NumberOfOffense == 4)
                    return "10 days Suspension";
                else if (NumberOfOffense == 5)
                    return "Dismissal with Due Process";
                else if (NumberOfOffense > 5)
                    return "HR: FOR IMMEDIATE ACTION";
                else
                    return "";
            }
        }

        public class PerMonth
        {
            public int EmployeeId { get; set; }
            public int Month { get; set; }
            public decimal Days { get; set; }
            public decimal Hours { get; set; }
        }
    }
}