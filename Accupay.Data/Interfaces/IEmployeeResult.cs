using static AccuPay.Data.Helpers.ProgressGenerator;

namespace AccuPay.Data.Interfaces
{
    public interface IEmployeeResult : IResult
    {
        string EmployeeNumber { get; }

        string EmployeeFullName { get; }

        int EmployeeId { get; }

        string Description { get; }
    }
}