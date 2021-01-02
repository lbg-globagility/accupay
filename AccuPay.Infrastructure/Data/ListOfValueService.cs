using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ListOfValueService : IListOfValueService
    {
        private readonly PayrollContext _context;

        public ListOfValueService(PayrollContext context)
        {
            _context = context;
        }

        public ListOfValueCollection Create(string type = null)
        {
            List<ListOfValue> listOfValues;

            if (type == null)
            {
                listOfValues = _context.ListOfValues.ToList();
            }
            else
            {
                listOfValues = _context.ListOfValues
                    .Where(x => x.Type.ToLower() == type.ToLower())
                    .ToList();
            }

            return new ListOfValueCollection(listOfValues);
        }

        public async Task<ListOfValueCollection> CreateAsync(string type = null)
        {
            List<ListOfValue> listOfValues;

            if (type == null)
            {
                listOfValues = await _context.ListOfValues.ToListAsync();
            }
            else
            {
                listOfValues = await _context.ListOfValues
                    .Where(x => x.Type.ToLower() == type.ToLower())
                    .ToListAsync();
            }
            return new ListOfValueCollection(listOfValues);
        }

        public ListOfValueCollection Create(IEnumerable<ListOfValue> values)
        {
            return new ListOfValueCollection(values);
        }
    }
}
