using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
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
    }
}
