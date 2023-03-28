using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Enums;
using System.Collections.Generic;

namespace AccuPay.Core.Interfaces.Imports.Base
{
    internal interface IEmployeeImporter
    {
        IImportPolicy ImportPolicy { get; }
        bool IsSpecificToOrganization { get; }

        ImportEmployeeIdentifier EmployeeIdentifier { get; }

        bool IsEmployeeIdIdentifier { get; }
        bool IsFullnameEmployeeIdCompanyname { get; }
        bool IsOpenToAllImportation { get; }

        bool IsEqualToEmployeeIdentifier(Employee comparedEmployee, string parsedEmployeeIdentifier);
    }
}
