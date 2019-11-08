using Accupay.Data;
using System;

namespace AccupayWindowsService
{
    internal class PayPeriod : IPayPeriod
    {
        public int? RowID { get; set; }
        public DateTime PayFromDate { get; set; }
        public DateTime PayToDate { get; set; }
    }
}