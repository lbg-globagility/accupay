using System;

namespace AccuPay.Data
{
    public interface IPaystubEmailHistory
    {
        int RowID { get; set; }
        DateTime Created { get; set; }
        int SentBy { get; set; }
        int PaystubID { get; set; }
        DateTime? SentDateTime { get; set; }
        string ReferenceNumber { get; set; }
        string EmailAddress { get; set; }
    }
}