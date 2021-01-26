using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAttachmentRepository : ISavableRepository<Attachment>
    {
        Task<ICollection<Attachment>> GetByEmployeeAsync(int employeeId);
    }
}
