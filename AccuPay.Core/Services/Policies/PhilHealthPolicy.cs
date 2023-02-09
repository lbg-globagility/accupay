using AccuPay.Core.Enums;
using AccuPay.Core.Interfaces;

namespace AccuPay.Core.Services
{
    public class PhilHealthPolicy : IPhilHealthPolicy
    {
        public const string TYPE = "PhilHealth";
        public const string LIC_CALCULATION_BASIS = "CalculationBasis";
        public const string LIC_IMPLEMENTS_IN_PAYFREQUENCY = "ImplementsInPayFrequency";

        private readonly ListOfValueCollection _settings;

        public PhilHealthPolicy(ListOfValueCollection settings) => _settings = settings;

        public bool OddCentDifference => _settings.GetBoolean("PhilHealth.Remainder", true);

        public decimal MinimumContribution => _settings.GetDecimal("PhilHealth.MinimumContribution");

        public decimal MaximumContribution => _settings.GetDecimal("PhilHealth.MaximumContribution");

        public decimal Rate => _settings.GetDecimal("PhilHealth.Rate");

        private bool IsPolicyByOrganization => _settings.GetBoolean("Policy.ByOrganization", false);

        public PhilHealthCalculationBasis CalculationBasis(int organizationId) => _settings.GetEnum(
            $"{TYPE}.{LIC_CALCULATION_BASIS}",
            PhilHealthCalculationBasis.BasicSalary,
            IsPolicyByOrganization,
            organizationId);

        public PayFrequencyType ImplementsInPayFrequency(int organizationId) => _settings.GetEnum(
            $"{TYPE}.{LIC_IMPLEMENTS_IN_PAYFREQUENCY}",
            PayFrequencyType.SemiMonthly,
            IsPolicyByOrganization,
            organizationId);
    }
}
