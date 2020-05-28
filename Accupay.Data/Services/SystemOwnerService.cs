using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        
        private readonly PayrollContext _context;

        public SystemOwnerService(PayrollContext context)
        {
            _context = context;
        }

        public string GetCurrentSystemOwner()
        {
            return GetCurrentSystemOwnerBaseQuery().
                    FirstOrDefault();
        }

        public async Task<string> GetCurrentSystemOwnerAsync()
        {
            return await GetCurrentSystemOwnerBaseQuery().
                            FirstOrDefaultAsync();
        }

        private IQueryable<string> GetCurrentSystemOwnerBaseQuery()
        {
            return _context.SystemOwners.
                    Where(x => x.IsCurrentOwner == "1").
                    Select(x => x.Name);
        }
    }
}