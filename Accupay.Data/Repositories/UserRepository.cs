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
        public class UserBuilder : IDisposable
        {
            private PayrollContext _context;
            private IQueryable<User> _query;

            public UserBuilder(ILoggerFactory loggerFactory = null)
            {
                if (loggerFactory != null)
                {
                    _context = new PayrollContext(loggerFactory);
                }
                else
                {
                    _context = new PayrollContext();
                }
                _query = _context.Users;
            }

            #region Builder Methods

            public UserBuilder ById(int rowId)
            {
                _query = _query.Where(u => u.RowID.Value == rowId);
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
            //using (var builder = new UserBuilder(PayrollContext.DbCommandConsoleLoggerFactory))
            using (var builder = new UserBuilder())
            {
                return await builder.ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllActiveAsync()
        {
            using (var builder = new UserBuilder())
            {
                return await builder.
                    IsActive().
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllActiveWithPositionAsync()
        {
            using (var builder = new UserBuilder())
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
            using (var builder = new UserBuilder())
            {
                return await builder.
                    ById(rowId).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetByIdWithPositionAsync(int rowId)
        {
            using (var builder = new UserBuilder())
            {
                return await builder.
                    IncludePosition().
                    ById(rowId).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using (var builder = new UserBuilder())
            {
                return await builder.
                    ByUsername(username).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetByUsernameWithPositionAsync(string username)
        {
            using (var builder = new UserBuilder())
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
            List<User> users = new List<User>();
            users.Add(user);

            await this.SaveManyAsync(users);
        }

        public async Task SaveManyAsync(List<User> users)
        {
            using (PayrollContext context = new PayrollContext(PayrollContext.DbCommandConsoleLoggerFactory))
            {
                var updated = users.Where(e => e.RowID.HasValue).ToList();
                if (updated.Any())
                {
                    updated.ForEach(x => context.Entry(x).State = EntityState.Modified);
                }

                var added = users.Where(e => !e.RowID.HasValue).ToList();
                if (added.Any())
                {
                    // this adds a value to RowID (int minimum value)
                    // so if there is a code checking for null to RowID
                    // it will always be false
                    context.Users.AddRange(added);
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                var user = await GetByIdAsync(id);

                user.SetStatus(UserStatus.Inactive);

                context.Entry(user).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        #endregion CRUD
    }
}