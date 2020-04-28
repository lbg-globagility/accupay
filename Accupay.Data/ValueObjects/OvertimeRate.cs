using System.Collections.Generic;

namespace AccuPay.Data.ValueObjects
{
    public class OvertimeRate
    {
        public Rate BasePay { get; set; }
        public Rate Overtime { get; set; }
        public Rate NightDifferential { get; set; }
        public Rate NightDifferentialOvertime { get; set; }

        public Rate RestDay { get; set; }
        public Rate RestDayOvertime { get; set; }
        public Rate RestDayNightDifferential { get; set; }
        public Rate RestDayNightDifferentialOvertime { get; set; }

        public Rate SpecialHoliday { get; set; }
        public Rate SpecialHolidayOvertime { get; set; }
        public Rate SpecialHolidayNightDifferential { get; set; }
        public Rate SpecialHolidayNightDifferentialOvertime { get; set; }
        public Rate SpecialHolidayRestDay { get; set; }
        public Rate SpecialHolidayRestDayOvertime { get; set; }
        public Rate SpecialHolidayRestDayNightDifferential { get; set; }
        public Rate SpecialHolidayRestDayNightDifferentialOvertime { get; set; }

        public Rate RegularHoliday { get; set; }
        public Rate RegularHolidayOvertime { get; set; }
        public Rate RegularHolidayNightDifferential { get; set; }
        public Rate RegularHolidayNightDifferentialOvertime { get; set; }
        public Rate RegularHolidayRestDay { get; set; }
        public Rate RegularHolidayRestDayOvertime { get; set; }
        public Rate RegularHolidayRestDayNightDifferential { get; set; }
        public Rate RegularHolidayRestDayNightDifferentialOvertime { get; set; }

        public Rate DoubleHoliday { get; set; }
        public Rate DoubleHolidayOvertime { get; set; }
        public Rate DoubleHolidayNightDifferential { get; set; }
        public Rate DoubleHolidayNightDifferentialOvertime { get; set; }
        public Rate DoubleHolidayRestDay { get; set; }
        public Rate DoubleHolidayRestDayOvertime { get; set; }
        public Rate DoubleHolidayRestDayNightDifferential { get; set; }
        public Rate DoubleHolidayRestDayNightDifferentialOvertime { get; set; }

        public const string BasePayDescription = "Base Pay";
        public const string OvertimeDescription = "Overtime";
        public const string NightDifferentialDescription = "Night Differential";
        public const string NightDifferentialOvertimeDescription = "Night Differential OT";

        public const string RestDayDescription = "Rest Day";
        public const string RestDayOvertimeDescription = "Rest Day OT";
        public const string RestDayNightDifferentialDescription = "Rest Day Night Differential";
        public const string RestDayNightDifferentialOvertimeDescription = "Rest Day Night Differential OT";

        public const string SpecialHolidayDescription = "Special Holiday";
        public const string SpecialHolidayOvertimeDescription = "Special Holiday OT";
        public const string SpecialHolidayNightDifferentialDescription = "Special Holiday Night Differential";
        public const string SpecialHolidayNightDifferentialOvertimeDescription = "Special Holiday Night Differential OT";
        public const string SpecialHolidayRestDayDescription = "Special Holiday Rest Day";
        public const string SpecialHolidayRestDayOvertimeDescription = "Special Holiday Rest Day OT";
        public const string SpecialHolidayRestDayNightDifferentialDescription = "Special Holiday Rest Day Night Differential";
        public const string SpecialHolidayRestDayNightDifferentialOvertimeDescription = "Special Holiday Rest Day Night Differential OT";

        public const string RegularHolidayDescription = "Regular Holiday";
        public const string RegularHolidayOvertimeDescription = "Regular Holiday OT";
        public const string RegularHolidayNightDifferentialDescription = "Regular Holiday Night Differential";
        public const string RegularHolidayNightDifferentialOvertimeDescription = "Regular Holiday Night Differential OT";
        public const string RegularHolidayRestDayDescription = "Regular Holiday Rest Day";
        public const string RegularHolidayRestDayOvertimeDescription = "Regular Holiday Rest Day OT";
        public const string RegularHolidayRestDayNightDifferentialDescription = "Regular Holiday Rest Day Night Differential";
        public const string RegularHolidayRestDayNightDifferentialOvertimeDescription = "Regular Holiday Rest Day Night Differential OT";

