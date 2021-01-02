using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPermissionRepository
    {
        Task<ICollection<Permission>> GetAll(bool forDesktopOnly = false);
    }
}
