using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class BreakTimeBracketRepository
    {
        private readonly PayrollContext _context;

        public BreakTimeBracketRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<BreakTimeBracket> GetAll(int organizationId)
        {
            return _context.BreakTimeBrackets.
                            Include(x => x.Division).
                            Where(x => x.Division.OrganizationID == organizationId).
                            ToList();
        }
    }
}