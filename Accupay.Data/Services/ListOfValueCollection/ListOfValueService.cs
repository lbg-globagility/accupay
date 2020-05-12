using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class ListOfValueService
    {
        private readonly PayrollContext context;

        public ListOfValueService(PayrollContext context)
        {
            this.context = context;
        }

        public ListOfValueCollection Create(string type = null)
        {
            List<ListOfValue> listOfValues;

            if (type == null)
            {
                listOfValues = context.ListOfValues.ToList();
            }
            else
            {
                listOfValues = context.ListOfValues.
                                        Where(x => x.Type.ToLower() == type.ToLower()).
                                        ToList();
            }

            return new ListOfValueCollection(listOfValues);
        }

        public async Task<ListOfValueCollection> CreateAsync(string type = null)
        {
            List<ListOfValue> listOfValues;

            if (type == null)
            {
                listOfValues = await context.ListOfValues.ToListAsync();
            }
            else
            {
                listOfValues = await context.ListOfValues.
                                        Where(x => x.Type.ToLower() == type.ToLower()).
                                        ToListAsync();
            }
            return new ListOfValueCollection(listOfValues);
        }

        public ListOfValueCollection Create(IEnumerable<ListOfValue> values)
        {
            return new ListOfValueCollection(values);
        }
    }
}