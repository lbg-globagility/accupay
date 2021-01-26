using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.OfficialBusiness;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOfficialBusinessDataService : IBaseSavableDataService<OfficialBusiness>
    {
        Task<List<OfficialBusiness>> BatchApply(IReadOnlyCollection<OfficialBusinessImportModel> validRecords, int organizationId, int currentlyLoggedInUserId);
    }
}
