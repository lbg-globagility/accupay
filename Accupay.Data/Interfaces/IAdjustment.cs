using AccuPay.Data.Entities;
using System;

namespace AccuPay.Data
{
    public interface IAdjustment
    {
        int? RowID { get; set; }

        int? OrganizationID { get; set; }

        DateTime Created { get; set; }

        int? CreatedBy { get; set; }

        DateTime? LastUpd { get; set; }

        int? LastUpdBy { get; set; }

        int? PaystubID { get; set; }

        int? ProductID { get; set; }

        decimal Amount { get; set; }

        string Comment { get; set; }

        bool IsActual { get; set; }

        Paystub Paystub { get; set; }

        Product Product { get; set; }

        bool Is13thMonthPay { get; set; }
    }
}