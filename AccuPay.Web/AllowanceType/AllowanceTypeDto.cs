namespace AccuPay.Web.AllowanceType
{
    public class AllowanceTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string DisplayString { get; set; }
        public string Frequency { get; set; }
        public bool IsTaxable { get; set; }
        public bool Is13thMonthPay { get; set; }
        public bool IsFixed { get; set; }

        public static AllowanceTypeDto Convert(Data.Entities.AllowanceType allowanceType)
        {
            return new AllowanceTypeDto()
            {
                DisplayString = allowanceType.DisplayString,
                Frequency = allowanceType.Frequency,
                Id = allowanceType.Id,
                Is13thMonthPay = allowanceType.Is13thMonthPay,
                IsFixed = allowanceType.IsFixed,
                IsTaxable = allowanceType.IsTaxable,
                Name = allowanceType.Name
            };
        }
    }
}
