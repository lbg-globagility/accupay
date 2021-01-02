using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeQueryBuilder
    {
        List<Employee> ToList(int organizationId);

        Task<List<Employee>> ToListAsync(int? organizationId);

        Task<Employee> FirstOrDefaultAsync(int organizationId);

        Task<Employee> GetByIdAsync(int employeeId, int? organizationId);

        IEmployeeQueryBuilder ByEmployeeNumber(string employeeNumber);

        IEmployeeQueryBuilder Filter(Expression<Func<Employee, bool>> filter);

        IEmployeeQueryBuilder HasNoPaystubs(int payPeriodId);

        IEmployeeQueryBuilder HasPaystubs(int payPeriodId);

        IEmployeeQueryBuilder IncludeAgency();

        IEmployeeQueryBuilder IncludeBranch();

        IEmployeeQueryBuilder IncludeDivision();

        IEmployeeQueryBuilder IncludeEmploymentPolicy();

        IEmployeeQueryBuilder IncludeImage();

        IEmployeeQueryBuilder IncludePayFrequency();

        IEmployeeQueryBuilder IncludePosition();

        IEmployeeQueryBuilder IsActive();
    }
}
