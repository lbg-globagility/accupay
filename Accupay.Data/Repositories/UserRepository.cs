using AccuPay.Data.Entities;
using AccuPay.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccuPay.Data.Helpers.UserConstants;

namespace AccuPay.Data.Repositories
{
    public class UserRepository : SavableRepository<User>
    {
        private readonly IEncryption _encryption;

        public UserRepository(PayrollContext context, IEncryption encryption) : base(context)
        {
            _encryption = encryption;
        }

        #region Save

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

        public async Task<string> GetUniqueUsernameAsync(string username, int? clientId = null)
        {
            if (clientId.HasValue)
            {
                if (await CheckIfUsernameExistsAsync(username))
                {
                    username = $"{username} - {clientId}";
                }
                else
                {
                    return username;
                }
            }

            int counter = 0;
            while (await CheckIfUsernameExistsAsync(username))
            {
                username = $"{username} - {clientId}[{counter}]";
                counter++;
            }

            return username;
        }

        #endregion Save

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

        public async Task<User> GetByIdWithPositionAsync(int id)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .ById(id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByAspNetUserIdAsync(Guid aspNetUserId)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder
                .ByAspNetUserId(aspNetUserId)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetFirstUserAsync()
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfUsernameExistsAsync(string username, bool isIncrypted = false)
        {
            if (isIncrypted == false)
            {
                username = _encryption.Encrypt(username);
            }

            var builder = new UserQueryBuilder(_context);
            return await builder
                .ByUsername(username)
                .AnyAsync();
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
    }
}