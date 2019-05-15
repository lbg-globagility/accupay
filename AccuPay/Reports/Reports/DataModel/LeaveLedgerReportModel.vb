Public Class LeaveLedgerReportModel

    Public Property EmployeeNumber As String

    Public Property FullName As String

    Public Property LeaveType As LeaveType.LeaveType

    Sub New(employeeNumber As String,
            fullName As String,
            leaveType As LeaveType.LeaveType,
            beginningBalance As Decimal,
            availedLeave As Decimal,
            endingBalance As Decimal)
    End Sub


    Public ReadOnly Property LeaveTypeDescription As String
        Get
            Select Case LeaveType
                Case AccuPay.LeaveType.LeaveType.Sick
                    Return "SL"
                Case AccuPay.LeaveType.LeaveType.Vacation
                    Return "VL"
                Case Else
                    Return ""
            End Select
        End Get

    End Property

    Private _beginningBalance As Decimal
    Public Property BeginningBalance() As Decimal
        Get
            Return _beginningBalance
        End Get
        Set(ByVal value As Decimal)
            _beginningBalance = AccuMath.CommercialRound(value)
        End Set
    End Property

    Private _availedLeave As Decimal
    Public Property AvailedLeave() As Decimal
        Get
            Return _availedLeave
        End Get
        Set(ByVal value As Decimal)
            _availedLeave = AccuMath.CommercialRound(value)
        End Set
    End Property

    Public ReadOnly Property BeginningBalanceInDays As Decimal
        Get
            Return BeginningBalance / PayrollTools.WorkHoursPerDay
        End Get
    End Property

    Public ReadOnly Property AvailedLeaveInDays As Decimal
        Get
            Return AvailedLeave / PayrollTools.WorkHoursPerDay
        End Get
    End Property

    Public ReadOnly Property EndingBalance As Decimal
        Get
            Return BeginningBalance - AvailedLeave
        End Get
    End Property

    Public ReadOnly Property EndingBalanceInDays As Decimal
        Get
            Return EndingBalance / 8
        End Get
    End Property


End Class
