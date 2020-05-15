using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Data.Helpers.UserConstants;

namespace AccuPay.Data.Repositories
{
    public class UserRepository
    {
        private readonly PositionViewRepository positionViewRepository;
        private readonly PayrollContext _context;

        public UserRepository(PayrollContext context,
                            PositionViewQueryBuilder positionViewQueryBuilder)
        {
            positionViewRepository = new PositionViewRepository(context, positionViewQueryBuilder);
            _context = context;
        }

        #region User List

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // builder's lifecycle should only span in each method
            // that is why we did not inject this on the constructor
            // and create a class variable of EmployeeQueryBuilder
            var builder = new UserQueryBuilder(_context);
            return await builder.
                            ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveAsync()
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.
                        IsActive().
                        ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveWithPositionAsync()
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.
                        IncludePosition().
                        IsActive().
                        ToListAsync();
        }

        #endregion User List

        #region By User

        public User GetById(int id)
        {
            var builder = new UserQueryBuilder(_context);
            return builder.
                        ById(id).
                        FirstOrDefault();
        }

        public async Task<User> GetByIdAsync(int rowId)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.
                        ById(rowId).
                        FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdWithPositionAsync(int rowId)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.
                        IncludePosition().
                        ById(rowId).
                        FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.
                        ByUsername(username).
                        FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameWithPositionAsync(string username)
        {
            var builder = new UserQueryBuilder(_context);
            return await builder.
                        IncludePosition().
                        ByUsername(username).
                        FirstOrDefaultAsync();
        }

        #endregion By User

        #region CRUD

        public async Task SaveAsync(User user)
        {
            await positionViewRepository.FillUserPositionViewAsync(positionId: user.PositionID, organizationId: user.OrganizationID, userId: user.CreatedBy.Value);

            List<User> users = new List<User>();
            users.Add(user);

            await this.SaveManyAsync(users);
        }

        public async Task SaveManyAsync(List<User> users)
        {
            var updated = users.Where(e => e.RowID.HasValue).ToList();
            if (updated.Any())
            {
                updated.ForEach(x => _context.Entry(x).State = EntityState.Modified);
            }

            var added = users.Where(e => !e.RowID.HasValue).ToList();
            if (added.Any())
            {
                // this adds a value to RowID (int minimum value)
                // so if there is a code checking for null to RowID
                // it will always be false
                _context.Users.AddRange(added);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);

            user.SetStatus(UserStatus.Inactive);

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        #endregion CRUD
    }
}