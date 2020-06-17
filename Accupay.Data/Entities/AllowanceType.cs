using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("allowancetype")]
    public class AllowanceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string DisplayString { get; set; }
        public string Frequency { get; set; }
        public bool IsTaxable { get; set; }
        public bool Is13thMonthPay { get; set; }
        public bool IsFixed { get; set; }
    }
}