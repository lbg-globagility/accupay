using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBankFileHeaderRepository : ISavableRepository<BankFileHeader>
    {
        Task<BankFileHeader> GetByOrganizationOrCreateAsync(int organizationID,
            int userId,
            string companyCode = "0",
            string fundingAccountNo = "0",
            string presentingOfficeNo = "0",
            string batchNo = "0");
    }
}
