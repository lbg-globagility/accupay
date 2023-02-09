using AccuPay.Core.Enums;

namespace AccuPay.Core.Interfaces
{
    public interface IPhilHealthPolicy
    {
        decimal MaximumContribution { get; }
        decimal MinimumContribution { get; }
        bool OddCentDifference { get; }
        decimal Rate { get; }

        PhilHealthCalculationBasis CalculationBasis(int organizationId);

        PayFrequencyType ImplementsInPayFrequency(int organizationId);
    }
}
