namespace AccuPay.Data.Repositories
{
    public class BaseRepository
    {
        public bool isNewEntity(int? id)
        {
            return id == null || id == int.MinValue;
        }
    }
}