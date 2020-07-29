namespace AccuPay.Data.Helpers
{
    public class LoanPageOptions : PageOptions
    {
        public string SearchTerm { get; set; }

        public bool HasSearchTerm => !string.IsNullOrWhiteSpace(SearchTerm);

        public int? LoanTypeId { get; set; }

        public bool HasLoanTypeId => LoanTypeId.HasValue;

        public string Status { get; set; }

        public bool HasStatus => !string.IsNullOrWhiteSpace(Status);
    }
}
