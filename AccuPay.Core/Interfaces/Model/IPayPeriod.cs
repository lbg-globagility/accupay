using System;

namespace AccuPay.Core
{
    public interface IPayPeriod
    {
        int? RowID { get; set; }
        DateTime PayFromDate { get; set; }
        DateTime PayToDate { get; set; }
        int Month { get; set; }
        int Year { get; set; }
        bool IsFirstHalf { get; }
    }
}