using System;

namespace AccuPay.Web.Core.Auth
{
    /// <summary>
    /// Represents the existing user for the current request.
    /// </summary>
    public interface ICurrentUser
    {
        int UserId { get; }

        int OrganizationId { get; }

        int ClientId { get; }

        public int? EmployeeId { get; }
    }
}
