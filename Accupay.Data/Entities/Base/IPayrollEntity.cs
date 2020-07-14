using System;

namespace AccuPay.Data.Entities
{
    public interface IPayrollEntity
    {
        DateTime? PayrollDate { get; }

        int? OrganizationID { get; set; }
    }
}