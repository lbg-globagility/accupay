using AccuPay.Core.Enums;

namespace AccuPay.Core.Interfaces
{
    public interface ISssPolicy
    {
        bool IsPolicyByOrganization { get; }

        SssCalculationBasis SssCalculationBasis(int organizationId);

        PayFrequencyType ImplementsInPayFrequency(int organizationId);
    }
}
