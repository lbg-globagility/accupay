using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EmployeeQueryBuilder : IEmployeeQueryBuilder
    {
        private readonly PayrollContext _context;

        private IQueryable<Employee> _query;

        public EmployeeQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.Employees;
        }

        #region Builder Methods

        public IEmployeeQueryBuilder IsActive()
        {
            _query = _query.Where(x => x.IsActive);
            return this;
        }

        public IEmployeeQueryBuilder ByEmployeeNumber(string employeeNumber)
        {
            _query = _query.Where(x => x.EmployeeNo.Trim().ToLower() == employeeNumber.ToTrimmedLowerCase());
            return this;
        }

        public IEmployeeQueryBuilder Filter(Expression<Func<Employee, bool>> filter)
        {
            _query = _query.Where(filter);
            return this;
        }

        public IEmployeeQueryBuilder HasPaystubs(int payPeriodId)
        {
            _query = _query.Where(CheckIfEmployeeHasPaystub(payPeriodId: payPeriodId, expected: true));
            return this;
        }

        public IEmployeeQueryBuilder HasNoPaystubs(int payPeriodId)
        {
            _query = _query.Where(CheckIfEmployeeHasPaystub(payPeriodId: payPeriodId, expected: false));
            return this;
        }

        public IEmployeeQueryBuilder IncludeAgency()
        {
            _query = _query.Include(x => x.Agency);
            return this;
        }

        public IEmployeeQueryBuilder IncludeBranch()
        {
            _query = _query.Include(x => x.Branch);
            return this;
        }

        public IEmployeeQueryBuilder IncludeDivision()
        {
            // Note: If needed division, position will also be queried automatically
            _query = _query.Include(x => x.Position.Division);
            return this;
        }

        public IEmployeeQueryBuilder IncludeEmploymentPolicy()
        {
            _query = _query.Include(x => x.EmploymentPolicy);
            return this;
        }

        public IEmployeeQueryBuilder IncludeImage()
        {
            _query = _query.Include(x => x.OriginalImage);
            return this;
        }

        public IEmployeeQueryBuilder IncludePayFrequency()
        {
            _query = _query.Include(x => x.PayFrequency);
            return this;
        }

        public IEmployeeQueryBuilder IncludePosition()
        {
            // Note: No need to call WithPosition if WithDivision is already called.
            _query = _query.Include(x => x.Position);
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

        public async Task<Employee> GetByIdAsync(int employeeId, int? organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return await _query.Where(x => x.RowID == employeeId).FirstOrDefaultAsync();
        }

        public async Task<Employee> FirstOrDefaultAsync(int organizationId)
        {
            ResolveOrganizationIdQuery(organizationId);
            return await _query.FirstOrDefaultAsync();
        }

        private IEmployeeQueryBuilder ResolveOrganizationIdQuery(int? organizationId)
        {
            if (organizationId != null)
            {
                _query = _query.Where(e => e.OrganizationID == organizationId);
            }

            return this;
        }

        private Expression<Func<Employee, bool>> CheckIfEmployeeHasPaystub(int payPeriodId, bool expected)
        {
            return e => _context.Paystubs
                .Any(p => p.EmployeeID == e.RowID && p.PayPeriodID == payPeriodId)
                == expected;
        }
    }
}
