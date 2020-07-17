namespace AccuPay.Data.Helpers
{
    public class OrganizationPageOptions : PageOptions
    {
        public int? ClientId { get; set; }

        public bool HasClientId => ClientId.HasValue;
    }
}