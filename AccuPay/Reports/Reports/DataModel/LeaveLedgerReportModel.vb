Option Strict On

Imports AccuPay.Core
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers

''' <summary>
''' Anemic implementation of ILeaveLedgerReportModel just for Crystal Report data source
''' </summary>
Public Class LeaveLedgerReportModel
    Implements ILeaveLedgerReportModel

    Public ReadOnly Property EmployeeNumber As String Implements ILeaveLedgerReportModel.EmployeeNumber

    Public ReadOnly Property FullName As String Implements ILeaveLedgerReportModel.FullName

    Public ReadOnly Property LeaveType As LeaveType Implements ILeaveLedgerReportModel.LeaveType

    Sub New(employeeNumber As String,
            fullName As String,
            leaveType As LeaveType,
            beginningBalance As Decimal,
            availedLeave As Decimal)

        Me.EmployeeNumber = employeeNumber
        Me.FullName = fullName
        Me.LeaveType = leaveType
        Me.BeginningBalance = beginningBalance
        Me.AvailedLeave = availedLeave
    End Sub

    Public ReadOnly Property LeaveTypeDescription As String Implements ILeaveLedgerReportModel.LeaveTypeDescription
        Get
            Select Case LeaveType
                Case LeaveType.Sick
                    Return "SL"
                Case LeaveType.Vacation
                    Return "VL"
                Case Else
                    Return ""
            End Select
        End Get

    End Property

    Public ReadOnly Property BeginningBalance() As Decimal Implements ILeaveLedgerReportModel.BeginningBalance

    Public ReadOnly Property AvailedLeave() As Decimal Implements ILeaveLedgerReportModel.AvailedLeave

    Public ReadOnly Property BeginningBalanceInDays As Decimal Implements ILeaveLedgerReportModel.BeginningBalanceInDays
        Get
            Return BeginningBalance / Helpers.PayrollTools.WorkHoursPerDay
        End Get
    End Property

    Public ReadOnly Property AvailedLeaveInDays As Decimal Implements ILeaveLedgerReportModel.AvailedLeaveInDays
        Get
            Return AvailedLeave / PayrollTools.WorkHoursPerDay
        End Get
    End Property

    Public ReadOnly Property EndingBalance As Decimal Implements ILeaveLedgerReportModel.EndingBalance
        Get
            Return BeginningBalance - AvailedLeave
        End Get
    End Property

    Public ReadOnly Property EndingBalanceInDays As Decimal Implements ILeaveLedgerReportModel.EndingBalanceInDays
        Get
            Return EndingBalance / 8
        End Get
    End Property

End Class
