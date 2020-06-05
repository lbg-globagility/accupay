namespace AccuPay.Data.Services
{
    public class TotalTimeEntryHours
    {
        public decimal RegularHours { get; protected set; }
        public decimal AbsentHours { get; protected set; }
        public decimal LateHours { get; protected set; }
        public decimal UndertimeHours { get; protected set; }
        public decimal OvertimeHours { get; protected set; }
        public decimal LeaveHours { get; protected set; }
        public decimal NightDifferentialHours { get; protected set; }
        public decimal NightDifferentialOvertimeHours { get; protected set; }
        public decimal SpecialHolidayHours { get; protected set; }
        public decimal SpecialHolidayOTHours { get; protected set; }
        public decimal RegularHolidayHours { get; protected set; }
        public decimal RegularHolidayOTHours { get; protected set; }
        public decimal RestDayHours { get; protected set; }
        public decimal RestDayOTHours { get; protected set; }
    }
}