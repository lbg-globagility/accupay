namespace AccuPay.Data.Repositories
{
    public class BaseRepository
    {
        public bool IsNewEntity(int? id)
        {
            return id == null || id == int.MinValue;
        }
    }
}