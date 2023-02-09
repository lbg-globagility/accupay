using AccuPay.Core.Enums;
using AccuPay.Core.Interfaces;

namespace AccuPay.Core.Services.Policies
{
    public class SssPolicy : ISssPolicy
    {
        public const string TYPE = "SocialSecuritySystem";
        public const string LIC_CALCULATION_BASIS = "CalculationBasis";
        public const string LIC_IMPLEMENTS_IN_PAYFREQUENCY = "ImplementsInPayFrequency";

        private readonly ListOfValueCollection _settings;

        public SssPolicy(ListOfValueCollection settings) => _settings = settings;

        public bool IsPolicyByOrganization => _settings.GetBoolean("Policy.ByOrganization", false);

        public PayFrequencyType ImplementsInPayFrequency(int organizationId) => _settings.GetEnum(
            $"{TYPE}.{LIC_IMPLEMENTS_IN_PAYFREQUENCY}",
            PayFrequencyType.SemiMonthly,
            IsPolicyByOrganization,
            organizationId);

        public SssCalculationBasis SssCalculationBasis(int organizationId) => _settings.GetEnum(
            $"{TYPE}.{LIC_CALCULATION_BASIS}",
            Enums.SssCalculationBasis.BasicSalary,
            IsPolicyByOrganization,
            organizationId);
    }
}
