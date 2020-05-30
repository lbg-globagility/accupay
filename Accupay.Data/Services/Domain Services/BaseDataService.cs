namespace AccuPay.Data.Services
{
    public class BaseDataService
    {
        public bool isNewEntity(int? id)
        {
            // sometimes it's not int.MinValue
            return id == null || id <= 0;
        }
    }
}