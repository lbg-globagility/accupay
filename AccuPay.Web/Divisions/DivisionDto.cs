namespace AccuPay.Web.Divisions
{
    public class DivisionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }
    }
}
