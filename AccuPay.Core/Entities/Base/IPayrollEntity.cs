using System;

namespace AccuPay.Core.Entities
{
    public interface IPayrollEntity
    {
        DateTime? PayrollDate { get; }

        int? OrganizationID { get; set; }
    }
}