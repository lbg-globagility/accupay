namespace AccuPay.Web.Core.Auth
{
    public static class SelfServiceUser
    {
        // Self-service create/update/delete actions are attributed to this fixed
        // system user id rather than the authenticated caller's own UserId.
        public const int Id = 2;
    }
}
