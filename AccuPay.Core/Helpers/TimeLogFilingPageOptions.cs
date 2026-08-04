using System;

namespace AccuPay.Core.Helpers
{
    public class TimeLogFilingPageOptions : PageOptions
    {
        public string SearchTerm { get; set; }

        public bool HasSearchTerm => !string.IsNullOrWhiteSpace(SearchTerm);

        public DateTime? DateFrom { get; set; }

        public bool HasDateFrom => DateFrom.HasValue;

        public DateTime? DateTo { get; set; }

        public bool HasDateTo => DateTo.HasValue;

        public int? EmployeeId { get; set; }

        public bool HasEmployeeId => EmployeeId.HasValue;

        public string Status { get; set; }

        public bool HasStatus => !string.IsNullOrWhiteSpace(Status);
    }
}
