using AccuPay.Core.Enums;

namespace AccuPay.Core.Interfaces
{
    public interface IHdmfPolicy
    {
        bool IsPolicyByOrganization { get; }

        HdmfCalculationBasis HdmfCalculationBasis(int organizationId);

        PayFrequencyType ImplementsInPayFrequency(int organizationId);
    }
}
