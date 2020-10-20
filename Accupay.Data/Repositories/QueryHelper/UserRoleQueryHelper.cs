using AccuPay.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuPay.Data.Repositories
{
    public static class UserRoleQueryHelper
    {
        public static IQueryable<UserRoleData> GetBaseQuery(PayrollContext context)
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