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

        public UserRepository(PayrollContext context)
        {
            positionViewRepository = new PositionViewRepository(context);
            _context = context;
        }

        public class UserBuilder : IDisposable
        {
            private PayrollContext _context;
            private IQueryable<User> _query;

            public UserBuilder(PayrollContext context)
            {
                _context = context;
                _query = _context.OldUsers;
            }

            #region Builder Methods

            public UserBuilder ById(int rowId)
            {
                _query = _query.Where(u => u.RowID == rowId);
                return this;
            }

            public UserBuilder ByUsername(string username)
            {
                _query = _query.Where(u => u.Username == username);
                return this;
            }

            public UserBuilder IsActive()
            {
                _query = _query.Where(u => u.IsActive);
                return this;
            }

            public UserBuilder IncludePosition()
            {
                _query = _query.Include(u => u.Position);
                return this;
            }

            public async Task<IEnumerable<User>> ToListAsync()
            {
                return await _query.ToListAsync();
            }

            public async Task<User> FirstOrDefaultAsync()
            {
                return await _query.FirstOrDefaultAsync();
            }

            public void Dispose()
            {
                _context.Dispose();
            }

            #endregion Builder Methods
        }

        #region User List

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllActiveAsync()
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.
                    IsActive().
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllActiveWithPositionAsync()
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.
                    IncludePosition().
                    IsActive().
                    ToListAsync();
            }
        }

        #endregion User List

        #region By User

        public async Task<User> GetByIdAsync(int rowId)
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.
                    ById(rowId).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetByIdWithPositionAsync(int rowId)
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.
                IncludePosition().
                ById(rowId).
                FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.
                ByUsername(username).
                FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetByUsernameWithPositionAsync(string username)
        {
            using (var builder = new UserBuilder(_context))
            {
                return await builder.
                IncludePosition().
                ByUsername(username).
                FirstOrDefaultAsync();
            }
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
                _context.OldUsers.AddRange(added);
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