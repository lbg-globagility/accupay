namespace AccuPay.Web.Divisions
{
    public class DivisionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public string DivisionType { get; set; }

        public int? WorkDaysPerYear { get; set; }

        public bool? AutomaticOvertimeFiling { get; set; }

        public string PhilHealthDeductionSchedule { get; set; }

        public string SssDeductionSchedule { get; set; }

        public string PagIBIGDeductionSchedule { get; set; }

        public string WithholdingTaxSchedule { get; set; }
    }
}
