using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeQueryBuilder
    {
        private readonly PayrollContext _context;

        private IQueryable<Employee> _query;

        public EmployeeQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.Employees;
        }

        #region Builder Methods

        public EmployeeQueryBuilder IsActive()
        {
            _query = _query.Where(x => x.IsActive);
            return this;
        }

        public EmployeeQueryBuilder ByEmployeeNumber(string employeeNumber)
        {
            _query = _query.Where(x => x.EmployeeNo.Trim().ToLower() ==
                                        employeeNumber.ToTrimmedLowerCase());
            return this;
        }

        public EmployeeQueryBuilder Filter(Expression<Func<Employee, bool>> filter)
        {
            _query = _query.Where(filter);
            return this;
        }

        public EmployeeQueryBuilder HasPaystubs(int payPeriodId)
        {
            _query = _query.Where(CheckIfEmployeeHasPaystub(payPeriodId: payPeriodId, expected: true));
            return this;
        }

        public EmployeeQueryBuilder HasNoPaystubs(int payPeriodId)
        {
            _query = _query.Where(CheckIfEmployeeHasPaystub(payPeriodId: payPeriodId, expected: false));
            return this;
        }

        public EmployeeQueryBuilder IncludePayFrequency()
        {
            _query = _query.Include(x => x.PayFrequency);
            return this;
        }

        public EmployeeQueryBuilder IncludePosition()
        {
            // Note: No need to call WithPosition if WithDivision is already called.
            _query = _query.Include(x => x.Position);
            return this;
        }

        public EmployeeQueryBuilder IncludeDivision()
        {
            // Note: If needed division, position will also be queried automatically
            _query = _query.Include(x => x.Position.Division);
            return this;
        }

        public EmployeeQueryBuilder IncludeBranch()
        {
            _query = _query.Include(x => x.Branch);
            return this;
        }

        #endregion Builder Methods

        public List<Employee> ToList(int organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return _query.ToList();
        }

        public async Task<List<Employee>> ToListAsync(int? organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return await _query.ToListAsync();
        }

        public Employee GetById(int employeeId, int? organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return _query.Where(x => x.RowID == employeeId).FirstOrDefault();
        }

        public async Task<Employee> GetByIdAsync(int employeeId, int? organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return await _query.Where(x => x.RowID == employeeId).FirstOrDefaultAsync();
        }

        public Employee FirstOrDefault(int? organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return _query.FirstOrDefault();
        }

        public async Task<Employee> FirstOrDefaultAsync(int organizationId)
        {
            return await _query.FirstOrDefaultAsync();
        }

        private EmployeeQueryBuilder ResolveOrganizationIdQuery(int? organizationId)
        {
            if (organizationId != null)
            {
                _query = _query.Where(e => e.OrganizationID == organizationId);
            }

            return this;
        }

        private Expression<Func<Employee, bool>> CheckIfEmployeeHasPaystub(int payPeriodId, bool expected)
        {
            return e => _context.Paystubs.
                                    Any(p => p.EmployeeID == e.RowID &&
                                        p.PayPeriodID == payPeriodId) == expected;
        }
    }
}