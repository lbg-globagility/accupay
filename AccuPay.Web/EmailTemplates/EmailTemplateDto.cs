namespace AccuPay.Web.EmailTemplates
{
    public class EmailTemplateDto
    {
        public int RowId { get; set; }
        public string Subject { get; set; }

        public string HtmlBody { get; set; }

        public string TextBody { get; set; }
    }
}
