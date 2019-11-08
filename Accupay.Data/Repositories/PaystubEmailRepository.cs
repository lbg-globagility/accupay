using Accupay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accupay.Data.Repositories
{
    public class PaystubEmailRepository
    {
        public PaystubEmail FirstWithPaystubDetails()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.PaystubEmails
                    .Include(x => x.Paystub.PayPeriod)
                    .Include(x => x.Paystub.Employee)
                    .FirstOrDefault();
            }
        }
    }
}