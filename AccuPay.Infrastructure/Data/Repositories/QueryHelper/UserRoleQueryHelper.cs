using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Infrastructure.Data
{
    internal static class UserRoleQueryHelper
    {
        internal static IQueryable<UserRoleData> GetBaseQuery(PayrollContext context)
        {
            return from user in context.Users
                   join userRole in context.UserRoles
                   on user.Id
                   equals userRole.UserId
                   join role in context.Roles
                   on userRole.RoleId
                   equals role.Id
                   select new UserRoleData()
                   {
                       OrganizationId = userRole.OrganizationId,
                       User = user,
                       Role = role
                   };
        }
    }
}
