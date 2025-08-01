﻿Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Acupay

Namespace Global.AccuPay.Entity

    <Table("employeetimeentry")>
    Public Class TimeEntry

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property EmployeeID As Integer?

        Public Property PayRateID As Integer?

        <Column("Date")>
        Public Property EntryDate As Date

        <Column("RegularHoursWorked")>
        Public Property RegularHours As Decimal

        <Column("RegularHoursAmount")>
        Public Property RegularPay As Decimal

        <Column("OvertimeHoursWorked")>
        Public Property OvertimeHours As Decimal

        <Column("OvertimeHoursAmount")>
        Public Property OvertimePay As Decimal

        <Column("NightDifferentialHours")>
        Public Property NightDiffHours As Decimal

        <Column("NightDiffHoursAmount")>
        Public Property NightDiffPay As Decimal

        <Column("NightDifferentialOTHours")>
        Public Property NightDiffOvertimeHours As Decimal

        <Column("NightDiffOTHoursAmount")>
        Public Property NightDiffOvertimePay As Decimal

        Public Property RestDayHours As Decimal

        <Column("RestDayAmount")>
        Public Property RestDayPay As Decimal

        Public Property VacationLeaveHours As Decimal

        Public Property SickLeaveHours As Decimal

        Public Property MaternityLeaveHours As Decimal

        Public Property OtherLeaveHours As Decimal

        <Column("Leavepayment")>
        Public Property LeavePay As Decimal

        <Column("HolidayPayAmount")>
        Public Property HolidayPay As Decimal

        <Column("HoursLate")>
        Public Property LateHours As Decimal

        <Column("HoursLateAmount")>
        Public Property LateDeduction As Decimal

        Public Property UndertimeHours As Decimal

        <Column("UndertimeHoursAmount")>
        Public Property UndertimeDeduction As Decimal

        Public Property AbsentHours As Decimal

        <Column("Absent")>
        Public Property AbsentDeduction As Decimal

        <Column("BasicDayPay")>
        Public Property BasicDayPay As Decimal

        <Column("TotalDayPay")>
        Public Property TotalDayPay As Decimal

        <NotMapped>
        Public Property DutyStart As Date

        <NotMapped>
        Public Property DutyEnd As Date

        Public Sub New()
        End Sub

        Public Sub New(timeLog As TimeLog, shiftToday As CurrentShift)
            Dim timeIn = timeLog.FullTimeIn
            Dim timeOut = timeLog.FullTimeOut

            DutyStart = {timeIn, shiftToday.RangeStart}.Max
            DutyEnd = {timeOut, shiftToday.RangeEnd}.Min
        End Sub

        Public ReadOnly Property TotalLeaveHours As Decimal
            Get
                Return VacationLeaveHours + SickLeaveHours + MaternityLeaveHours + OtherLeaveHours
            End Get
        End Property

    End Class

End Namespace