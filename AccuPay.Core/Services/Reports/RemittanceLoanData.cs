using System.Collections.Generic;

namespace AccuPay.Core.Services.Reports
{
    public class RemittanceLoanData
    {
        /// <summary>
        /// Loans with a deduction in the selected month.
        /// </summary>
        public ICollection<RemittanceLoanRow> Paid { get; set; }

        /// <summary>
        /// Loans paid in the previous month but not in the selected month.
        /// </summary>
        public ICollection<RemittanceLoanRow> StoppedPayment { get; set; }
    }
}
