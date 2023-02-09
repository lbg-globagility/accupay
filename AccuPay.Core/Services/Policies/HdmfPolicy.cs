using AccuPay.Core.Enums;
using AccuPay.Core.Interfaces;

namespace AccuPay.Core.Services.Policies
{
    public class HdmfPolicy : IHdmfPolicy
    {
        public const string TYPE = "HDMF";
        public const string LIC_CALCULATION_BASIS = "CalculationBasis";
        public const string LIC_IMPLEMENTS_IN_PAYFREQUENCY = "ImplementsInPayFrequency";

        private readonly ListOfValueCollection _settings;

        public HdmfPolicy(ListOfValueCollection settings) => _settings = settings;

        public bool IsPolicyByOrganization => _settings.GetBoolean("Policy.ByOrganization", false);

        public PayFrequencyType ImplementsInPayFrequency(int organizationId) => _settings.GetEnum(
            $"{TYPE}.{LIC_IMPLEMENTS_IN_PAYFREQUENCY}",
            PayFrequencyType.SemiMonthly,
            IsPolicyByOrganization,
            organizationId);

        public HdmfCalculationBasis HdmfCalculationBasis(int organizationId) => _settings.GetEnum(
            $"{TYPE}.{LIC_CALCULATION_BASIS}",
            Enums.HdmfCalculationBasis.Default,
            IsPolicyByOrganization,
            organizationId);
    }
}
