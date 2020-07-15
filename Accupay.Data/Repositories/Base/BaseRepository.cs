namespace AccuPay.Data.Repositories
{
    public class BaseRepository
    {
        public bool IsNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0 || !id.HasValue;
        }
    }
}