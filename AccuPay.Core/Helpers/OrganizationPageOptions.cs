namespace AccuPay.Core.Helpers
{
    public class OrganizationPageOptions : PageOptions
    {
        public int? ClientId { get; set; }

        public bool HasClientId => ClientId.HasValue;

        public new static OrganizationPageOptions AllData
        {
            get
            {
                var options = new OrganizationPageOptions();
                options.All = true;

                return options;
            }
        }
    }
}