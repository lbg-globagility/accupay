using static AccuPay.Core.Helpers.ProgressGenerator;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeResult : IResult
    {
        string EmployeeNumber { get; }

        string EmployeeFullName { get; }

        int EmployeeId { get; }

        string Description { get; }
    }
}