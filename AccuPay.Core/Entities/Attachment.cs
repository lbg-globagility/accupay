using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeeattachments")]
    public class Attachment : EmployeeDataEntity
    {
        public string Type { get; set; }

        public byte[] AttachedFile { get; set; }

        public string FileType { get; set; }

        public string FileName { get; set; }
    }
}
