using System;

namespace AccuPay.Web.Files.Exceptions
{
    /// <summary>
    /// Exception for failing to create an AttachmentClaim
    /// </summary>
    public class AttachmentClaimException : Exception
    {
        public AttachmentClaimException(string message) : base(message)
        {
        }
    }
}
