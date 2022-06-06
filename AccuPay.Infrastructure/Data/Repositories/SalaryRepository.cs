using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class SalaryRepository : SavableRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(PayrollContext context) : base(context)
        {
        }

        #region Save

        protected override void DetachNavigationProperties(Salary salary)
        {
            if (salary.Employee != null)
            {
                _context.Entry(salary.Employee).State = EntityState.Detached;
            }
        }

        #endregion Save

        #region Queries
        #region Single entity

        public async Task<Salary> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.Salaries
                .Include(x => x.Employee)
                .Where(x => x.RowID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Salary> GetLatest(int employeeId)
        {
            return await _context.Salaries
                .Where(t => t.EmployeeID == employeeId)
                .OrderByDescending(t => t.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<Salary>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Salaries
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<PaginatedList<Salary>> List(
            PageOptions options,
            int organizationId,
            string searchTerm = "",
            int? employeeId = null)
        {
            var query = _context.Salaries
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.EffectiveFrom)
                .ThenBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            if (employeeId.HasValue)
            {
                query = query.Where(t => t.EmployeeID == employeeId);
            }

            var salaries = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Salary>(salaries, count);
        }

        public async Task<ICollection<Salary>> GetByEmployeeAndEffectiveFromAsync(int employeeId, DateTime date)
        {
            return await _context.Salaries
                .AsNoTracking()
                .Where(l => l.EmployeeID == employeeId)
                .Where(l => l.EffectiveFrom.Date == date)
                .ToListAsync();
        }

        public async Task<ICollection<Salary>> GetAllAsync(int organizationId)
        {
            return await _context.Salaries
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<ICollection<Salary>> GetByCutOffAsync(int organizationId, DateTime cutoffEnd)
        {
            return await CreateBaseQueryByCutOff(cutoffEnd)
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<ICollection<Salary>> GetByMultipleEmployeeAsync(int[] employeeIds, DateTime cutoffEnd)
        {
            return await CreateBaseQueryByCutOff(cutoffEnd)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        public async Task<List<Salary>> GetSalariesByIds(int[] rowIds)
        {
            return await _context.Salaries.
                Where(s => rowIds.Contains(s.RowID.Value)).
                ToListAsync();
        }
        #endregion List of entities

        #endregion Queries

        private IQueryable<Salary> CreateBaseQueryByCutOff(DateTime cutoffEnd)
        {
            var query = _context.Salaries.AsQueryable();

            query = SalaryQueryHelper.GetLatestSalaryQuery(query, cutoffEnd);

            return query
                .AsNoTracking()
                .GroupBy(x => x.EmployeeID)
                .Select(g => g.FirstOrDefault());
        }

        public Task<List<Salary>> GetMultipleSalariesAsync(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            var startDate = $"'{dateFrom:yyyy-MM-dd}'";
            var endDate = $"'{dateTo:yyyy-MM-dd}'";
            var sql = $"CALL `GetMultipleSalaries`({organizationId}, {startDate}, {endDate}); SELECT * FROM `temposalary` WHERE(({startDate} BETWEEN EffectiveDateFrom AND EffectiveDateTo) OR ({endDate} BETWEEN EffectiveDateFrom AND EffectiveDateTo));";

            var dataTable = new DataTable();
            var connection = (MySqlConnection)_context.Database.GetDbConnection();
            var cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            var adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dataTable);

            var salaries = dataTable.Rows.OfType<DataRow>()
                .Select(r => Salary.NewSalary(employeeId: (int)r["EmployeeID"],
                    organizationId: (int)r["OrganizationID"],
                    positionID: (int?)r["PositionID"],
                    philHealthDeduction: (decimal)r["PhilHealthDeduction"],
                    hDMFAmount: (decimal)r["HDMFAmount"],
                    basicSalary: (decimal)r["Salary"],
                    allowanceSalary: (decimal)r["UndeclaredSalary"],
                    totalSalary: (decimal)r["TrueSalary"],
                    effectiveFrom: (DateTime)r["EffectiveDateFrom"],
                    doPaySSSContribution: (int)r["DoPaySSSContribution"],
                    autoComputePhilHealthContribution: (int)r["AutoComputePhilHealthContribution"],
                    autoComputeHDMFContribution: (int)r["AutoComputeHDMFContribution"],
                    isMinimumWage: (int)r["IsMinimumWage"],
                    effectiveTo: (DateTime)r["EffectiveDateTo"]))
                .ToList();

            return Task.FromResult(salaries);
        }
    }
}
