Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employee")>
    Public Class Employee

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property OrganizationID As Integer?

        Public Property PositionID As Integer?

        Public Property PayFrequencyID As Integer?

        Public Property Salutation As String

        Public Property FirstName As String

        Public Property MiddleName As String

        Public Property LastName As String

        Public Property Surname As String

        <Column("EmployeeID")>
        Public Property EmployeeNo As String

        Public Property TinNo As String

        Public Property SssNo As String

        Public Property HdmfNo As String

        Public Property PhilHealthNo As String

        Public Property EmploymentStatus As String

        Public Property EmailAddress As String

        Public Property WorkPhone As String

        Public Property HomePhone As String

        Public Property MobilePhone As String

        Public Property HomeAddress As String

        Public Property Nickname As String

        Public Property JobTitle As String

        Public Property Gender As String

        Public Property EmployeeType As String

        Public Property MaritalStatus As String

        Public Property BirthDate As Date

        Public Property StartDate As Date

        Public Property TerminationDate As Date?

        Public Property NoOfDependents As Integer?

        Public Property UndertimeOverride As Boolean

        Public Property OvertimeOverride As Boolean

        Public Property NewEmployeeFlag As Boolean

        Public Property LeaveBalance As Decimal

        Public Property SickLeaveBalance As Decimal

        Public Property MaternityLeaveBalance As Decimal

        Public Property OtherLeaveBalance As Decimal

        <Column("LeaveAllowance")>
        Public Property VacationLeaveAllowance As Decimal

        Public Property SickLeaveAllowance As Decimal

        Public Property MaternityLeaveAllowance As Decimal

        Public Property OtherLeaveAllowance As Decimal

        Public Property LeavePerPayPeriod As Decimal

        Public Property SickLeavePerPayPeriod As Decimal

        Public Property MaternityLeavePerPayPeriod As Decimal

        Public Property OtherLeavePerPayPeriod As Decimal

        Public Property AlphalistExempted As Boolean

        Public Property WorkDaysPerYear As Decimal

        Public Property DayOfRest As Integer?

        Public Property AtmNo As String

        Public Property BankName As String

        Public Property CalcHoliday As Boolean

        Public Property CalcSpecialHoliday As Boolean

        Public Property CalcNightDiff As Boolean

        Public Property CalcNightDiffOT As Boolean

        Public Property CalcRestDay As Boolean

        Public Property CalcRestDayOT As Boolean

        Public Property DateRegularized As Date?

        Public Property DateEvaluated As Date?

        Public Property RevealInPayroll As Boolean

        Public Property LateGracePeriod As Decimal

        Public Property OffsetBalance As Decimal

        Public Property AgencyID As Integer?

        Public Property Image As Byte()

        Public Property AdvancementPoints As Integer

        <ForeignKey("PositionID")>
        Public Overridable Property Position As Position

        <ForeignKey("PayFrequencyID")>
        Public Overridable Property PayFrequency As PayFrequency

        Public ReadOnly Property MiddleInitial As String
            Get
                Return If(String.IsNullOrEmpty(MiddleName), Nothing, MiddleName.Substring(0, 1))
            End Get
        End Property

        Public ReadOnly Property IsDaily As Boolean
            Get
                Return (EmployeeType.ToLower = "daily") '"Daily"
            End Get
        End Property

        Public ReadOnly Property IsMonthly As Boolean
            Get
                Return (EmployeeType.ToLower = "monthly") '"Monthly"
            End Get
        End Property

        Public ReadOnly Property IsFixed As Boolean
            Get
                Return (EmployeeType.ToLower = "fixed") '"Fixed"
            End Get
        End Property

        Public ReadOnly Property IsWeeklyPaid As Boolean
            Get
                Return PayFrequencyID.Value = 4
            End Get
        End Property

        Public ReadOnly Property FullNameLastNameFirst As String
            Get
                Return $"{LastName}, {FirstName}"
            End Get
        End Property

        Public ReadOnly Property FullNameWithMiddleInitialLastNameFirst As String
            Get
                Return $"{LastName}, {FirstName} {MiddleInitial}"
            End Get
        End Property

        Public ReadOnly Property FullNameWithMiddleInitial As String
            Get
                Return $"{FirstName} {If(MiddleInitial Is Nothing, "", MiddleInitial & ". ")}{LastName}"
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return $"{FirstName} {LastName}"
            End Get
        End Property

        Public ReadOnly Property EmployeeIdWithPositionAndEmployeeType As String
            Get
                Return $"ID# {EmployeeNo}, {Position?.Name}, {EmployeeType} Salary"
            End Get
        End Property

        Public ReadOnly Property IsUnderAgency As Boolean
            Get
                Return AgencyID.HasValue
            End Get
        End Property

        Public ReadOnly Property SssSchedule As String
            Get
                Return If(
                    IsUnderAgency,
                    Position?.Division?.AgencySssDeductionSchedule,
                    Position?.Division?.SssDeductionSchedule)
            End Get
        End Property

        Public ReadOnly Property PhilHealthSchedule As String
            Get
                Return If(
                    IsUnderAgency,
                    Position?.Division?.AgencyPhilHealthDeductionSchedule,
                    Position?.Division?.PhilHealthDeductionSchedule)
            End Get
        End Property

        Public ReadOnly Property PagIBIGSchedule As String
            Get
                Return If(
                    IsUnderAgency,
                    Position?.Division?.AgencyPagIBIGDeductionSchedule,
                    Position?.Division?.PagIBIGDeductionSchedule)
            End Get
        End Property

        Public ReadOnly Property WithholdingTaxSchedule As String
            Get
                Return If(
                    IsUnderAgency,
                    Position?.Division?.AgencyWithholdingTaxSchedule,
                    Position?.Division?.WithholdingTaxSchedule)
            End Get
        End Property

        Public ReadOnly Property IsActive As Boolean
            Get
                Return IsResigned = False AndAlso IsTerminated = False AndAlso IsRetired = False
            End Get
        End Property

        Public ReadOnly Property IsResigned As Boolean
            Get
                Return EmploymentStatus.Trim().ToUpper() = "RESIGNED"
            End Get
        End Property

        Public ReadOnly Property IsTerminated As Boolean
            Get
                Return EmploymentStatus.Trim().ToUpper() = "TERMINATED"
            End Get
        End Property

        Public ReadOnly Property IsRetired As Boolean
            Get
                Return EmploymentStatus.ToUpper().Trim() = "RETIRED"
            End Get
        End Property

    End Class

End Namespace