namespace AccuPay.Data.Entities
{
    public class EmploymentPolicyItem
    {
        public int Id { get; set; }

        public int EmploymentPolicyId { get; set; }

        public int EmploymentPolicyTypeId { get; set; }

        public string Value { get; set; }
    }
}