        public const string DoubleHolidayDescription = "Double Holiday";
        public const string DoubleHolidayOvertimeDescription = "Double Holiday OT";
        public const string DoubleHolidayNightDifferentialDescription = "Double Holiday Night Differential";
        public const string DoubleHolidayNightDifferentialOvertimeDescription = "ReguDoublelar Holiday Night Differential OT";
        public const string DoubleHolidayRestDayDescription = "Double Holiday Rest Day";
        public const string DoubleHolidayRestDayOvertimeDescription = "Double Holiday Rest Day OT";
        public const string DoubleHolidayRestDayNightDifferentialDescription = "Double Holiday Rest Day Night Differential";
        public const string DoubleHolidayRestDayNightDifferentialOvertimeDescription = "Double Holiday Rest Day Night Differential OT";

        private List<Rate> _overtimeRateList;

        public List<Rate> OvertimeRateList
        {
            get
            {
                if (_overtimeRateList == null)
                {
                    _overtimeRateList = new List<Rate>();

                    // _rateList.Add(Me.BasePay)
                    _overtimeRateList.Add(this.Overtime);
                    _overtimeRateList.Add(this.NightDifferential);
                    _overtimeRateList.Add(this.NightDifferentialOvertime);
                    _overtimeRateList.Add(this.RestDay);
                    _overtimeRateList.Add(this.RestDayOvertime);
                    _overtimeRateList.Add(this.RestDayNightDifferential);
                    _overtimeRateList.Add(this.RestDayNightDifferentialOvertime);

                    _overtimeRateList.Add(this.SpecialHoliday);
                    _overtimeRateList.Add(this.SpecialHolidayOvertime);
                    _overtimeRateList.Add(this.SpecialHolidayNightDifferential);
                    _overtimeRateList.Add(this.SpecialHolidayNightDifferentialOvertime);
                    _overtimeRateList.Add(this.SpecialHolidayRestDay);
                    _overtimeRateList.Add(this.SpecialHolidayRestDayOvertime);
                    _overtimeRateList.Add(this.SpecialHolidayRestDayNightDifferential);
                    _overtimeRateList.Add(this.SpecialHolidayRestDayNightDifferentialOvertime);

                    _overtimeRateList.Add(this.RegularHoliday);
                    _overtimeRateList.Add(this.RegularHolidayOvertime);
                    _overtimeRateList.Add(this.RegularHolidayNightDifferential);
                    _overtimeRateList.Add(this.RegularHolidayNightDifferentialOvertime);
                    _overtimeRateList.Add(this.RegularHolidayRestDay);
                    _overtimeRateList.Add(this.RegularHolidayRestDayOvertime);
                    _overtimeRateList.Add(this.RegularHolidayRestDayNightDifferential);
                    _overtimeRateList.Add(this.RegularHolidayRestDayNightDifferentialOvertime);
                }

                return _overtimeRateList;
            }
        }

        public RateGroup RegularRateGroup
        {
            get
            {
                return new RateGroup()
                {
                    BasePay = this.BasePay,
                    Overtime = this.Overtime,
                    NightDifferential = this.NightDifferential,
                    NightDifferentialOvertime = this.NightDifferentialOvertime,
                    RestDay = this.RestDay,
                    RestDayOvertime = this.RestDayOvertime,
                    RestDayNightDifferential = this.RestDayNightDifferential,
                    RestDayNightDifferentialOvertime = this.RestDayNightDifferentialOvertime
                };
            }
        }

        public RateGroup SpecialHolidayRateGroup
        {
            get
            {
                return new RateGroup()
                {
                    BasePay = this.SpecialHoliday,
                    Overtime = this.SpecialHolidayOvertime,
                    NightDifferential = this.SpecialHolidayNightDifferential,
                    NightDifferentialOvertime = this.SpecialHolidayNightDifferentialOvertime,
                    RestDay = this.SpecialHolidayRestDay,
                    RestDayOvertime = this.SpecialHolidayRestDayOvertime,
                    RestDayNightDifferential = this.SpecialHolidayRestDayNightDifferential,
                    RestDayNightDifferentialOvertime = this.SpecialHolidayRestDayNightDifferentialOvertime
                };
            }
        }

