using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ProjectEmployeeRepository : SavableRepository<ProjectEmployee>, IProjectEmployeeRepository
    {
        public ProjectEmployeeRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<ProjectEmployee>> GetAllByProjectIdAsync(int projectId)
            => await _context.ProjectEmployees
                .AsNoTracking()
                .Include(pe => pe.Project)
                .Include(pe => pe.Employee)
                    .ThenInclude(e => e.Organization)
                .Where(pe => pe.ProjectId == projectId)
                .ToListAsync();

        public async Task<ICollection<int>> GetContractorEmployeeIdsAsync(int organizationId, TimePeriod timePeriod)
        {
            var start = timePeriod.Start;
            var end = timePeriod.End;

            return await _context.ProjectEmployees
                .AsNoTracking()
                .Where(pe => pe.EmployeeId != null)
                .Where(pe => pe.Employee.OrganizationID == organizationId)
                .Where(pe => pe.Project.ContractorId != null)
                .Where(pe => pe.BeginDate == null || pe.BeginDate <= end)
                .Where(pe => pe.EndDate == null || pe.EndDate >= start)
                .Where(pe => pe.Project.BeginDate == null || pe.Project.BeginDate <= end)
                .Where(pe => pe.Project.EndDate == null || pe.Project.EndDate >= start)
                .Select(pe => pe.EmployeeId.Value)
                .Distinct()
                .ToListAsync();
        }
    }
}
