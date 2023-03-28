using AccuPay.Core.Services.Imports.Enums;

namespace AccuPay.Core.Interfaces
{
    public interface IImportPolicy
    {
        ImportEmployeeIdentifier EmployeeIdentifier { get; }

        bool IsSpecificToOrganization { get; }

        bool IsOpenToAllImportMethod { get; }
    }
}
