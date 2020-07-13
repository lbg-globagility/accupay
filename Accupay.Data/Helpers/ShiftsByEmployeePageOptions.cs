using System;

namespace AccuPay.Data.Helpers
{
    public class ShiftsByEmployeePageOptions : PageOptions
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string SearchTerm { get; set; }

        public bool HasSearchTerm => !string.IsNullOrWhiteSpace(SearchTerm);

        public string Status { get; set; }

        public bool HasStatus => !string.IsNullOrWhiteSpace(Status);
    }
}