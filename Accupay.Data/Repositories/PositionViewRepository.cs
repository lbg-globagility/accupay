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
        private readonly PayrollContext _context;
        private readonly PositionViewQueryBuilder _builder;

        public PositionViewRepository(PayrollContext context, PositionViewQueryBuilder builder)
        {
            this._context = context;
            this._builder = builder;
        }

        public async Task FillUserPositionViewAsync(int positionId, int organizationId, int userId)
        {
            var jobPositionView = await _builder.
                ByOrganizationId(organizationId).
                ByPositionId(positionId).
                ToListAsync();

            // if no PositionView fetched, base on PositionId and OrganizationId given,
            // this code should provide for it
            var mockPositionViews = await ImitationPositionViews(positionId, userId);

            if (!jobPositionView.Any())
            {
                _context.PositionViews.AddRange(mockPositionViews);
                await _context.SaveChangesAsync();
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
                    _context.PositionViews.AddRange(mockPositionViews);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task<List<PositionView>> ImitationPositionViews(int positionId, int userId)
        {
            var position = await _context.Positions.FindAsync(positionId);
            //var samePositions = await positionRepository.GetAllByNameAsync(position.Name);

            var privileges = await _context.Privileges.ToListAsync();

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