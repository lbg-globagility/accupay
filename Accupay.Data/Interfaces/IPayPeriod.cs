using System;

namespace Accupay.Data
{
    public interface IPayPeriod
    {
        int? RowID { get; set; }
        DateTime PayFromDate { get; set; }
        DateTime PayToDate { get; set; }
    }
}