        public RateGroup RegularHolidayRateGroup
        {
            get
            {
                return new RateGroup()
                {
                    BasePay = this.RegularHoliday,
                    Overtime = this.RegularHolidayOvertime,
                    NightDifferential = this.RegularHolidayNightDifferential,
                    NightDifferentialOvertime = this.RegularHolidayNightDifferentialOvertime,
                    RestDay = this.RegularHolidayRestDay,
                    RestDayOvertime = this.RegularHolidayRestDayOvertime,
                    RestDayNightDifferential = this.RegularHolidayRestDayNightDifferential,
                    RestDayNightDifferentialOvertime = this.RegularHolidayRestDayNightDifferentialOvertime
                };
            }
        }

        public RateGroup DoubleHolidayRateGroup
        {
            get
            {
                return new RateGroup()
                {
                    BasePay = this.DoubleHoliday,
                    Overtime = this.DoubleHolidayOvertime,
                    NightDifferential = this.DoubleHolidayNightDifferential,
                    NightDifferentialOvertime = this.DoubleHolidayNightDifferentialOvertime,
                    RestDay = this.DoubleHolidayRestDay,
                    RestDayOvertime = this.DoubleHolidayRestDayOvertime,
                    RestDayNightDifferential = this.DoubleHolidayRestDayNightDifferential,
                    RestDayNightDifferentialOvertime = this.DoubleHolidayRestDayNightDifferentialOvertime
                };
            }
        }

