using AccuPay.Core.Entities;
using System;
using System.Globalization;

namespace AccuPay.Core.Services.Policies
{
    public class WeeklyPayPeriodPolicy
    {
        public const string LIC = "Policy";
        public const string TYPE = "WeeklyPayPeriodPolicy";
        public const string ATM_DISPLAY_VALUE = "2022-01-01,2022-12-30,Sat,Fri,6,5";
        public const string CASH_DISPLAY_VALUE = "2022-01-03,2023-01-01,Mon,Sat,1,6";
        public const int INITIAL_YEAR = 2022;
        public static DateTime ATM_INITIAL_DATE = new DateTime(2022, 1, 1);
        public static DateTime ATM_ENDING_DATE = new DateTime(2022, 12, 30);
        public static DateTime CASH_INITIAL_DATE = new DateTime(2022, 1, 3);
        public static DateTime CASH_ENDING_DATE = new DateTime(2023, 1, 1);

        public WeeklyPayPeriodPolicy(ListOfValue listOfValue, int year)
        {
            char[] commaChar = { ',' };
            var delimitedValues = listOfValue.DisplayValue.Split(commaChar);

            char[] hypenChar = { '-' };
            var initialDate = delimitedValues[0].Split(hypenChar);
            var initialDateYear = int.Parse(initialDate[0]);
            var initialDateMonth = int.Parse(initialDate[1]);
            var initialDateDay = int.Parse(initialDate[2]);
            InitialDate = new DateTime(initialDateYear, initialDateMonth, initialDateDay);

            var finalDate = delimitedValues[1].Split(hypenChar);
            var finalDateYear = int.Parse(finalDate[0]);
            var finalDateMonth = int.Parse(finalDate[1]);
            var finalDateDay = int.Parse(finalDate[2]);
            FinalDate = new DateTime(finalDateYear, finalDateMonth, finalDateDay);

            StartDayOfWeek = InitialDate.DayOfWeek;
            EndDayOfWeek = InitialDate.Date.AddDays(6).DayOfWeek;

            var cultureInfo = new CultureInfo("en-US");
            var calendar = cultureInfo.Calendar;

            cultureInfo.DateTimeFormat.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
            var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            cultureInfo.DateTimeFormat.FirstDayOfWeek = StartDayOfWeek;
            var dayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            var lastDay = new DateTime(year: year,
                month: 12,
                day: DateTime.DaysInMonth(year: year, month: 12));
            WeekCountOfYear = GetWeekOfYear(lastDay: lastDay, calendar: calendar, calendarWeekRule: calendarWeekRule, dayOfWeek: dayOfWeek) - 1;

            var yearDiff = year - InitialDate.Year;
            if (!(yearDiff == 0))
            {
                var weekCount = WeekCountOfYear;
                InitialDate = AddWeeks(InitialDate, weekCount);
                FinalDate = AddWeeks(FinalDate, weekCount);
            }
        }

        public static int GetWeekOfYear(int year)
        {
            var lastDay = new DateTime(year: year,
                month: 12,
                day: DateTime.DaysInMonth(year: year, month: 12));

            return GetWeekOfYear(lastDay);
        }

        public static int GetWeekOfYear(DateTime lastDay,
            Calendar calendar = null,
            CalendarWeekRule calendarWeekRule = CalendarWeekRule.FirstDay,
            DayOfWeek dayOfWeek = DayOfWeek.Sunday)
        {
            if (calendar == null)
            {
                var cultureInfo = new CultureInfo("en-US");
                calendar = cultureInfo.Calendar;
            }
            return calendar.GetWeekOfYear(lastDay, calendarWeekRule, dayOfWeek);
        }

        public static DateTime AddWeeks(DateTime time, int weeks)
        {
            var gc = new GregorianCalendar();
            return gc.AddWeeks(time, weeks);
        }

        public DateTime InitialDate { get; }
        public DateTime FinalDate { get; }
        public DayOfWeek StartDayOfWeek { get; }
        public DayOfWeek EndDayOfWeek { get; }
        public string StartDayNameOfWeek => StartDayOfWeek.ToString();
        public string EndDayNameOfWeek => EndDayOfWeek.ToString();

        public int WeekCountOfYear { get; }
    }
}
