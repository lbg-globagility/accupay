using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccuPay.Data.Helpers.UserConstants;

namespace AccuPay.Data.Repositories
{
    public class UserRepository : SavableRepository<User>
    {
        public UserRepository(PayrollContext context) : base(context)
        {
        }

        #region User List

        public async Task<ICollection<User>> GetAllAsync()
        {
            // builder's lifecycle should only span in each method
            // that is why we did not inject this on the constructor
            // and create a class variable of EmployeeQueryBuilder
            var builder = new UserQueryBuilder(_context);
            return await builder.ToListAsync();
        }

        public async Task<ICollection<User>> GetAllActiveAsync()
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .IsActive()
                .ToListAsync();
        }

        public async Task<ICollection<User>> GetAllActiveWithPositionAsync()
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .IsActive()
                .ToListAsync();
        }

        #endregion User List

        #region By User

        public async Task<User> GetByIdWithPositionAsync(int rowId)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .ById(rowId)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .ByUsername(username)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameWithPositionAsync(string username)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .ByUsername(username)
                .FirstOrDefaultAsync();
        }

        #endregion By User

        #region CRUD

        protected override void DetachNavigationProperties(User user)
        {
            if (user.Position != null)
            {
                _context.Entry(user.Position).State = EntityState.Detached;
            }
        }

        public async Task SoftDeleteAsync(User user)
        {
            user.SetStatus(UserStatus.Inactive);

            await SaveAsync(user);
        }

        #endregion CRUD
    }
}