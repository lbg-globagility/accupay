using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Repositories
{
    public interface IBankFileHeaderRepository : ISavableRepository<BankFileHeader>
    {
        Task<BankFileHeader> GetByOrganizationOrCreateAsync(int organizationID,
            int userId,
            string companyCode = "0",
            string batchNo = "0");

    }
}
