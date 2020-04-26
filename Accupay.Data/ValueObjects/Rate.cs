namespace AccuPay.Data.ValueObjects
{
    public class Rate
    {
        public string Name { get; set; }
        public decimal CurrentRate { get; set; }
        public Rate BaseRate { get; set; }

        public Rate(string name, decimal rate, Rate baseRate = null)
        {
            this.Name = name;
            this.CurrentRate = rate;
            this.BaseRate = baseRate;
        }
    }
}