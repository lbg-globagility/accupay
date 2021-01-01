namespace AccuPay.Core.Services
{
    public class DefaultRates
    {
        public decimal RegularRate { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal NightDiffRate { get; set; }
        public decimal NightDiffOTRate { get; set; }
        public decimal RestDayRate { get; set; }
        public decimal RestDayOTRate { get; set; }
        public decimal RestDayNDRate { get; set; }
        public decimal RestDayNDOTRate { get; set; }
    }
}