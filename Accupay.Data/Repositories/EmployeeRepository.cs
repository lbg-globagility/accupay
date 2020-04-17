using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class EmployeeRepository
    {
        public class EmployeeBuilder : IDisposable
        {
            private PayrollContext _context;
            private IQueryable<Employee> _query;

            public EmployeeBuilder()
            {
                _context = new PayrollContext();
                _query = _context.Employees;
            }

            #region Builder Methods

            public EmployeeBuilder WithPayFrequency()
            {
                _query = _query.Include(x => x.PayFrequency);
                return this;
            }

            public EmployeeBuilder WithPosition()
            {
                _query = _query.Include(x => x.Position);
                // Note: No need to call WithPosition if WithDivision is already called.
                return this;
            }

            public EmployeeBuilder WithDivision()
            {
                _query = _query.Include(x => x.Position.Division);
                return this;
            }

            public EmployeeBuilder WithBranch()
            {
                _query = _query.Include(x => x.Branch);
                return this;
            }

            #endregion Builder Methods

            public IEnumerable<Employee> ToList()
            {
                return _query.ToList();
            }

            public Employee FirstOrDefault(int? employeeId = null)
            {
                if (employeeId != null)
                {
                    _query = _query.Where(x => x.RowID == employeeId);
                }

                return _query.FirstOrDefault();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }
    }
}