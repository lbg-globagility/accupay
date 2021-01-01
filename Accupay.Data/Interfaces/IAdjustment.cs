using AccuPay.Data.Entities;

namespace AccuPay.Data
{
    public interface IAdjustment : IAuditableEntity
    {
        int? RowID { get; set; }

        int? OrganizationID { get; set; }

        int? PaystubID { get; set; }

        int? ProductID { get; set; }

        decimal Amount { get; set; }

        string Comment { get; set; }

        bool IsActual { get; set; }

        Paystub Paystub { get; set; }

        Product Product { get; set; }

        bool Is13thMonthPay { get; set; }

        IAdjustment Clone();
    }
}
