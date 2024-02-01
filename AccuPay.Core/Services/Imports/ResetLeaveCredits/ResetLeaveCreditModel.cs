using System;
using System.Collections.Generic;
using System.Linq;
using AccuPay.Core.Entities;

namespace AccuPay.Core.Services.Imports.ResetLeaveCredits
{
    public class ResetLeaveCreditModel
    {
        public ResetLeaveCreditModel(ResetLeaveCredit entity)
        {
            OriginalResetLeaveCredit = entity;
            Id = entity.RowID;
            StartPeriodId = entity.StartPeriodId;

            PayPeriod = entity.PayPeriod;
            if (PayPeriod != null)
            {
                var fsfs = PayPeriod.Half == 0 ? "2nd" : "1st";
                PeriodText = $"{new DateTime(PayPeriod.Year, PayPeriod.Month, 1):d} {fsfs} half {PayPeriod.Year}";
                PeriodDisplay = $"{PayPeriod.PayFromDate:d}";
            }

            if (entity.ResetLeaveCreditItems != null)
                Items = entity.ResetLeaveCreditItems.Select(i => new ResetLeaveCreditItemModel(i)).ToList();
        }

        public ResetLeaveCredit OriginalResetLeaveCredit { get; }
        public int? Id { get; }
        public int? StartPeriodId { get; }
        public PayPeriod PayPeriod { get; }
        public string PeriodText { get; set; }
        public string PeriodDisplay { get; set; }
        public List<ResetLeaveCreditItemModel> Items { get; set; }
        public bool IsNew => Id == null;
        public bool HasChanged => Items.Where(i => i.HasChanged).Any();
        public bool HasSelections => Items != null && Items.Where(i => !i.IsApplied && i.IsSelected).Any();
        public bool HasNotApplied => Items != null && Items.Where(i => !i.IsApplied).Any();
        public bool HasValues => Items != null && Items
            .Where(i => !i.IsApplied && i.IsSelected)
            .Where(i => i.VacationLeaveCredit > 0 || i.SickLeaveCredit > 0)
            .Any();
    }
}
