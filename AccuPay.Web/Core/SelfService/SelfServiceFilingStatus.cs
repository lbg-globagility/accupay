using AccuPay.Core.Exceptions;
using System;

namespace AccuPay.Web.Core.SelfService
{
    /// <summary>
    /// Reads the optional "status" key of the self-service create requests (leaves, overtimes and
    /// timelog filings). A record that was already approved outside AccuPay (e.g. in Zoho People)
    /// can be created as "Approved"; without a status it is created as "Pending" and goes through
    /// the usual AccuPay approval flow.
    /// </summary>
    public static class SelfServiceFilingStatus
    {
        public const string Pending = "Pending";

        public const string Approved = "Approved";

        /// <summary>
        /// Returns true when the request asks for the record to be created as "Approved", and false
        /// when it asks for "Pending" or has no status. Matching ignores case and surrounding spaces.
        /// </summary>
        /// <exception cref="BusinessLogicException">The status is any other value.</exception>
        public static bool IsApproved(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return false;

            var value = status.Trim();

            if (string.Equals(value, Approved, StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(value, Pending, StringComparison.OrdinalIgnoreCase))
                return false;

            throw new BusinessLogicException(
                $"Status '{status}' is not valid. Allowed values are '{Pending}' and '{Approved}'.");
        }
    }
}
