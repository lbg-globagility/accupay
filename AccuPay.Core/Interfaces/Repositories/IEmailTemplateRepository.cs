using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmailTemplateRepository : ISavableRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetByCodeAsync(string code, int? organizationId);
    }
}
