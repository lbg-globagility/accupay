Option Strict On

Imports AccuPay.Entity
Imports FluentNHibernate.Mapping

Public Class PaystubMap
    Inherits ClassMap(Of Paystub)

    Public Sub New()
        Table("paystub")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.PayPeriodID)
        Map(Function(x) x.EmployeeID)
        Map(Function(x) x.TimeEntryID)
        Map(Function(x) x.PayFromdate)
        Map(Function(x) x.PayToDate)

        Map(Function(x) x.RegularHours)
        Map(Function(x) x.RegularPay)
        Map(Function(x) x.OvertimeHours)
        Map(Function(x) x.OvertimePay)
        Map(Function(x) x.NightDiffHours)
        Map(Function(x) x.NightDiffPay)
        Map(Function(x) x.NightDiffOvertimeHours)
        Map(Function(x) x.NightDiffOvertimePay)
        Map(Function(x) x.RestDayHours)
        Map(Function(x) x.RestDayPay)
        Map(Function(x) x.RestDayOTHours)
        Map(Function(x) x.RestDayOTPay)
        Map(Function(x) x.LeaveHours)
        Map(Function(x) x.LeavePay)
        Map(Function(x) x.SpecialHolidayHours)
        Map(Function(x) x.SpecialHolidayPay)
        Map(Function(x) x.SpecialHolidayOTHours)
        Map(Function(x) x.SpecialHolidayOTPay)
        Map(Function(x) x.RegularHolidayHours)
        Map(Function(x) x.RegularHolidayPay)
        Map(Function(x) x.RegularHolidayOTHours)
        Map(Function(x) x.RegularHolidayOTPay)
        Map(Function(x) x.HolidayPay)
        Map(Function(x) x.LateHours)
        Map(Function(x) x.LateDeduction)
        Map(Function(x) x.UndertimeHours)
        Map(Function(x) x.UndertimeDeduction)
        Map(Function(x) x.AbsentHours)
        Map(Function(x) x.AbsenceDeduction)
        Map(Function(x) x.TotalEarnings).Column("WorkPay")

        Map(Function(x) x.TotalBonus)
        Map(Function(x) x.TotalAllowance)
        Map(Function(x) x.GrossPay).Column("TotalGrossSalary")
        Map(Function(x) x.TaxableIncome).Column("TotalTaxableSalary")
        Map(Function(x) x.WithholdingTax).Column("TotalEmpWithholdingTax")
        Map(Function(x) x.SssEmployeeShare).Column("TotalEmpSSS")
        Map(Function(x) x.SssEmployerShare).Column("TotalCompSSS")
        Map(Function(x) x.PhilHealthEmployeeShare).Column("TotalEmpPhilHealth")
        Map(Function(x) x.PhilHealthEmployerShare).Column("TotalCompPhilHealth")
        Map(Function(x) x.HdmfEmployeeShare).Column("TotalEmpHDMF")
        Map(Function(x) x.HdmfEmployerShare).Column("TotalCompHDMF")
        Map(Function(x) x.TotalVacationDaysLeft)
        Map(Function(x) x.TotalUndeclaredSalary)
        Map(Function(x) x.TotalLoans)
        Map(Function(x) x.TotalAdjustments)
        Map(Function(x) x.NetPay).Column("TotalNetSalary")
        Map(Function(x) x.ThirteenthMonthInclusion)
        Map(Function(x) x.FirstTimeSalary)

        HasOne(Function(x) x.ThirteenthMonthPay).PropertyRef(Function(x) x.Paystub).Cascade.All()
        HasMany(Function(x) x.AllowanceItems).Inverse().Cascade.All()
    End Sub

End Class
