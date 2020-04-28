using System.Linq;

namespace AccuPay.Data.Services
{
    public class SystemOwnerService
    {
        public const string text_goldwings = "Goldwings";

        public const string text_hyundai = "Hyundai";

        public const string text_cinema2000 = "Cinema 2000";

        public const string text_benchmark = "Benchmark";

        public const string text_laglobal = "LA Global";

        public const string text_default = "Default";

        public static string Goldwings => text_goldwings;

        public static string Hyundai => text_hyundai;

        public static string Cinema2000 => text_cinema2000;

        public static string Benchmark => text_benchmark;

        public static string LAGlobal => text_laglobal;

        public static string DefaultOwner => text_default;

        public string GetCurrentSystemOwner()
        {
            using (var context = new PayrollContext())
            {
                return context.SystemOwners.
                        Where(x => x.IsCurrentOwner == "1").
                        Select(x => x.Name).
                        FirstOrDefault();
            }
        }
    }
}