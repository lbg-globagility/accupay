using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionViewRepository
    {
        private class PositionViewBuilder : IDisposable
        {
            private PayrollContext _context;
            private IQueryable<PositionView> _query;

            public PositionViewBuilder(ILoggerFactory loggerFactory = null)
            {
                if (loggerFactory != null)
                {
                    _context = new PayrollContext(loggerFactory);
                }
                else
                {
                    _context = new PayrollContext();
                }
                _query = _context.PositionViews;
            }

            public PositionViewBuilder ByOrganizationId(int organizationId)
            {
                _query = _query.Where(u => u.OrganizationID == organizationId);
                return this;
            }

            public PositionViewBuilder ByPositionId(int positionId)
            {
                _query = _query.Where(u => u.PositionID == positionId);
                return this;
            }

            public PositionViewBuilder IncludeView()
            {
                _query = _query.Include(p => p.View);
                return this;
            }

            public PositionViewBuilder IncludePosition()
            {
                _query = _query.Include(p => p.Position);
                return this;
            }

            public async Task<IEnumerable<PositionView>> ToListAsync()
            {
                return await _query.ToListAsync();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }

        public async Task FillUserPositionViewAsync(int positionId, int organizationId, int userId)
        {
            using (var builder = new PositionViewBuilder())
            {
                var jobPositionView = await builder.
                    ByOrganizationId(organizationId).
                    ByPositionId(positionId).
                    ToListAsync();

                // if no PositionView fetched, base on PositionId and OrganizationId given,
                // this code should provide for it
                using (var context = new PayrollContext())
                {
                    var mockPositionViews = await ImitationPositionViews(positionId, userId, context);

                    if (!jobPositionView.Any())
                    {
                        context.PositionViews.AddRange(mockPositionViews);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var jobPositionViewPositionIds = jobPositionView.Select(p => p.PositionID).ToList();
                        var jobPositionViewViewIds = jobPositionView.Select(p => p.ViewID).ToList();
                        var jobPositionViewOrganizationIds = jobPositionView.Select(p => p.OrganizationID).ToList();
                        var missingPositionViews = mockPositionViews.
                            Where(p => !jobPositionViewPositionIds.Contains(p.PositionID)).
                            Where(p => !jobPositionViewViewIds.Contains(p.ViewID)).
                            Where(p => !jobPositionViewOrganizationIds.Contains(p.OrganizationID)).
                            ToList();

                        if (missingPositionViews.Any())
                        {
                            context.PositionViews.AddRange(mockPositionViews);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private async Task<List<PositionView>> ImitationPositionViews(int positionId, int userId, PayrollContext context)
        {
            var position = await context.Positions.FindAsync(positionId);
            //var samePositions = await positionRepository.GetAllByNameAsync(position.Name);

            var privileges = await context.Privileges.ToListAsync();

            var newPositionViews = new List<PositionView>();
            foreach (var view in privileges)
            {
                var viewId = view.RowID;
                var viewOrganizationId = view.OrganizationID.Value;

                //foreach (var p in samePositions)
                //{
                var newPositionView = PositionView.NewPositionView(viewOrganizationId, userId);

                newPositionView.ViewID = viewId;
                newPositionView.PositionID = position.RowID;// p.RowID;

                newPositionViews.Add(newPositionView);
                //}
            }

            return newPositionViews;
        }
    }
}