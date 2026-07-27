using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EmailTemplateRepository : SavableRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<EmailTemplate> GetByCodeAsync(string code, int? organizationId)
        {
            return await _context.Set<EmailTemplate>()
                .AsNoTracking()
                .Where(t => t.Code == code &&
                    t.IsActive &&
                    (t.OrganizationID == organizationId || t.OrganizationID == null))
                .OrderByDescending(t => t.OrganizationID.HasValue)
                .FirstOrDefaultAsync();
        }
    }
}
