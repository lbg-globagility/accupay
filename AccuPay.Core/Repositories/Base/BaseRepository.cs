using AccuPay.Core.Entities;

namespace AccuPay.Core.Repositories
{
    public abstract class BaseRepository
    {
        public bool IsNewEntity(int? id) => BaseEntity.CheckIfNewEntity(id);
    }
}
