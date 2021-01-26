using System;

namespace AccuPay.Core.Helpers
{
    public class OfficialBusinessPageOptions : PageOptions
    {
        public string Term { get; set; }

        public bool HasSearchTerm => !string.IsNullOrWhiteSpace(Term);

        public DateTime? DateFrom { get; set; }

        public bool HasDateFrom => DateFrom.HasValue;

        public DateTime? DateTo { get; set; }

        public bool HasDateTo => DateTo.HasValue;

        public int? EmployeeId { get; set; }

        public bool HasEmployeeId => EmployeeId.HasValue;
    }
}