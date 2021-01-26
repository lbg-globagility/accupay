using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IUserActivityRepository
    {
        Task CreateRecordAsync(int currentlyLoggedInUserId, string entityName, int organizationId, string recordType, List<UserActivityItem> activityItems = null);

        Task<PaginatedList<UserActivityItem>> GetPaginatedListAsync(PageOptions options, int? organizationId = null, string[] entityNames = null, int? changedByUserId = null, UserActivity.ChangedType? changedType = null, int? changedEntityId = null, string description = null, DateTime? dateFrom = null, DateTime? dateTo = null);

        Task RecordAddAsync(int currentlyLoggedInUserId, string entityName, int entityId, int organizationId, string suffixIdentifier = "", int? changedEmployeeId = null, int? changedUserId = null);

        Task RecordDeleteAsync(int currentlyLoggedInUserId, string entityName, int entityId, int organizationId, string suffixIdentifier = "", int? changedEmployeeId = null, int? changedUserId = null);
    }
}
