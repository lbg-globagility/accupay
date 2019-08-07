Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.SimplifiedEntities

Namespace Benchmark

    Public Class BenchmarkPayrollGeneration

        Private ReadOnly _currentPayPeriod As IPayPeriod

        Private ReadOnly _employee As New Employee

        Private ReadOnly _regularDays As Decimal

        Private ReadOnly _lateDays As Decimal

        Private ReadOnly _leaveDays As Decimal

        Private ReadOnly _overtimeRate As OvertimeRate

        Private ReadOnly _ecola As Allowance

        Private ReadOnly _actualSalaryPolicy As ActualTimeEntryPolicy

        Private ReadOnly _selectedDeductions As List(Of AdjustmentInput)

        Private ReadOnly _selectedIncomes As List(Of AdjustmentInput)

        Private ReadOnly _overtimes As List(Of OvertimeInput)

        Private ReadOnly _payrollResources As PayrollResources

        Private ReadOnly _employeeRate As BenchmarkPaystubRate

        Private Sub New(
                employee As Employee,
                payrollResources As PayrollResources,
                currentPayPeriod As IPayPeriod,
                employeeRate As BenchmarkPaystubRate,
                regularDays As Decimal,
                lateDays As Decimal,
                leaveDays As Decimal,
                overtimeRate As OvertimeRate,
                actualSalaryPolicy As ActualTimeEntryPolicy,
                selectedDeductions As List(Of AdjustmentInput),
                selectedIncomes As List(Of AdjustmentInput),
                overtimes As List(Of OvertimeInput),
                ecola As Allowance)

            _employee = employee
            _payrollResources = payrollResources
            _currentPayPeriod = currentPayPeriod
            _employeeRate = employeeRate
            _regularDays = regularDays
            _lateDays = lateDays
            _leaveDays = leaveDays
            _overtimeRate = overtimeRate
            _actualSalaryPolicy = actualSalaryPolicy
            _selectedDeductions = selectedDeductions
            _selectedIncomes = selectedIncomes
            _overtimes = overtimes
            _ecola = ecola
        End Sub

        Public Class DoProcessOutput

            Public ReadOnly Property Paystub As Paystub
            Public ReadOnly Property LoanTransanctions As List(Of LoanTransaction)

            Sub New(paystub As Paystub, loanTransanctions As List(Of LoanTransaction))

                Me.Paystub = paystub
                Me.LoanTransanctions = loanTransanctions

            End Sub

        End Class

        Public Shared Function DoProcess(
                                    employee As Employee,
                                    payrollResources As PayrollResources,
                                    currentPayPeriod As IPayPeriod,
                                    employeeRate As BenchmarkPaystubRate,
                                    regularDays As Decimal,
                                    lateDays As Decimal,
                                    leaveDays As Decimal,
                                    overtimeRate As OvertimeRate,
                                    actualSalaryPolicy As ActualTimeEntryPolicy,
                                    selectedDeductions As List(Of AdjustmentInput),
                                    selectedIncomes As List(Of AdjustmentInput),
                                    overtimes As List(Of OvertimeInput),
                                    ecola As Allowance) As DoProcessOutput

            Dim generator As New BenchmarkPayrollGeneration(
                                    employee,
                                    payrollResources,
                                    currentPayPeriod,
                                    employeeRate,
                                    regularDays:=regularDays,
                                    lateDays:=lateDays,
                                    leaveDays:=leaveDays,
                                    overtimeRate:=overtimeRate,
                                    actualSalaryPolicy:=actualSalaryPolicy,
                                    selectedDeductions:=selectedDeductions,
                                    selectedIncomes:=selectedIncomes,
                                    overtimes:=overtimes,
                                    ecola:=ecola)

            Dim payrollGeneration = New PayrollGeneration(
                                generator._employee,
                                generator._payrollResources
                            )

            Dim output As DoProcessOutput = generator.CreatePaystub(employee, payrollGeneration)

            Return output

        End Function

        Private Function CreatePaystub(employee As Employee, generator As PayrollGeneration) As DoProcessOutput
            Dim paystub = New Paystub() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .LastUpdBy = z_User,
                    .EmployeeID = employee.RowID,
                    .PayPeriodID = _currentPayPeriod.RowID,
                    .PayFromdate = _currentPayPeriod.PayFromDate,
                    .PayToDate = _currentPayPeriod.PayToDate
                }

            paystub.Actual = New PaystubActual With {
            .OrganizationID = z_OrganizationID,
            .EmployeeID = employee.RowID,
            .PayPeriodID = _currentPayPeriod.RowID,
            .PayFromDate = _currentPayPeriod.PayFromDate,
            .PayToDate = _currentPayPeriod.PayToDate
        }

            paystub.EmployeeID = employee.RowID

            ComputeBasicHoursAndBasicPay(paystub, employee)
            ComputeHoursAndPay(paystub)

            'TODO
            'ComputeEcola()

            CreateAdjustments(paystub)

            ComputeTotalEarnings(paystub, employee)

            Dim loans = generator.ComputePayroll(paystub)

            Return New DoProcessOutput(paystub, loans)
        End Function

        Private Sub ComputeBasicHoursAndBasicPay(paystub As Paystub, employee As Employee)

            Dim cutOffsPerMonth As Integer = 2

            Dim workDaysThisCutOff = PayrollTools.
                GetWorkDaysPerMonth(employee.WorkDaysPerYear) / cutOffsPerMonth

            paystub.BasicHours = workDaysThisCutOff * PayrollTools.WorkHoursPerDay

            paystub.BasicPay = paystub.BasicHours * _employeeRate.HourlyRate

        End Sub

        Private Sub ComputeHoursAndPay(paystub As Paystub)

            Dim regularHours As Decimal = ConvertDaysToHours(_regularDays)
            Dim overtimeHours As Decimal = GetOvertime(OvertimeRate.OvertimeDescription)

            Dim nightDiffHours As Decimal = GetOvertime(OvertimeRate.NightDifferentialDescription)
            Dim nightDiffOvertimeHours As Decimal = GetOvertime(OvertimeRate.NightDifferentialOvertimeDescription)
            Dim restDayHours As Decimal = GetOvertime(OvertimeRate.RestDayDescription)
            Dim restDayOTHours As Decimal = GetOvertime(OvertimeRate.RestDayOvertimeDescription)
            Dim specialHolidayHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayDescription)
            Dim specialHolidayOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayOvertimeDescription)
            Dim regularHolidayHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayDescription)
            Dim regularHolidayOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayOvertimeDescription)

            Dim leaveHours As Decimal = ConvertDaysToHours(_leaveDays)
            Dim lateHours As Decimal = ConvertDaysToHours(_lateDays)
            Dim undertimeHours As Decimal = 0
            Dim absentHours As Decimal = 0

            Dim restDayNightDiffHours As Decimal = GetOvertime(OvertimeRate.RestDayNightDifferentialDescription)
            Dim restDayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.RestDayNightDifferentialOvertimeDescription)
            Dim specialHolidayNightDiffHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayNightDifferentialDescription)
            Dim specialHolidayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayNightDifferentialOvertimeDescription)
            Dim specialHolidayRestDayHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayDescription)
            Dim specialHolidayRestDayOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayOvertimeDescription)
            Dim specialHolidayRestDayNightDiffHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayNightDifferentialDescription)
            Dim specialHolidayRestDayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.SpecialHolidayRestDayNightDifferentialOvertimeDescription)
            Dim regularHolidayNightDiffHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayNightDifferentialDescription)
            Dim regularHolidayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayNightDifferentialOvertimeDescription)
            Dim regularHolidayRestDayHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayDescription)
            Dim regularHolidayRestDayOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayOvertimeDescription)
            Dim regularHolidayRestDayNightDiffHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayNightDifferentialDescription)
            Dim regularHolidayRestDayNightDiffOTHours As Decimal = GetOvertime(OvertimeRate.RegularHolidayRestDayNightDifferentialOvertimeDescription)

            _employeeRate.Compute(
            _overtimeRate,
            employeeEntitledForNightDifferentialPay:=_employeeRate.Employee.CalcNightDiff,
            employeeEntitledForSpecialHolidayPay:=_employeeRate.Employee.CalcSpecialHoliday,
            employeeEntitledForRegularHolidayPay:=_employeeRate.Employee.CalcHoliday,
            employeeEntitledForRestDayPay:=_employeeRate.Employee.CalcRestDay,
            allowanceForOvertimePolicy:=_actualSalaryPolicy.AllowanceForOvertime,
            allowanceForNightDiffPolicy:=_actualSalaryPolicy.AllowanceForNightDiff,
            allowanceForNightDiffOTPolicy:=_actualSalaryPolicy.AllowanceForNightDiffOT,
            allowanceForHolidayPolicy:=_actualSalaryPolicy.AllowanceForHoliday,
            allowanceForRestDayPolicy:=_actualSalaryPolicy.AllowanceForRestDay,
            allowanceForRestDayOTPolicy:=_actualSalaryPolicy.AllowanceForRestDayOT,
            regularHours:=regularHours,
            overtimeHours:=overtimeHours,
            nightDiffHours:=nightDiffHours,
            nightDiffOvertimeHours:=nightDiffOvertimeHours,
            restDayHours:=restDayHours,
            restDayOTHours:=restDayOTHours,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            leaveHours:=leaveHours,
            lateHours:=lateHours,
            undertimeHours:=undertimeHours,
            absentHours:=absentHours,
            restDayNightDiffHours:=restDayNightDiffHours,
            restDayNightDiffOTHours:=restDayNightDiffOTHours,
            specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
            specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
            specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
            regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
            regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours,
            regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
            regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours
        )

            paystub.RegularHours = _employeeRate.RegularHours
            paystub.RegularPay = _employeeRate.RegularPay
            paystub.Actual.RegularPay = _employeeRate.ActualRegularPay

            paystub.OvertimeHours = _employeeRate.OvertimeHours
            paystub.OvertimePay = _employeeRate.OvertimePay
            paystub.Actual.OvertimePay = _employeeRate.ActualOvertimePay

            paystub.NightDiffHours = _employeeRate.NightDiffHours
            paystub.NightDiffPay = _employeeRate.NightDiffPay
            paystub.Actual.NightDiffPay = _employeeRate.ActualNightDiffPay

            paystub.NightDiffOvertimeHours = _employeeRate.NightDiffOvertimeHours
            paystub.NightDiffOvertimePay = _employeeRate.NightDiffOvertimePay
            paystub.Actual.NightDiffOvertimePay = _employeeRate.ActualNightDiffOvertimePay

            paystub.RestDayHours = _employeeRate.RestDayHours
            paystub.RestDayPay = _employeeRate.RestDayPay
            paystub.Actual.RestDayPay = _employeeRate.ActualRestDayPay

            paystub.RestDayOTHours = _employeeRate.RestDayOTHours
            paystub.RestDayOTPay = _employeeRate.RestDayOTPay
            paystub.Actual.RestDayOTPay = _employeeRate.ActualRestDayOTPay

            paystub.SpecialHolidayHours = _employeeRate.SpecialHolidayHours
            paystub.SpecialHolidayPay = _employeeRate.SpecialHolidayPay
            paystub.Actual.SpecialHolidayPay = _employeeRate.ActualSpecialHolidayPay

            paystub.SpecialHolidayOTHours = _employeeRate.SpecialHolidayOTHours
            paystub.SpecialHolidayOTPay = _employeeRate.SpecialHolidayOTPay
            paystub.Actual.SpecialHolidayOTPay = _employeeRate.ActualSpecialHolidayOTPay

            paystub.RegularHolidayHours = _employeeRate.RegularHolidayHours
            paystub.RegularHolidayPay = _employeeRate.RegularHolidayPay
            paystub.Actual.RegularHolidayPay = _employeeRate.ActualRegularHolidayPay

            paystub.RegularHolidayOTHours = _employeeRate.RegularHolidayOTHours
            paystub.RegularHolidayOTPay = _employeeRate.RegularHolidayOTPay
            paystub.Actual.RegularHolidayOTPay = _employeeRate.ActualRegularHolidayOTPay

            paystub.LeaveHours = _employeeRate.LeaveHours
            paystub.LeavePay = _employeeRate.LeavePay
            paystub.Actual.LeavePay = _employeeRate.ActualLeavePay

            paystub.LateHours = _employeeRate.LateHours
            paystub.LateDeduction = _employeeRate.LateDeduction
            paystub.Actual.LateDeduction = _employeeRate.ActualLateDeduction

            paystub.UndertimeHours = _employeeRate.UndertimeHours
            paystub.UndertimeDeduction = _employeeRate.UndertimeDeduction
            paystub.Actual.UndertimeDeduction = _employeeRate.ActualUndertimeDeduction

            paystub.AbsentHours = _employeeRate.AbsentHours
            paystub.AbsenceDeduction = _employeeRate.AbsenceDeduction
            paystub.Actual.AbsenceDeduction = _employeeRate.ActualAbsenceDeduction

            'new
            paystub.RestDayNightDiffHours = _employeeRate.RestDayNightDiffHours
            paystub.RestDayNightDiffPay = _employeeRate.RestDayNightDiffPay
            paystub.Actual.RestDayNightDiffPay = _employeeRate.ActualRestDayNightDiffPay

            paystub.RestDayNightDiffOTHours = _employeeRate.RestDayNightDiffOTHours
            paystub.RestDayNightDiffOTPay = _employeeRate.RestDayNightDiffOTPay
            paystub.Actual.RestDayNightDiffOTPay = _employeeRate.ActualRestDayNightDiffOTPay

            paystub.SpecialHolidayNightDiffHours = _employeeRate.SpecialHolidayNightDiffHours
            paystub.SpecialHolidayNightDiffPay = _employeeRate.SpecialHolidayNightDiffPay
            paystub.Actual.SpecialHolidayNightDiffPay = _employeeRate.ActualSpecialHolidayNightDiffPay

            paystub.SpecialHolidayNightDiffOTHours = _employeeRate.SpecialHolidayNightDiffOTHours
            paystub.SpecialHolidayNightDiffOTPay = _employeeRate.SpecialHolidayNightDiffOTPay
            paystub.Actual.SpecialHolidayNightDiffOTPay = _employeeRate.ActualSpecialHolidayNightDiffOTPay

            paystub.SpecialHolidayRestDayHours = _employeeRate.SpecialHolidayRestDayHours
            paystub.SpecialHolidayRestDayPay = _employeeRate.SpecialHolidayRestDayPay
            paystub.Actual.SpecialHolidayRestDayPay = _employeeRate.ActualSpecialHolidayRestDayPay

            paystub.SpecialHolidayRestDayOTHours = _employeeRate.SpecialHolidayRestDayOTHours
            paystub.SpecialHolidayRestDayOTPay = _employeeRate.SpecialHolidayRestDayOTPay
            paystub.Actual.SpecialHolidayRestDayOTPay = _employeeRate.ActualSpecialHolidayRestDayOTPay

            paystub.SpecialHolidayRestDayNightDiffHours = _employeeRate.SpecialHolidayRestDayNightDiffHours
            paystub.SpecialHolidayRestDayNightDiffPay = _employeeRate.SpecialHolidayRestDayNightDiffPay
            paystub.Actual.SpecialHolidayRestDayNightDiffPay = _employeeRate.ActualSpecialHolidayRestDayNightDiffPay

            paystub.SpecialHolidayRestDayNightDiffOTHours = _employeeRate.SpecialHolidayRestDayNightDiffOTHours
            paystub.SpecialHolidayRestDayNightDiffOTPay = _employeeRate.SpecialHolidayRestDayNightDiffOTPay
            paystub.Actual.SpecialHolidayRestDayNightDiffOTPay = _employeeRate.ActualSpecialHolidayRestDayNightDiffOTPay

            paystub.RegularHolidayNightDiffHours = _employeeRate.RegularHolidayNightDiffHours
            paystub.RegularHolidayNightDiffPay = _employeeRate.RegularHolidayNightDiffPay
            paystub.Actual.RegularHolidayNightDiffPay = _employeeRate.ActualRegularHolidayNightDiffPay

            paystub.RegularHolidayNightDiffOTHours = _employeeRate.RegularHolidayNightDiffOTHours
            paystub.RegularHolidayNightDiffOTPay = _employeeRate.RegularHolidayNightDiffOTPay
            paystub.Actual.RegularHolidayNightDiffOTPay = _employeeRate.ActualRegularHolidayNightDiffOTPay

            paystub.RegularHolidayRestDayHours = _employeeRate.RegularHolidayRestDayHours
            paystub.RegularHolidayRestDayPay = _employeeRate.RegularHolidayRestDayPay
            paystub.Actual.RegularHolidayRestDayPay = _employeeRate.ActualRegularHolidayRestDayPay

            paystub.RegularHolidayRestDayOTHours = _employeeRate.RegularHolidayRestDayOTHours
            paystub.RegularHolidayRestDayOTPay = _employeeRate.RegularHolidayRestDayOTPay
            paystub.Actual.RegularHolidayRestDayOTPay = _employeeRate.ActualRegularHolidayRestDayOTPay

            paystub.RegularHolidayRestDayNightDiffHours = _employeeRate.RegularHolidayRestDayNightDiffHours
            paystub.RegularHolidayRestDayNightDiffPay = _employeeRate.RegularHolidayRestDayNightDiffPay
            paystub.Actual.RegularHolidayRestDayNightDiffPay = _employeeRate.ActualRegularHolidayRestDayNightDiffPay

            paystub.RegularHolidayRestDayNightDiffOTHours = _employeeRate.RegularHolidayRestDayNightDiffOTHours
            paystub.RegularHolidayRestDayNightDiffOTPay = _employeeRate.RegularHolidayRestDayNightDiffOTPay
            paystub.Actual.RegularHolidayRestDayNightDiffOTPay = _employeeRate.ActualRegularHolidayRestDayNightDiffOTPay

        End Sub

        Private Sub CreateAdjustments(paystub As Paystub)

            paystub.Adjustments = New List(Of Adjustment)

            For Each deduction In _selectedDeductions

                paystub.Adjustments.Add(New Adjustment With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .ProductID = deduction.Adjustment?.RowID,
                .Amount = deduction.Amount * -1 'to make it negative
            })

            Next

            For Each deduction In _selectedIncomes

                paystub.Adjustments.Add(New Adjustment With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .ProductID = deduction.Adjustment?.RowID,
                .Amount = deduction.Amount
            })

            Next
        End Sub

        Private Sub ComputeTotalEarnings(paystub As Paystub, employee As Employee)

            If employee.IsFixed Then

                paystub.TotalEarnings = paystub.BasicPay + paystub.AdditionalPay
            Else
                paystub.TotalEarnings =
                    paystub.RegularPay +
                    paystub.LeavePay +
                    paystub.AdditionalPay
            End If

        End Sub

        Private Function ConvertDaysToHours(days As Decimal) As Decimal

            Return days * BenchmarkPaystubRate.WorkHoursPerDay

        End Function

        Private Function GetOvertime(overtimeDescription As String) As Decimal

            Dim overtime = _overtimes.
            Where(Function(o) o.OvertimeType.Name = overtimeDescription).
            FirstOrDefault

            If overtime Is Nothing Then
                Return 0
            Else
                Return overtime.Hours
            End If

        End Function

    End Class

End Namespace