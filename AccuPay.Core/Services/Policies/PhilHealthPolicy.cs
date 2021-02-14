using AccuPay.Core.Enums;
using AccuPay.Core.Interfaces;

namespace AccuPay.Core.Services
{
    public class PhilHealthPolicy : IPhilHealthPolicy
    {
        private readonly ListOfValueCollection _settings;

        public PhilHealthPolicy(ListOfValueCollection settings) => _settings = settings;

        public bool OddCentDifference => _settings.GetBoolean("PhilHealth.Remainder", true);

        public decimal MinimumContribution => _settings.GetDecimal("PhilHealth.MinimumContribution");

        public decimal MaximumContribution => _settings.GetDecimal("PhilHealth.MaximumContribution");

        public decimal Rate => _settings.GetDecimal("PhilHealth.Rate");

        public PhilHealthCalculationBasis CalculationBasis(int organizationId)
        {
            var policyByOrganization = _settings.GetBoolean("Policy.ByOrganization", false);

            return _settings.GetEnum(
                "PhilHealth.CalculationBasis",
                PhilHealthCalculationBasis.BasicSalary,
                policyByOrganization,
                organizationId);
        }
    }
}
