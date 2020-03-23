using AccuPay.Data.Entities;
using System;

namespace AccuPay.Data
{
    public interface IPaystubEmail
    {
        int RowID { get; set; }
        DateTime Created { get; set; }
        int CreatedBy { get; set; }
        int PaystubID { get; set; }
        DateTime? ProcessingStarted { get; set; }
        string ErrorLogMessage { get; set; }
        string Status { get; set; }
    }
}