        public OvertimeRate(decimal basePay, decimal overtime, decimal nightDifferential, decimal nightDifferentialOvertime, decimal restDay, decimal restDayOvertime, decimal restDayNightDifferential, decimal restDayNightDifferentialOvertime, decimal specialHoliday, decimal specialHolidayOvertime, decimal specialHolidayNightDifferential, decimal specialHolidayNightDifferentialOvertime, decimal specialHolidayRestDay, decimal specialHolidayRestDayOvertime, decimal specialHolidayRestDayNightDifferential, decimal specialHolidayRestDayNightDifferentialOvertime, decimal regularHoliday, decimal regularHolidayOvertime, decimal regularHolidayNightDifferential, decimal regularHolidayNightDifferentialOvertime, decimal regularHolidayRestDay, decimal regularHolidayRestDayOvertime, decimal regularHolidayRestDayNightDifferential, decimal regularHolidayRestDayNightDifferentialOvertime, decimal doubleHoliday, decimal doubleHolidayOvertime, decimal doubleHolidayNightDifferential, decimal doubleHolidayNightDifferentialOvertime, decimal doubleHolidayRestDay, decimal doubleHolidayRestDayOvertime, decimal doubleHolidayRestDayNightDifferential, decimal doubleHolidayRestDayNightDifferentialOvertime)
        {
            this.BasePay = new Rate("Base Pay", basePay);
            this.Overtime = new Rate("Overtime", overtime);
            this.NightDifferential = new Rate("Night Differential", nightDifferential, this.BasePay);
            this.NightDifferentialOvertime = new Rate("Night Differential OT", nightDifferentialOvertime);

            this.RestDay = new Rate("Rest Day", restDay);
            this.RestDayOvertime = new Rate("Rest Day OT", restDayOvertime);
            this.RestDayNightDifferential = new Rate("Rest Day Night Differential", restDayNightDifferential, this.RestDay);
            this.RestDayNightDifferentialOvertime = new Rate("Rest Day Night Differential OT", restDayNightDifferentialOvertime);

            this.SpecialHoliday = new Rate("Special Holiday", specialHoliday);
            this.SpecialHolidayOvertime = new Rate("Special Holiday OT", specialHolidayOvertime);
            this.SpecialHolidayNightDifferential = new Rate("Special Holiday Night Differential", specialHolidayNightDifferential, this.SpecialHoliday);
            this.SpecialHolidayNightDifferentialOvertime = new Rate("Special Holiday Night Differential OT", specialHolidayNightDifferentialOvertime);
            this.SpecialHolidayRestDay = new Rate("Special Holiday Rest Day", specialHolidayRestDay);
            this.SpecialHolidayRestDayOvertime = new Rate("Special Holiday Rest Day OT", specialHolidayRestDayOvertime);
            this.SpecialHolidayRestDayNightDifferential = new Rate("Special Holiday Rest Day Night Differential", specialHolidayRestDayNightDifferential, this.SpecialHolidayRestDay);
            this.SpecialHolidayRestDayNightDifferentialOvertime = new Rate("Special Holiday Rest Day Night Differential OT", specialHolidayRestDayNightDifferentialOvertime);

            this.RegularHoliday = new Rate("Regular Holiday", regularHoliday);
            this.RegularHolidayOvertime = new Rate("Regular Holiday OT", regularHolidayOvertime);
            this.RegularHolidayNightDifferential = new Rate("Regular Holiday Night Differential", regularHolidayNightDifferential, this.RegularHoliday);
            this.RegularHolidayNightDifferentialOvertime = new Rate("Regular Holiday Night Differential OT", regularHolidayNightDifferentialOvertime);
            this.RegularHolidayRestDay = new Rate("Regular Holiday Rest Day", regularHolidayRestDay);
            this.RegularHolidayRestDayOvertime = new Rate("Regular Holiday Rest Day OT", regularHolidayRestDayOvertime);
            this.RegularHolidayRestDayNightDifferential = new Rate("Regular Holiday Rest Day Night Differential", regularHolidayRestDayNightDifferential, this.RegularHolidayRestDay);
            this.RegularHolidayRestDayNightDifferentialOvertime = new Rate("Regular Holiday Rest Day Night Differential OT", regularHolidayRestDayNightDifferentialOvertime);

            this.DoubleHoliday = new Rate("Double Holiday", doubleHoliday);
            this.DoubleHolidayOvertime = new Rate("Double Holiday OT", doubleHolidayOvertime);
            this.DoubleHolidayNightDifferential = new Rate("Double Holiday Night Differential", doubleHolidayNightDifferential, this.DoubleHoliday);
            this.DoubleHolidayNightDifferentialOvertime = new Rate("Double Holiday Night Differential OT", doubleHolidayNightDifferentialOvertime);
            this.DoubleHolidayRestDay = new Rate("Double Holiday Rest Day", doubleHolidayRestDay);
            this.DoubleHolidayRestDayOvertime = new Rate("Double Holiday Rest Day OT", doubleHolidayRestDayOvertime);
            this.DoubleHolidayRestDayNightDifferential = new Rate("Double Holiday Rest Day Night Differential", doubleHolidayRestDayNightDifferential, this.DoubleHoliday);
            this.DoubleHolidayRestDayNightDifferentialOvertime = new Rate("Double Holiday Rest Day Night Differential OT", doubleHolidayRestDayNightDifferentialOvertime);

            this.DoubleHoliday = new Rate("Double Holiday", doubleHoliday);
            this.DoubleHolidayOvertime = new Rate("Double Holiday OT", doubleHolidayOvertime);
            this.DoubleHolidayNightDifferential = new Rate("Double Holiday Night Differential", doubleHolidayNightDifferential, this.DoubleHoliday);
            this.DoubleHolidayNightDifferentialOvertime = new Rate("Double Holiday Night Differential OT", doubleHolidayNightDifferentialOvertime);
            this.DoubleHolidayRestDay = new Rate("Double Holiday Rest Day", doubleHolidayRestDay);
            this.DoubleHolidayRestDayOvertime = new Rate("Double Holiday Rest Day OT", doubleHolidayRestDayOvertime);
            this.DoubleHolidayRestDayNightDifferential = new Rate("Double Holiday Rest Day Night Differential", doubleHolidayRestDayNightDifferential, this.DoubleHoliday);
            this.DoubleHolidayRestDayNightDifferentialOvertime = new Rate("Double Holiday Rest Day Night Differential OT", doubleHolidayRestDayNightDifferentialOvertime);
        }

        public class RateGroup
        {
            public Rate BasePay { get; set; }
            public Rate Overtime { get; set; }
            public Rate NightDifferential { get; set; }
            public Rate NightDifferentialOvertime { get; set; }

            public Rate RestDay { get; set; }
            public Rate RestDayOvertime { get; set; }
            public Rate RestDayNightDifferential { get; set; }
            public Rate RestDayNightDifferentialOvertime { get; set; }
        }
    }
}