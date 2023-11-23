using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("systemowner")]
    public class SystemOwner
    {
        public const string text_goldwings = "Goldwings";

        public const string text_hyundai = "Hyundai";

        public const string text_cinema2000 = "Cinema 2000";

        public const string text_benchmark = "Benchmark";

        public const string text_laglobal = "LA Global";

        public const string text_rgi = "RGI";

        public const string text_default = "Default";

        private const string text_itc = "ITC";

        public static string Goldwings => text_goldwings;

        public static string Hyundai => text_hyundai;

        public static string Cinema2000 => text_cinema2000;

        public static string Benchmark => text_benchmark;

        public static string LAGlobal => text_laglobal;

        public static string RGI => text_rgi;

        public static string DefaultOwner => text_default;

        public static string ITC => text_itc;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public string Name { get; set; }
        public string IsCurrentOwner { get; set; }
    }
}
