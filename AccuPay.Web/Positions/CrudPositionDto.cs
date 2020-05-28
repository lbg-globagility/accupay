namespace AccuPay.Web.Positions
{
    public abstract class CrudPositionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DivisionId { get; set; }

        public string DivisionName { get; set; }
    }
}
