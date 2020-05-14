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
        private readonly UserQueryBuilder _builder;

        public UserRepository(PayrollContext context,
                            UserQueryBuilder userQueryBuilder,
                            PositionViewQueryBuilder positionViewQueryBuilder)
        {
            positionViewRepository = new PositionViewRepository(context, positionViewQueryBuilder);
            _context = context;
            _builder = userQueryBuilder;
        }

        #region User List

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _builder.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveAsync()
        {
            return await _builder.
                IsActive().
                ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveWithPositionAsync()
        {
            return await _builder.
                IncludePosition().
                IsActive().
                ToListAsync();
        }

        #endregion User List

        #region By User

        public User GetById(int id)
        {
            return _builder.
                    ById(id).
                    FirstOrDefault();
        }

        public async Task<User> GetByIdAsync(int rowId)
        {
            return await _builder.
                ById(rowId).
                FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdWithPositionAsync(int rowId)
        {
            return await _builder.
            IncludePosition().
            ById(rowId).
            FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _builder.
            ByUsername(username).
            FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameWithPositionAsync(string username)
        {
            return await _builder.
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