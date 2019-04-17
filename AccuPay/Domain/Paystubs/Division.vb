Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("division")>
    Public Class Division

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property Name As String

        Public Property DivisionType As String

        Public Property TradeName As String

        Public Property TINNo As String

        Public Property BusinessAddress As String

        Public Property MainPhone As String

        Public Property AltPhone As String

        Public Property EmailAddress As String

        Public Property AltEmailAddress As String

        Public Property ContactName As String

        Public Property FaxNumber As String

        Public Property URL As String

        Public Property DefaultVacationLeave As Decimal?

        Public Property DefaultSickLeave As Decimal?

        Public Property GracePeriod As Decimal?

        Public Property WorkDaysPerYear As Integer?

        Public Property AutomaticOvertimeFiling As Boolean?

        <Column("PhHealthDeductSched")>
        Public Property PhilHealthDeductionSchedule As String

        <Column("PhilhealthDeductionWeekSchedule")>
        Public Property WeeklyPhilHealthDeductionSchedule As String

        <Column("PhHealthDeductSchedAgency")>
        Public Property AgencyPhilHealthDeductionSchedule As String

        <Column("PhilhealthDeductionWeekwithAgenSchedule")>
        Public Property WeeklyAgencyPhilHealthDeductionSchedule As String

        <Column("SSSDeductSched")>
        Public Property SssDeductionSchedule As String

        <Column("SSSDeductionWeekSchedule")>
        Public Property WeeklySSSDeductionSchedule As String

        <Column("SSSDeductSchedAgency")>
        Public Property AgencySssDeductionSchedule As String

        <Column("SSSDeductionWeekwithAgenSchedule")>
        Public Property WeeklyAgencySssDeductionSchedule As String

        <Column("HDMFDeductSched")>
        Public Property PagIBIGDeductionSchedule As String

        <Column("PagIbigDeductionWeekSchedule")>
        Public Property WeeklyPagIBIGDeductionSchedule As String

        <Column("HDMFDeductSchedAgency")>
        Public Property AgencyPagIBIGDeductionSchedule As String

        <Column("PagIbigDeductionWeekwithAgenSchedule")>
        Public Property WeeklyAgencyPagIBIGDeductionSchedule As String

        <Column("WTaxDeductSched")>
        Public Property WithholdingTaxSchedule As String

        <Column("WithholdingTaxDeductionWeekSchedule")>
        Public Property WeeklyWithholdingTaxSchedule As String

        <Column("WTaxDeductSchedAgency")>
        Public Property AgencyWithholdingTaxSchedule As String

        <Column("WithholdingTaxDeductionWeekwithAgenSchedule")>
        Public Property WeeklyAgencyWithholdingTaxSchedule As String

        Public Property ParentDivisionID As Integer?

        <ForeignKey("ParentDivisionID")>
        Public Overridable Property ParentDivision As Division

        Public Property DivisionHeadID As Integer?

        <ForeignKey("DivisionHeadID")>
        Public Overridable Property DivisionHead As Position

        Public Property PayFrequencyID As Integer?

        <ForeignKey("PayFrequencyID")>
        Public Overridable Property PayFrequency As PayFrequency

        Public ReadOnly Property IsRoot As Boolean
            Get
                Return ParentDivisionID Is Nothing
            End Get
        End Property

        Public ReadOnly Property FullDivisionName As String
            Get
                Dim parentName = If(ParentDivision Is Nothing, "", ParentDivision.Name & " - ")

                Return parentName & Name
            End Get
        End Property

        Public Function IsParent(division As Division) As Boolean
            Return Nullable.Equals(ParentDivisionID, division.RowID)
        End Function

    End Class

End Namespace
