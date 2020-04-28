namespace AccuPay.Data.Services
{
    public class RenewLeaveBalancePolicy
    {
        public bool ProrateOnFirstAnniversary { get; set; } = false;

        public LeaveAllowanceAmountBasis LeaveAllowanceAmount { get; set; }

        public enum LeaveAllowanceAmountBasis
        {
            Default,
            NumberOfService
        }
    }
}