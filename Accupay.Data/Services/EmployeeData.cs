using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class EmployeeData
    {
        private readonly PayrollContext _context;

        public EmployeeData(PayrollContext context)
        {
            _context = context;
        }
    }
}