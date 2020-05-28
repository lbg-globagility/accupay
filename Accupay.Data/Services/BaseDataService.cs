namespace AccuPay.Data.Services
{
    public class BaseDataService
    {
        public bool isNewEntity(int? id)
        {
            return id == null || id == int.MinValue;
        }
    }
}