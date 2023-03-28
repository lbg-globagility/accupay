using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Imports.Base;
using AccuPay.Core.Services.Imports.Enums;
using AccuPay.Core.Services.Imports.Policy;

namespace AccuPay.Core.Services.Imports.Base
{
    public abstract class EmployeeImporter : IEmployeeImporter
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ListOfValueCollection _settings;
        private readonly IProductRepository _productRepository;
        private ImportEmployeeIdentifier _employeeIdentifier;

        public EmployeeImporter(IEmployeeRepository employeeRepository,
            IOrganizationRepository organizationRepository,
            IProductRepository productRepository,
            IListOfValueService settings)
        {
            _employeeRepository = employeeRepository;
            _organizationRepository = organizationRepository;
            _settings = settings.Create();
            _productRepository = productRepository;
        }

        public IImportPolicy ImportPolicy => new ImportPolicy(settings: _settings);

        public bool IsSpecificToOrganization => ImportPolicy.IsSpecificToOrganization;

        public ImportEmployeeIdentifier EmployeeIdentifier
        {
            get
            {
                _employeeIdentifier = ImportPolicy.EmployeeIdentifier;
                return _employeeIdentifier;
            }
        }

        public bool IsEqualToEmployeeIdentifier(Employee comparedEmployee, string parsedEmployeeIdentifier)
        {
            switch (_employeeIdentifier)
            {
                case ImportEmployeeIdentifier.FullnameEmployeeIdCompanyname:
                    return comparedEmployee.FullnameEmployeeIdCompanyname.ToLower() == parsedEmployeeIdentifier;

                default: // ImportEmployeeIdentifier.EmployeeNo
                    return comparedEmployee.EmployeeNo.ToLower() == parsedEmployeeIdentifier;
            }
        }

        public bool IsEmployeeIdIdentifier => _employeeIdentifier == default(ImportEmployeeIdentifier);

        public bool IsFullnameEmployeeIdCompanyname => _employeeIdentifier == ImportEmployeeIdentifier.FullnameEmployeeIdCompanyname;

        public bool IsOpenToAllImportation => IsFullnameEmployeeIdCompanyname;
    }
}
