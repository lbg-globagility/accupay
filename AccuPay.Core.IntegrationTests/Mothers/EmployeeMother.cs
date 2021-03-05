using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;

namespace AccuPay.Core.IntegrationTests
{
    public class EmployeeMother
    {
        public static Employee Simple(string employeeType, int organizationId)
        {
            return new Employee()
            {
                EmployeeType = employeeType,
                WorkDaysPerYear = 312,
                OrganizationID = organizationId,
                Position = new Position()
                {
                    Division = new Division()
                    {
                        SssDeductionSchedule = ContributionSchedule.FIRST_HALF
                    }
                }
            };
        }
    }
}
