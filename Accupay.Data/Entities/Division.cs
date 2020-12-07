using AccuPay.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("division")]
    public class Division : OrganizationalEntity
    {
        public const string DefaultLocationName = "Default Location";
        public const string DefaultDivisionName = "Default Division";

        public string Name { get; set; }

        public string DivisionType { get; set; }

        public string TradeName { get; set; }

        public string TINNo { get; set; }

        public string BusinessAddress { get; set; }

        public string MainPhone { get; set; }

        public string AltPhone { get; set; }

        public string EmailAddress { get; set; }

        public string AltEmailAddress { get; set; }

        public string ContactName { get; set; }

        public string FaxNumber { get; set; }

        public string URL { get; set; }

        public decimal? DefaultVacationLeave { get; set; }

        public decimal? DefaultSickLeave { get; set; }

        public decimal? DefaultOtherLeave { get; set; }

        public decimal? GracePeriod { get; set; }

        public int? WorkDaysPerYear { get; set; }

        public bool? AutomaticOvertimeFiling { get; set; }

        [Column("PhHealthDeductSched")]
        public string PhilHealthDeductionSchedule { get; set; }

        [Column("PhilhealthDeductionWeekSchedule")]
        public string WeeklyPhilHealthDeductionSchedule { get; set; }

        [Column("PhHealthDeductSchedAgency")]
        public string AgencyPhilHealthDeductionSchedule { get; set; }

        [Column("PhilhealthDeductionWeekwithAgenSchedule")]
        public string WeeklyAgencyPhilHealthDeductionSchedule { get; set; }

        [Column("SSSDeductSched")]
        public string SssDeductionSchedule { get; set; }

        [Column("SSSDeductionWeekSchedule")]
        public string WeeklySSSDeductionSchedule { get; set; }

        [Column("SSSDeductSchedAgency")]
        public string AgencySssDeductionSchedule { get; set; }

        [Column("SSSDeductionWeekwithAgenSchedule")]
        public string WeeklyAgencySssDeductionSchedule { get; set; }

        [Column("HDMFDeductSched")]
        public string PagIBIGDeductionSchedule { get; set; }

        [Column("PagIbigDeductionWeekSchedule")]
        public string WeeklyPagIBIGDeductionSchedule { get; set; }

        [Column("HDMFDeductSchedAgency")]
        public string AgencyPagIBIGDeductionSchedule { get; set; }

        [Column("PagIbigDeductionWeekwithAgenSchedule")]
        public string WeeklyAgencyPagIBIGDeductionSchedule { get; set; }

        [Column("WTaxDeductSched")]
        public string WithholdingTaxSchedule { get; set; }

        [Column("WithholdingTaxDeductionWeekSchedule")]
        public string WeeklyWithholdingTaxSchedule { get; set; }

        [Column("WTaxDeductSchedAgency")]
        public string AgencyWithholdingTaxSchedule { get; set; }

        [Column("WithholdingTaxDeductionWeekwithAgenSchedule")]
        public string WeeklyAgencyWithholdingTaxSchedule { get; set; }

        public int? ParentDivisionID { get; set; }

        [ForeignKey("ParentDivisionID")]
        public virtual Division ParentDivision { get; set; }

        public int? DivisionHeadID { get; set; }

        [ForeignKey("DivisionHeadID")]
        public virtual Position DivisionHead { get; set; }

        public int? PayFrequencyID { get; set; }

        public int? DivisionUniqueID { get; set; }

        public decimal? MinimumWageAmount { get; set; }

        // calling this from EF core would not result to this
        // being translated into sql queries. Instead the database will
        // query all data then filter this in memory
        public bool IsRoot => ParentDivisionID == null;

        public string FullDivisionName
        {
            get
            {
                var parentName = ParentDivision == null ? "" : ParentDivision.Name + " - ";

                return parentName + Name;
            }
        }

        public bool IsParent(Division division)
        {
            return ParentDivisionID == division.RowID;
        }

        public static Division NewDivision(int organizationId)
        {
            return new Division()
            {
                OrganizationID = organizationId,
                GracePeriod = 0,
                WorkDaysPerYear = 313,
                PhilHealthDeductionSchedule = ContributionSchedule.END_OF_THE_MONTH,
                PagIBIGDeductionSchedule = ContributionSchedule.END_OF_THE_MONTH,
                SssDeductionSchedule = ContributionSchedule.END_OF_THE_MONTH,
                WithholdingTaxSchedule = ContributionSchedule.END_OF_THE_MONTH,
                DefaultVacationLeave = 0,
                DefaultSickLeave = 0,
                DefaultOtherLeave = 0,
                AgencyPhilHealthDeductionSchedule = ContributionSchedule.END_OF_THE_MONTH,
                AgencyPagIBIGDeductionSchedule = ContributionSchedule.END_OF_THE_MONTH,
                AgencySssDeductionSchedule = ContributionSchedule.END_OF_THE_MONTH,
                AgencyWithholdingTaxSchedule = ContributionSchedule.END_OF_THE_MONTH,
                DivisionUniqueID = 1,
                MinimumWageAmount = 537,
                AutomaticOvertimeFiling = false,
                WeeklySSSDeductionSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyPhilHealthDeductionSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyAgencyPagIBIGDeductionSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyWithholdingTaxSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyAgencySssDeductionSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyAgencyPhilHealthDeductionSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyPagIBIGDeductionSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH,
                WeeklyAgencyWithholdingTaxSchedule = ContributionSchedule.LAST_WEEK_OF_THE_MONTH
            };
        }
    }
}
