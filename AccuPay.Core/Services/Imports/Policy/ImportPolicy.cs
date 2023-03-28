using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Imports.Enums;

namespace AccuPay.Core.Services.Imports.Policy
{
    public class ImportPolicy : IImportPolicy
    {
        public const string TYPE = "ImportPolicy";
        private readonly ListOfValueCollection _settings;

        public ImportPolicy(ListOfValueCollection settings)
        {
            _settings = settings;
        }

        public bool IsSpecificToOrganization => EmployeeIdentifier == default(ImportEmployeeIdentifier);

        public ImportEmployeeIdentifier EmployeeIdentifier => _settings.GetEnum<ImportEmployeeIdentifier>(
            name: $"{TYPE}.EmployeeIdentifier");

        public bool IsOpenToAllImportMethod => EmployeeIdentifier == ImportEmployeeIdentifier.FullnameEmployeeIdCompanyname;
    }
}
