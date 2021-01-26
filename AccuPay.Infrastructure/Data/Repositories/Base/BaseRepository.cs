using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public abstract class BaseRepository : IBaseRepository
    {
        public bool IsNewEntity(int? id) => BaseEntity.CheckIfNewEntity(id);
    }
}
