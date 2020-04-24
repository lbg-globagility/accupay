using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionRepository
    {
        public async Task<List<Position>> GetAll(int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Positions.
                    Where(p => p.OrganizationID == organizationID).
                    ToListAsync();
            }
        }

        public async Task<Position> GetByName(int organizationID, string positionName)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Positions.
                    Where(p => p.OrganizationID == organizationID).
                    Where(p => p.Name == positionName).
                    FirstOrDefaultAsync();
            }
        }
    }
}