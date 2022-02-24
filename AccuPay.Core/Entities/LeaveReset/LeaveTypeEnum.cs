using AccuPay.Core.Helpers;

namespace AccuPay.Core.Entities.LeaveReset
{
    public class LeaveTypeEnum
    {
        private LeaveTypeEnum(string value)
        { Type = value; }

        public string Type { get; internal set; }

        public static LeaveTypeEnum Vacation => new LeaveTypeEnum(ProductConstant.VACATION_LEAVE);
        public static LeaveTypeEnum Sick => new LeaveTypeEnum(ProductConstant.SICK_LEAVE);
        public static LeaveTypeEnum Others => new LeaveTypeEnum(ProductConstant.OTHERS_LEAVE);
        public static LeaveTypeEnum Parental => new LeaveTypeEnum(ProductConstant.PARENT_LEAVE);
    }
}
