using AccuPay.Data.Entities;

namespace AccuPay.Data.Repositories
{
    public abstract class BaseRepository
    {
        public bool IsNewEntity(int? id) => BaseEntity.CheckIfNewEntity(id);
    }
}
