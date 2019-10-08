Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Benchmark

    Public Class BenchmarkPaystubRate
        Implements IPaystubRate

        Public Const WorkHoursPerDay As Decimal = 8

        Public Property Employee As Employee
        Public Property Salary As Salary
        Public Property MonthlyRate As Decimal
        Public Property DailyRate As Decimal
        Public Property HourlyRate As Decimal
        Public Property ActualMonthlyRate As Decimal
        Public Property ActualDailyRate As Decimal
        Public Property ActualHourlyRate As Decimal

        Sub New(employee As Employee, salary As Salary)

            Me.Employee = employee

            Me.Salary = salary

            Me.MonthlyRate = AccuMath.CommercialRound(PayrollTools.GetEmployeeMonthlyRate(employee, Me.Salary), 4)
            Me.DailyRate = AccuMath.CommercialRound(PayrollTools.GetDailyRate(Me.MonthlyRate, employee.WorkDaysPerYear), 4)
            Me.HourlyRate = AccuMath.CommercialRound(PayrollTools.GetHourlyRateByDailyRate(Me.DailyRate), 4)

            Me.ActualMonthlyRate = AccuMath.CommercialRound(PayrollTools.GetEmployeeMonthlyRate(employee, Me.Salary, isActual:=True), 4)
            Me.ActualDailyRate = AccuMath.CommercialRound(PayrollTools.GetDailyRate(Me.ActualMonthlyRate, employee.WorkDaysPerYear), 4)
            Me.ActualHourlyRate = AccuMath.CommercialRound(PayrollTools.GetHourlyRateByDailyRate(Me.ActualDailyRate), 4)

        End Sub

        Public Function IsInvalid() As Boolean

            Return Me.Employee Is Nothing OrElse Me.Salary Is Nothing

        End Function

        Public Function AllowanceSalary() As Decimal

            Return Me.Salary.AllowanceSalary

        End Function

        Public Property RegularHours As Decimal Implements IPaystubRate.RegularHours
        Public Property RegularPay As Decimal Implements IPaystubRate.RegularPay
        Public Property ActualRegularPay As Decimal Implements IPaystubRate.ActualRegularPay

        Public Property OvertimeHours As Decimal Implements IPaystubRate.OvertimeHours
        Public Property OvertimePay As Decimal Implements IPaystubRate.OvertimePay
        Public Property ActualOvertimePay As Decimal Implements IPaystubRate.ActualOvertimePay

        Public Property NightDiffHours As Decimal Implements IPaystubRate.NightDiffHours
        Public Property NightDiffPay As Decimal Implements IPaystubRate.NightDiffPay
        Public Property ActualNightDiffPay As Decimal Implements IPaystubRate.ActualNightDiffPay

        Public Property NightDiffOvertimeHours As Decimal Implements IPaystubRate.NightDiffOvertimeHours
        Public Property NightDiffOvertimePay As Decimal Implements IPaystubRate.NightDiffOvertimePay
        Public Property ActualNightDiffOvertimePay As Decimal Implements IPaystubRate.ActualNightDiffOvertimePay

        Public Property RestDayHours As Decimal Implements IPaystubRate.RestDayHours
        Public Property RestDayPay As Decimal Implements IPaystubRate.RestDayPay
        Public Property ActualRestDayPay As Decimal Implements IPaystubRate.ActualRestDayPay

        Public Property RestDayOTHours As Decimal Implements IPaystubRate.RestDayOTHours
        Public Property RestDayOTPay As Decimal Implements IPaystubRate.RestDayOTPay
        Public Property ActualRestDayOTPay As Decimal Implements IPaystubRate.ActualRestDayOTPay

        Public Property SpecialHolidayHours As Decimal Implements IPaystubRate.SpecialHolidayHours
        Public Property SpecialHolidayPay As Decimal Implements IPaystubRate.SpecialHolidayPay
        Public Property ActualSpecialHolidayPay As Decimal Implements IPaystubRate.ActualSpecialHolidayPay

        Public Property SpecialHolidayOTHours As Decimal Implements IPaystubRate.SpecialHolidayOTHours
        Public Property SpecialHolidayOTPay As Decimal Implements IPaystubRate.SpecialHolidayOTPay
        Public Property ActualSpecialHolidayOTPay As Decimal Implements IPaystubRate.ActualSpecialHolidayOTPay

        Public Property RegularHolidayHours As Decimal Implements IPaystubRate.RegularHolidayHours
        Public Property RegularHolidayPay As Decimal Implements IPaystubRate.RegularHolidayPay
        Public Property ActualRegularHolidayPay As Decimal Implements IPaystubRate.ActualRegularHolidayPay

        Public Property RegularHolidayOTHours As Decimal Implements IPaystubRate.RegularHolidayOTHours
        Public Property RegularHolidayOTPay As Decimal Implements IPaystubRate.RegularHolidayOTPay
        Public Property ActualRegularHolidayOTPay As Decimal Implements IPaystubRate.ActualRegularHolidayOTPay

        Public Property HolidayPay As Decimal Implements IPaystubRate.HolidayPay

        Public Property LeaveHours As Decimal Implements IPaystubRate.LeaveHours
        Public Property LeavePay As Decimal Implements IPaystubRate.LeavePay
        Public Property ActualLeavePay As Decimal Implements IPaystubRate.ActualLeavePay

        Public Property LateHours As Decimal Implements IPaystubRate.LateHours
        Public Property LateDeduction As Decimal Implements IPaystubRate.LateDeduction
        Public Property ActualLateDeduction As Decimal Implements IPaystubRate.ActualLateDeduction

        Public Property UndertimeHours As Decimal Implements IPaystubRate.UndertimeHours
        Public Property UndertimeDeduction As Decimal Implements IPaystubRate.UndertimeDeduction
        Public Property ActualUndertimeDeduction As Decimal Implements IPaystubRate.ActualUndertimeDeduction

        Public Property AbsentHours As Decimal Implements IPaystubRate.AbsentHours
        Public Property AbsenceDeduction As Decimal Implements IPaystubRate.AbsenceDeduction
        Public Property ActualAbsenceDeduction As Decimal Implements IPaystubRate.ActualAbsenceDeduction

        'new
        Public Property RestDayNightDiffHours As Decimal

        Public Property RestDayNightDiffPay As Decimal
        Public Property ActualRestDayNightDiffPay As Decimal

        Public Property RestDayNightDiffOTHours As Decimal
        Public Property RestDayNightDiffOTPay As Decimal
        Public Property ActualRestDayNightDiffOTPay As Decimal

        Public Property SpecialHolidayNightDiffHours As Decimal
        Public Property SpecialHolidayNightDiffPay As Decimal
        Public Property ActualSpecialHolidayNightDiffPay As Decimal

        Public Property SpecialHolidayNightDiffOTHours As Decimal
        Public Property SpecialHolidayNightDiffOTPay As Decimal
        Public Property ActualSpecialHolidayNightDiffOTPay As Decimal

        Public Property SpecialHolidayRestDayHours As Decimal
        Public Property SpecialHolidayRestDayPay As Decimal
        Public Property ActualSpecialHolidayRestDayPay As Decimal

        Public Property SpecialHolidayRestDayOTHours As Decimal
        Public Property SpecialHolidayRestDayOTPay As Decimal
        Public Property ActualSpecialHolidayRestDayOTPay As Decimal

        Public Property SpecialHolidayRestDayNightDiffHours As Decimal
        Public Property SpecialHolidayRestDayNightDiffPay As Decimal
        Public Property ActualSpecialHolidayRestDayNightDiffPay As Decimal

        Public Property SpecialHolidayRestDayNightDiffOTHours As Decimal
        Public Property SpecialHolidayRestDayNightDiffOTPay As Decimal
        Public Property ActualSpecialHolidayRestDayNightDiffOTPay As Decimal

        Public Property RegularHolidayNightDiffHours As Decimal
        Public Property RegularHolidayNightDiffPay As Decimal
        Public Property ActualRegularHolidayNightDiffPay As Decimal

        Public Property RegularHolidayNightDiffOTHours As Decimal
        Public Property RegularHolidayNightDiffOTPay As Decimal
        Public Property ActualRegularHolidayNightDiffOTPay As Decimal

        Public Property RegularHolidayRestDayHours As Decimal
        Public Property RegularHolidayRestDayPay As Decimal
        Public Property ActualRegularHolidayRestDayPay As Decimal

        Public Property RegularHolidayRestDayOTHours As Decimal
        Public Property RegularHolidayRestDayOTPay As Decimal
        Public Property ActualRegularHolidayRestDayOTPay As Decimal

        Public Property RegularHolidayRestDayNightDiffHours As Decimal
        Public Property RegularHolidayRestDayNightDiffPay As Decimal
        Public Property ActualRegularHolidayRestDayNightDiffPay As Decimal

        Public Property RegularHolidayRestDayNightDiffOTHours As Decimal
        Public Property RegularHolidayRestDayNightDiffOTPay As Decimal
        Public Property ActualRegularHolidayRestDayNightDiffOTPay As Decimal

        Public Sub Compute(
                    payRate As OvertimeRate,
                    employeeEntitledForNightDifferentialPay As Boolean,
                    employeeEntitledForSpecialHolidayPay As Boolean,
                    employeeEntitledForRegularHolidayPay As Boolean,
                    employeeEntitledForRestDayPay As Boolean,
                    allowanceForOvertimePolicy As Boolean,
                    allowanceForNightDiffPolicy As Boolean,
                    allowanceForNightDiffOTPolicy As Boolean,
                    allowanceForHolidayPolicy As Boolean,
                    allowanceForRestDayPolicy As Boolean,
                    allowanceForRestDayOTPolicy As Boolean,
                    regularHours As Decimal,
                    overtimeHours As Decimal,
                    nightDiffHours As Decimal,
                    nightDiffOvertimeHours As Decimal,
                    restDayHours As Decimal,
                    restDayOTHours As Decimal,
                    specialHolidayHours As Decimal,
                    specialHolidayOTHours As Decimal,
                    regularHolidayHours As Decimal,
                    regularHolidayOTHours As Decimal,
                    leaveHours As Decimal,
                    lateHours As Decimal,
                    undertimeHours As Decimal,
                    absentHours As Decimal,
                    restDayNightDiffHours As Decimal,
                    restDayNightDiffOTHours As Decimal,
                    specialHolidayNightDiffHours As Decimal,
                    specialHolidayNightDiffOTHours As Decimal,
                    specialHolidayRestDayHours As Decimal,
                    specialHolidayRestDayOTHours As Decimal,
                    specialHolidayRestDayNightDiffHours As Decimal,
                    specialHolidayRestDayNightDiffOTHours As Decimal,
                    regularHolidayNightDiffHours As Decimal,
                    regularHolidayNightDiffOTHours As Decimal,
                    regularHolidayRestDayHours As Decimal,
                    regularHolidayRestDayOTHours As Decimal,
                    regularHolidayRestDayNightDiffHours As Decimal,
                    regularHolidayRestDayNightDiffOTHours As Decimal)

            ComputeRegularPays(payRate, allowanceForOvertimePolicy, regularHours, overtimeHours, leaveHours)
            ComputeDeductions(lateHours, undertimeHours, absentHours)

            If employeeEntitledForNightDifferentialPay Then

                ComputeNightDifferentialPays(
                    payRate:=payRate,
                    allowanceForNightDiffPolicy:=allowanceForNightDiffPolicy,
                    nightDiffHours:=nightDiffHours,
                    restDayNightDiffHours:=restDayNightDiffHours,
                    specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
                    specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
                    regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
                    regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours)

                ComputeNightDifferentialOvertimePays(
                    payRate:=payRate,
                    allowanceForNightDiffOTPolicy:=allowanceForNightDiffOTPolicy,
                    nightDiffOvertimeHours:=nightDiffOvertimeHours,
                    restDayNightDiffOTHours:=restDayNightDiffOTHours,
                    specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
                    specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
                    regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
                    regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours)

            End If

            ComputeHolidayPays(
                payRate:=payRate,
                employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
                employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
                allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
                allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
                allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
                allowanceForOvertimePolicy:=allowanceForOvertimePolicy,
                specialHolidayHours:=specialHolidayHours,
                specialHolidayOTHours:=specialHolidayOTHours,
                regularHolidayHours:=regularHolidayHours,
                regularHolidayOTHours:=regularHolidayOTHours,
                specialHolidayRestDayHours:=specialHolidayRestDayHours,
                specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
                regularHolidayRestDayHours:=regularHolidayRestDayHours,
                regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours)

            If employeeEntitledForRestDayPay Then

                ComputeRestDayPays(
                    payRate:=payRate,
                    allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
                    allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
                    restDayHours:=restDayHours,
                    restDayOTHours:=restDayOTHours)

            End If

        End Sub

        Private Function ComputeFinalRoundedRate(value As Decimal, Optional isActual As Boolean = False, Optional decimalPlace As Integer = 2) As Decimal

            Dim finalRate = If(isActual, Me.ActualHourlyRate, Me.HourlyRate)

            Return AccuMath.CommercialRound(value * finalRate, decimalPlace)

        End Function

        Private Sub ComputeRegularPays(payRate As OvertimeRate, allowanceForOvertimePolicy As Boolean, regularHours As Decimal, overtimeHours As Decimal, leaveHours As Decimal)
            Me.RegularHours = regularHours
            Me.RegularPay = ComputeFinalRoundedRate(Me.RegularHours, decimalPlace:=4)
            Me.ActualRegularPay = ComputeFinalRoundedRate(Me.RegularHours, isActual:=True, decimalPlace:=4)

            Me.OvertimeHours = overtimeHours
            Me.OvertimePay = ComputeFinalRoundedRate(Me.OvertimeHours * payRate.Overtime.Rate)
            If allowanceForOvertimePolicy Then
                Me.ActualOvertimePay = ComputeFinalRoundedRate(Me.OvertimeHours * payRate.Overtime.Rate, isActual:=True)
            Else
                Me.ActualOvertimePay = Me.OvertimePay

            End If

            Me.LeaveHours = leaveHours
            Me.LeavePay = ComputeFinalRoundedRate(Me.LeaveHours)
            Me.ActualLeavePay = ComputeFinalRoundedRate(Me.LeaveHours, isActual:=True)
        End Sub

        Private Sub ComputeDeductions(lateHours As Decimal, undertimeHours As Decimal, absentHours As Decimal)
            'Deductions
            Me.LateHours = lateHours
            Me.LateDeduction = ComputeFinalRoundedRate(Me.LateHours, decimalPlace:=4)
            Me.ActualLateDeduction = ComputeFinalRoundedRate(Me.LateHours, decimalPlace:=4, isActual:=True)

            Me.UndertimeHours = undertimeHours
            Me.UndertimeDeduction = ComputeFinalRoundedRate(Me.UndertimeHours, decimalPlace:=4)
            Me.ActualUndertimeDeduction = ComputeFinalRoundedRate(Me.UndertimeHours, decimalPlace:=4, isActual:=True)

            Me.AbsentHours = absentHours
            Me.AbsenceDeduction = ComputeFinalRoundedRate(Me.AbsentHours, decimalPlace:=4)
            Me.ActualAbsenceDeduction = ComputeFinalRoundedRate(Me.AbsentHours, decimalPlace:=4, isActual:=True)
        End Sub

        Private Function GetNightDifferentialRate(basicRate As Decimal, nightDifferentialRate As Decimal) As Decimal

            Return nightDifferentialRate - basicRate

        End Function

        Private Sub ComputeNightDifferentialPays(payRate As OvertimeRate, allowanceForNightDiffPolicy As Boolean, nightDiffHours As Decimal, restDayNightDiffHours As Decimal, specialHolidayNightDiffHours As Decimal, specialHolidayRestDayNightDiffHours As Decimal, regularHolidayNightDiffHours As Decimal, regularHolidayRestDayNightDiffHours As Decimal)
            Me.NightDiffHours = nightDiffHours
            Me.NightDiffPay = ComputeFinalRoundedRate(Me.NightDiffHours * GetNightDifferentialRate(payRate.BasePay.Rate, payRate.NightDifferential.Rate))
            Me.ActualNightDiffPay = Me.NightDiffPay

            Me.RestDayNightDiffHours = restDayNightDiffHours
            Me.RestDayNightDiffPay = ComputeFinalRoundedRate(Me.RestDayNightDiffHours * GetNightDifferentialRate(payRate.RestDay.Rate, payRate.RestDayNightDifferential.Rate))
            Me.ActualRestDayNightDiffPay = Me.RestDayNightDiffPay

            Me.SpecialHolidayNightDiffHours = specialHolidayNightDiffHours
            Me.SpecialHolidayNightDiffPay = ComputeFinalRoundedRate(Me.SpecialHolidayNightDiffHours * GetNightDifferentialRate(payRate.SpecialHoliday.Rate, payRate.SpecialHolidayNightDifferential.Rate))
            Me.ActualSpecialHolidayNightDiffPay = Me.SpecialHolidayNightDiffPay

            Me.SpecialHolidayRestDayNightDiffHours = specialHolidayRestDayNightDiffHours
            Me.SpecialHolidayRestDayNightDiffPay = ComputeFinalRoundedRate(Me.SpecialHolidayRestDayNightDiffHours * GetNightDifferentialRate(payRate.SpecialHolidayRestDay.Rate, payRate.SpecialHolidayRestDayNightDifferential.Rate))
            Me.ActualSpecialHolidayRestDayNightDiffPay = Me.SpecialHolidayRestDayNightDiffPay

            Me.RegularHolidayNightDiffHours = regularHolidayNightDiffHours
            Me.RegularHolidayNightDiffPay = ComputeFinalRoundedRate(Me.RegularHolidayNightDiffHours * GetNightDifferentialRate(payRate.RegularHoliday.Rate, payRate.RegularHolidayNightDifferential.Rate))
            Me.ActualRegularHolidayNightDiffPay = Me.RegularHolidayNightDiffPay

            Me.RegularHolidayRestDayNightDiffHours = regularHolidayRestDayNightDiffHours
            Me.RegularHolidayRestDayNightDiffPay = ComputeFinalRoundedRate(Me.RegularHolidayRestDayNightDiffHours * GetNightDifferentialRate(payRate.RegularHolidayRestDay.Rate, payRate.RegularHolidayRestDayNightDifferential.Rate))
            Me.ActualRegularHolidayRestDayNightDiffPay = Me.RegularHolidayRestDayNightDiffPay

            If allowanceForNightDiffPolicy Then
                Me.ActualNightDiffPay = ComputeFinalRoundedRate(Me.NightDiffHours * GetNightDifferentialRate(payRate.BasePay.Rate, payRate.NightDifferential.Rate), isActual:=True)
                Me.ActualRestDayNightDiffPay = ComputeFinalRoundedRate(Me.RestDayNightDiffHours * GetNightDifferentialRate(payRate.RestDay.Rate, payRate.RestDayNightDifferential.Rate), isActual:=True)
                Me.ActualSpecialHolidayNightDiffPay = ComputeFinalRoundedRate(Me.SpecialHolidayNightDiffHours * GetNightDifferentialRate(payRate.SpecialHoliday.Rate, payRate.SpecialHolidayNightDifferential.Rate), isActual:=True)
                Me.ActualSpecialHolidayRestDayNightDiffPay = ComputeFinalRoundedRate(Me.SpecialHolidayRestDayNightDiffHours * GetNightDifferentialRate(payRate.SpecialHolidayRestDay.Rate, payRate.SpecialHolidayRestDayNightDifferential.Rate), isActual:=True)
                Me.ActualRegularHolidayNightDiffPay = ComputeFinalRoundedRate(Me.RegularHolidayNightDiffHours * GetNightDifferentialRate(payRate.RegularHoliday.Rate, payRate.RegularHolidayNightDifferential.Rate), isActual:=True)
                Me.ActualRegularHolidayRestDayNightDiffPay = ComputeFinalRoundedRate(Me.RegularHolidayRestDayNightDiffHours * GetNightDifferentialRate(payRate.RegularHolidayRestDay.Rate, payRate.RegularHolidayRestDayNightDifferential.Rate), isActual:=True)
            End If
        End Sub

        Private Sub ComputeNightDifferentialOvertimePays(payRate As OvertimeRate, allowanceForNightDiffOTPolicy As Boolean, nightDiffOvertimeHours As Decimal, restDayNightDiffOTHours As Decimal, specialHolidayNightDiffOTHours As Decimal, specialHolidayRestDayNightDiffOTHours As Decimal, regularHolidayNightDiffOTHours As Decimal, regularHolidayRestDayNightDiffOTHours As Decimal)
            Me.NightDiffOvertimeHours = nightDiffOvertimeHours
            Me.NightDiffOvertimePay = ComputeFinalRoundedRate(Me.NightDiffOvertimeHours * payRate.NightDifferentialOvertime.Rate)
            Me.ActualNightDiffOvertimePay = Me.NightDiffOvertimePay

            Me.RestDayNightDiffOTHours = restDayNightDiffOTHours
            Me.RestDayNightDiffOTPay = ComputeFinalRoundedRate(Me.RestDayNightDiffOTHours * payRate.RestDayNightDifferentialOvertime.Rate)
            Me.ActualRestDayNightDiffOTPay = Me.RestDayNightDiffOTPay

            Me.SpecialHolidayNightDiffOTHours = specialHolidayNightDiffOTHours
            Me.SpecialHolidayNightDiffOTPay = ComputeFinalRoundedRate(Me.SpecialHolidayNightDiffOTHours * payRate.SpecialHolidayNightDifferentialOvertime.Rate)
            Me.ActualSpecialHolidayNightDiffOTPay = Me.SpecialHolidayNightDiffOTPay

            Me.SpecialHolidayRestDayNightDiffOTHours = specialHolidayRestDayNightDiffOTHours
            Me.SpecialHolidayRestDayNightDiffOTPay = ComputeFinalRoundedRate(Me.SpecialHolidayRestDayNightDiffOTHours * payRate.SpecialHolidayRestDayNightDifferentialOvertime.Rate)
            Me.ActualSpecialHolidayRestDayNightDiffOTPay = Me.SpecialHolidayRestDayNightDiffOTPay

            Me.RegularHolidayNightDiffOTHours = regularHolidayNightDiffOTHours
            Me.RegularHolidayNightDiffOTPay = ComputeFinalRoundedRate(Me.RegularHolidayNightDiffOTHours * payRate.RegularHolidayNightDifferentialOvertime.Rate)
            Me.ActualRegularHolidayNightDiffOTPay = Me.RegularHolidayNightDiffOTPay

            Me.RegularHolidayRestDayNightDiffOTHours = regularHolidayRestDayNightDiffOTHours
            Me.RegularHolidayRestDayNightDiffOTPay = ComputeFinalRoundedRate(Me.RegularHolidayRestDayNightDiffOTHours * payRate.RegularHolidayRestDayNightDifferentialOvertime.Rate)
            Me.ActualRegularHolidayRestDayNightDiffOTPay = Me.RegularHolidayRestDayNightDiffOTPay

            If allowanceForNightDiffOTPolicy Then
                Me.ActualNightDiffOvertimePay = ComputeFinalRoundedRate(Me.NightDiffOvertimeHours * payRate.NightDifferentialOvertime.Rate, isActual:=True)
                Me.ActualRestDayNightDiffOTPay = ComputeFinalRoundedRate(Me.RestDayNightDiffOTHours * payRate.RestDayNightDifferentialOvertime.Rate, isActual:=True)
                Me.ActualSpecialHolidayNightDiffOTPay = ComputeFinalRoundedRate(Me.SpecialHolidayNightDiffOTHours * payRate.SpecialHolidayNightDifferentialOvertime.Rate, isActual:=True)
                Me.ActualSpecialHolidayRestDayNightDiffOTPay = ComputeFinalRoundedRate(Me.SpecialHolidayRestDayNightDiffOTHours * payRate.SpecialHolidayRestDayNightDifferentialOvertime.Rate, isActual:=True)
                Me.ActualRegularHolidayNightDiffOTPay = ComputeFinalRoundedRate(Me.RegularHolidayNightDiffOTHours * payRate.RegularHolidayNightDifferentialOvertime.Rate, isActual:=True)
                Me.ActualRegularHolidayRestDayNightDiffOTPay = ComputeFinalRoundedRate(Me.RegularHolidayRestDayNightDiffOTHours * payRate.RegularHolidayRestDayNightDifferentialOvertime.Rate, isActual:=True)
            End If
        End Sub

        Private Function GetRateWithCondition(normalRate As Decimal, premiumRate As Decimal, condition As Boolean) As Decimal

            Return If(condition, premiumRate, normalRate)

        End Function

        Private Sub ComputeHolidayPays(
                        payRate As OvertimeRate,
                        employeeEntitledForSpecialHolidayPay As Boolean,
                        employeeEntitledForRegularHolidayPay As Boolean,
                        allowanceForHolidayPolicy As Boolean,
                        allowanceForRestDayPolicy As Boolean,
                        allowanceForRestDayOTPolicy As Boolean,
                        allowanceForOvertimePolicy As Boolean,
                        specialHolidayHours As Decimal,
                        specialHolidayOTHours As Decimal,
                        regularHolidayHours As Decimal,
                        regularHolidayOTHours As Decimal,
                        specialHolidayRestDayHours As Decimal,
                        specialHolidayRestDayOTHours As Decimal,
                        regularHolidayRestDayHours As Decimal,
                        regularHolidayRestDayOTHours As Decimal)

            Me.SpecialHolidayHours = specialHolidayHours
            Me.SpecialHolidayPay = ComputeSpecialHolidayPay(payRate, employeeEntitledForSpecialHolidayPay)
            Me.ActualSpecialHolidayPay = Me.SpecialHolidayPay

            Me.SpecialHolidayOTHours = specialHolidayOTHours
            Me.SpecialHolidayOTPay = ComputeSpecialOTPay(payRate, employeeEntitledForSpecialHolidayPay)
            Me.ActualSpecialHolidayOTPay = Me.SpecialHolidayOTPay

            Me.SpecialHolidayRestDayHours = specialHolidayRestDayHours
            Me.SpecialHolidayRestDayPay = ComputeSpecialHolidayRestDayPay(payRate, employeeEntitledForSpecialHolidayPay)
            Me.ActualSpecialHolidayRestDayPay = Me.SpecialHolidayRestDayPay

            Me.SpecialHolidayRestDayOTHours = specialHolidayRestDayOTHours
            Me.SpecialHolidayRestDayOTPay = ComputeSpecialHolidayRestDayOTPay(payRate, employeeEntitledForSpecialHolidayPay)
            Me.ActualSpecialHolidayRestDayOTPay = Me.SpecialHolidayRestDayOTPay

            Me.RegularHolidayHours = regularHolidayHours
            Me.RegularHolidayPay = ComputeRegularHolidayPay(payRate, employeeEntitledForRegularHolidayPay)
            Me.ActualRegularHolidayPay = Me.RegularHolidayPay

            Me.RegularHolidayOTHours = regularHolidayOTHours
            Me.RegularHolidayOTPay = ComputeRegularHolidayOTPay(payRate, employeeEntitledForRegularHolidayPay)
            Me.ActualRegularHolidayOTPay = Me.RegularHolidayOTPay

            Me.RegularHolidayRestDayHours = regularHolidayRestDayHours
            Me.RegularHolidayRestDayPay = ComputeRegularHolidayRestDayPay(payRate, employeeEntitledForRegularHolidayPay)
            Me.ActualRegularHolidayRestDayPay = Me.RegularHolidayRestDayPay

            Me.RegularHolidayRestDayOTHours = regularHolidayRestDayOTHours
            Me.RegularHolidayRestDayOTPay = GetRegularHolidayRestDayOTPay(payRate, employeeEntitledForRegularHolidayPay)
            Me.ActualRegularHolidayRestDayOTPay = Me.RegularHolidayRestDayOTPay

            ComputeActualPayForHolidays(
                payRate:=payRate,
                employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
                employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
                allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
                allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
                allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
                allowanceForOvertimePolicy:=allowanceForOvertimePolicy)
        End Sub

        Private Function ComputeSpecialHolidayPay(
                            payRate As OvertimeRate,
                            employeeEntitledForSpecialHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.SpecialHolidayHours *
                        GetRateWithCondition(
                                payRate.BasePay.Rate,
                                payRate.SpecialHoliday.Rate,
                                employeeEntitledForSpecialHolidayPay)),
                   isActual)
        End Function

        Private Function ComputeSpecialOTPay(
                            payRate As OvertimeRate,
                            employeeEntitledForSpecialHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.SpecialHolidayOTHours *
                        GetRateWithCondition(
                                payRate.Overtime.Rate,
                                payRate.SpecialHolidayOvertime.Rate,
                                employeeEntitledForSpecialHolidayPay)),
                   isActual)
        End Function

        Private Function ComputeSpecialHolidayRestDayPay(
                            payRate As OvertimeRate,
                            employeeEntitledForSpecialHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.SpecialHolidayRestDayHours *
                        GetRateWithCondition(
                                payRate.RestDay.Rate,
                                payRate.SpecialHolidayRestDay.Rate,
                                employeeEntitledForSpecialHolidayPay)),
                   isActual)
        End Function

        Private Function ComputeSpecialHolidayRestDayOTPay(
                            payRate As OvertimeRate,
                            employeeEntitledForSpecialHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.SpecialHolidayRestDayOTHours *
                        GetRateWithCondition(
                                payRate.RestDayOvertime.Rate,
                                payRate.SpecialHolidayRestDayOvertime.Rate,
                                employeeEntitledForSpecialHolidayPay)),
                   isActual)
        End Function

        Private Function ComputeRegularHolidayPay(
                            payRate As OvertimeRate,
                            employeeEntitledForRegularHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.RegularHolidayHours *
                        GetRateWithCondition(
                                payRate.BasePay.Rate,
                                payRate.RegularHoliday.Rate,
                                employeeEntitledForRegularHolidayPay)),
                   isActual)
        End Function

        Private Function ComputeRegularHolidayOTPay(
                            payRate As OvertimeRate,
                            employeeEntitledForRegularHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.RegularHolidayOTHours *
                        GetRateWithCondition(
                                payRate.Overtime.Rate,
                                payRate.RegularHolidayOvertime.Rate,
                                employeeEntitledForRegularHolidayPay)),
                   isActual)
        End Function

        Private Function ComputeRegularHolidayRestDayPay(
                            payRate As OvertimeRate,
                            employeeEntitledForRegularHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.RegularHolidayRestDayHours *
                        GetRateWithCondition(
                                payRate.RestDay.Rate,
                                payRate.RegularHolidayRestDay.Rate,
                                employeeEntitledForRegularHolidayPay)),
                   isActual)
        End Function

        Private Function GetRegularHolidayRestDayOTPay(
                            payRate As OvertimeRate,
                            employeeEntitledForRegularHolidayPay As Boolean,
                            Optional isActual As Boolean = False) As Decimal

            Return ComputeFinalRoundedRate((
                    Me.RegularHolidayRestDayOTHours *
                        GetRateWithCondition(
                                payRate.RestDayOvertime.Rate,
                                payRate.RegularHolidayRestDayOvertime.Rate,
                                employeeEntitledForRegularHolidayPay)),
                   isActual)

        End Function

        Private Sub ComputeActualPayForHolidays(payRate As OvertimeRate, employeeEntitledForSpecialHolidayPay As Boolean, employeeEntitledForRegularHolidayPay As Boolean, allowanceForHolidayPolicy As Boolean, allowanceForRestDayPolicy As Boolean, allowanceForRestDayOTPolicy As Boolean, allowanceForOvertimePolicy As Boolean)
            'if employeeEntitledForSpecialHolidayPay = false, treat is as normal day
            If employeeEntitledForSpecialHolidayPay = False Then

                If allowanceForRestDayPolicy Then
                    Me.ActualSpecialHolidayRestDayPay = ComputeSpecialHolidayRestDayPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                End If

                If allowanceForRestDayOTPolicy Then
                    Me.ActualSpecialHolidayRestDayOTPay = ComputeSpecialHolidayRestDayOTPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                End If

                If allowanceForOvertimePolicy Then
                    Me.ActualSpecialHolidayOTPay = ComputeSpecialOTPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                End If

                Me.ActualSpecialHolidayPay = ComputeSpecialHolidayPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
            Else
                If allowanceForHolidayPolicy Then
                    Me.ActualSpecialHolidayRestDayPay = ComputeSpecialHolidayRestDayPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                    Me.ActualSpecialHolidayRestDayOTPay = ComputeSpecialHolidayRestDayOTPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                    Me.ActualSpecialHolidayOTPay = ComputeSpecialOTPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                    Me.ActualSpecialHolidayPay = ComputeSpecialHolidayPay(payRate, employeeEntitledForSpecialHolidayPay, isActual:=True)
                End If
            End If

            'if employeeEntitledForRegularHolidayPay = false, treat is as normal day
            If employeeEntitledForRegularHolidayPay = False Then

                If allowanceForRestDayPolicy Then
                    Me.ActualRegularHolidayRestDayPay = ComputeRegularHolidayRestDayPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                End If

                If allowanceForRestDayOTPolicy Then
                    Me.ActualRegularHolidayRestDayOTPay = GetRegularHolidayRestDayOTPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                End If

                If allowanceForOvertimePolicy Then
                    Me.ActualRegularHolidayOTPay = ComputeRegularHolidayOTPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                End If

                Me.ActualRegularHolidayPay = ComputeRegularHolidayPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
            Else
                If allowanceForHolidayPolicy Then
                    Me.ActualRegularHolidayRestDayPay = ComputeRegularHolidayRestDayPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                    Me.ActualRegularHolidayRestDayOTPay = GetRegularHolidayRestDayOTPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                    Me.ActualRegularHolidayOTPay = ComputeRegularHolidayOTPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                    Me.ActualRegularHolidayPay = ComputeRegularHolidayPay(payRate, employeeEntitledForRegularHolidayPay, isActual:=True)
                End If
            End If
        End Sub

        Private Sub ComputeRestDayPays(payRate As OvertimeRate, allowanceForRestDayPolicy As Boolean, allowanceForRestDayOTPolicy As Boolean, restDayHours As Decimal, restDayOTHours As Decimal)
            Me.RestDayHours = restDayHours
            Me.RestDayPay = ComputeFinalRoundedRate(Me.RestDayHours * payRate.RestDay.Rate)

            Me.RestDayOTHours = restDayOTHours
            Me.RestDayOTPay = ComputeFinalRoundedRate(Me.RestDayOTHours * payRate.RestDayOvertime.Rate)

            If allowanceForRestDayPolicy Then
                Me.ActualRestDayPay = ComputeFinalRoundedRate(Me.RestDayHours * payRate.RestDay.Rate, isActual:=True)
            Else
                Me.ActualRestDayPay = Me.RestDayPay
            End If

            If allowanceForRestDayOTPolicy Then
                Me.ActualRestDayOTPay = ComputeFinalRoundedRate(Me.RestDayOTHours * payRate.RestDayOvertime.Rate, isActual:=True)
            Else
                Me.ActualRestDayOTPay = Me.RestDayOTPay
            End If
        End Sub

    End Class

End Namespace