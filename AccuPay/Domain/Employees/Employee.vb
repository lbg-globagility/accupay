Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data
Imports AccuPay.Enums
Imports PayrollSys

Namespace Global.AccuPay.Entity

    <Table("employee")>
    Public Class Employee
        Implements IEmployee

        <Key>
        Public Property RowID As Integer? Implements IEmployee.RowID

        Public Property Created As Date Implements IEmployee.Created
        Public Property CreatedBy As Integer? Implements IEmployee.CreatedBy
        Public Property LastUpd As Date? Implements IEmployee.LastUpd
        Public Property LastUpdBy As Integer? Implements IEmployee.LastUpdBy
        Public Property OrganizationID As Integer? Implements IEmployee.OrganizationID
        Public Property PositionID As Integer? Implements IEmployee.PositionID
        Public Property PayFrequencyID As Integer? Implements IEmployee.PayFrequencyID
        Public Property Salutation As String Implements IEmployee.Salutation
        Public Property FirstName As String Implements IEmployee.FirstName
        Public Property MiddleName As String Implements IEmployee.MiddleName
        Public Property LastName As String Implements IEmployee.LastName
        Public Property Surname As String Implements IEmployee.Surname

        <Column("EmployeeID")>
        Public Property EmployeeNo As String Implements IEmployee.EmployeeNo

        Public Property TinNo As String Implements IEmployee.TinNo
        Public Property SssNo As String Implements IEmployee.SssNo
        Public Property HdmfNo As String Implements IEmployee.HdmfNo
        Public Property PhilHealthNo As String Implements IEmployee.PhilHealthNo
        Public Property EmploymentStatus As String Implements IEmployee.EmploymentStatus
        Public Property EmailAddress As String Implements IEmployee.EmailAddress
        Public Property WorkPhone As String Implements IEmployee.WorkPhone
        Public Property HomePhone As String Implements IEmployee.HomePhone
        Public Property MobilePhone As String Implements IEmployee.MobilePhone
        Public Property HomeAddress As String Implements IEmployee.HomeAddress
        Public Property Nickname As String Implements IEmployee.Nickname
        Public Property JobTitle As String Implements IEmployee.JobTitle
        Public Property Gender As String Implements IEmployee.Gender
        Public Property EmployeeType As String Implements IEmployee.EmployeeType
        Public Property MaritalStatus As String Implements IEmployee.MaritalStatus
        Public Property BirthDate As Date Implements IEmployee.BirthDate
        Public Property StartDate As Date Implements IEmployee.StartDate
        Public Property TerminationDate As Date? Implements IEmployee.TerminationDate
        Public Property NoOfDependents As Integer? Implements IEmployee.NoOfDependents
        Public Property UndertimeOverride As Boolean Implements IEmployee.UndertimeOverride
        Public Property OvertimeOverride As Boolean Implements IEmployee.OvertimeOverride
        Public Property NewEmployeeFlag As Boolean Implements IEmployee.NewEmployeeFlag
        Public Property LeaveBalance As Decimal Implements IEmployee.LeaveBalance
        Public Property SickLeaveBalance As Decimal Implements IEmployee.SickLeaveBalance
        Public Property MaternityLeaveBalance As Decimal Implements IEmployee.MaternityLeaveBalance
        Public Property OtherLeaveBalance As Decimal Implements IEmployee.OtherLeaveBalance

        <Column("LeaveAllowance")>
        Public Property VacationLeaveAllowance As Decimal Implements IEmployee.LeaveAllowance

        Public Property SickLeaveAllowance As Decimal Implements IEmployee.SickLeaveAllowance
        Public Property MaternityLeaveAllowance As Decimal Implements IEmployee.MaternityLeaveAllowance
        Public Property OtherLeaveAllowance As Decimal Implements IEmployee.OtherLeaveAllowance
        Public Property LeavePerPayPeriod As Decimal Implements IEmployee.LeavePerPayPeriod
        Public Property SickLeavePerPayPeriod As Decimal Implements IEmployee.SickLeavePerPayPeriod
        Public Property MaternityLeavePerPayPeriod As Decimal Implements IEmployee.MaternityLeavePerPayPeriod
        Public Property OtherLeavePerPayPeriod As Decimal Implements IEmployee.OtherLeavePerPayPeriod
        Public Property AlphalistExempted As Boolean Implements IEmployee.AlphalistExempted
        Public Property WorkDaysPerYear As Decimal Implements IEmployee.WorkDaysPerYear
        Public Property DayOfRest As Integer? Implements IEmployee.DayOfRest
        Public Property AtmNo As String Implements IEmployee.AtmNo
        Public Property BankName As String Implements IEmployee.BankName
        Public Property CalcHoliday As Boolean Implements IEmployee.CalcHoliday
        Public Property CalcSpecialHoliday As Boolean Implements IEmployee.CalcSpecialHoliday
        Public Property CalcNightDiff As Boolean Implements IEmployee.CalcNightDiff
        Public Property CalcNightDiffOT As Boolean Implements IEmployee.CalcNightDiffOT
        Public Property CalcRestDay As Boolean Implements IEmployee.CalcRestDay
        Public Property CalcRestDayOT As Boolean Implements IEmployee.CalcRestDayOT
        Public Property DateRegularized As Date? Implements IEmployee.DateRegularized
        Public Property DateEvaluated As Date? Implements IEmployee.DateEvaluated
        Public Property RevealInPayroll As Boolean Implements IEmployee.RevealInPayroll
        Public Property LateGracePeriod As Decimal Implements IEmployee.LateGracePeriod
        Public Property OffsetBalance As Decimal Implements IEmployee.OffsetBalance
        Public Property AgencyID As Integer? Implements IEmployee.AgencyID
        Public Property Image As Byte() Implements IEmployee.Image
        Public Property AdvancementPoints As Integer Implements IEmployee.AdvancementPoints
        Public Property BPIInsurance As Decimal Implements IEmployee.BPIInsurance
        Public Property BranchID As Integer?

        <ForeignKey("PositionID")>
        Public Overridable Property Position As Position

        <ForeignKey("PayFrequencyID")>
        Public Overridable Property PayFrequency As PayFrequency

        Public Overridable Property Salaries As ICollection(Of Salary)

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
                Return PayFrequencyID.Value = PayFrequencyType.Weekly
            End Get
        End Property

        Public ReadOnly Property IsPremiumInclusive As Boolean
            Get
                Return IsMonthly OrElse IsFixed
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
