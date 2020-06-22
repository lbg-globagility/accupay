namespace AccuPay.Data.Helpers
{
    public class EmployeePageOptions : PageOptions
    {
        public string SearchTerm { get; set; }

        public bool HasSearchTerm => !string.IsNullOrWhiteSpace(SearchTerm);

        public string Filter { get; set; }

        public bool HasFilter => !string.IsNullOrWhiteSpace(Filter);
    }
}
