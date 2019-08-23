Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class PhilHealthCalculator

        Private ReadOnly _policy As PhilHealthPolicy
        Private ReadOnly _philHealthBrackets As ICollection(Of PhilHealthBracket)

        Private Const EcolaName As String = "ecola"

        Public Sub New(policy As PhilHealthPolicy, philHealthBrackets As ICollection(Of PhilHealthBracket))
            _policy = policy
            _philHealthBrackets = philHealthBrackets
        End Sub

        Public Sub Calculate(salary As Salary, paystub As Paystub, previousPaystub As Paystub, employee As Employee, payperiod As PayPeriod, allowances As ICollection(Of Allowance))
            ' Reset the PhilHealth to zero
            paystub.PhilHealthEmployeeShare = 0
            paystub.PhilHealthEmployerShare = 0

            Dim totalContribution As Decimal

            ' If auto compute the PhilHealth is true, then we use the available formulas to compute the total contribution.
            ' Otherwise, we use whatever amount is set in the salary.
            If salary.AutoComputePhilHealthContribution Then
                totalContribution = GetTotalContribution(
                    salary, paystub, previousPaystub, employee, allowances)
            Else
                totalContribution = salary.PhilHealthDeduction
            End If

            ' If totalContribution is zero, then the employee has no PhilHealth to pay
            If totalContribution <= 0 Then
                Return
            End If

            Dim halfContribution = AccuMath.Truncate(totalContribution / 2, 2)

            Dim employeeShare = halfContribution
            Dim employerShare = halfContribution

            ' Account for any division loss by putting the missing value to the employer's share
            If _policy.OddCentDifference Then
                Dim expectedTotal = halfContribution * 2
                Dim remainder As Decimal

                If expectedTotal < totalContribution Then
                    remainder = totalContribution - expectedTotal
                End If

                employerShare += remainder
            End If

            If employee.IsWeeklyPaid Then
                Dim is_deduct_sched_to_thisperiod = If(
                    employee.IsUnderAgency,
                    payperiod.PhHWeeklyAgentContribSched,
                    payperiod.PhHWeeklyContribSched)

                If is_deduct_sched_to_thisperiod Then
                    paystub.PhilHealthEmployeeShare = employeeShare
                    paystub.PhilHealthEmployerShare = employerShare
                End If
            Else
                Dim deductionSchedule = employee.PhilHealthSchedule

                If IsPhilHealthPaidOnFirstHalf(deductionSchedule, payperiod) Or IsPhilHealthPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then
                    paystub.PhilHealthEmployeeShare = employeeShare
                    paystub.PhilHealthEmployerShare = employerShare
                ElseIf IsPhilHealthPaidPerPayPeriod(deductionSchedule) Then
                    paystub.PhilHealthEmployeeShare = employeeShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                    paystub.PhilHealthEmployerShare = employerShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                End If
            End If
        End Sub

        Private Function GetTotalContribution(salary As Salary,
                                              paystub As Paystub,
                                              previousPaystub As Paystub,
                                              employee As Employee,
                                              allowances As ICollection(Of Allowance)) As Decimal

            Dim calculationBasis = _policy.CalculationBasis

            Dim basisPay = 0D

            ' If philHealth calculation is based on the basic salary, get it from the salary record
            If calculationBasis = PhilHealthCalculationBasis.BasicSalary Then

                basisPay = PayrollTools.GetEmployeeMonthlyRate(employee, salary.BasicSalary)

            ElseIf calculationBasis = PhilHealthCalculationBasis.BasicAndEcola Then

                Dim monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee, salary.BasicSalary)

                Dim ecolas = allowances.
                    Where(Function(ea) ea.Product.PartNo.ToLower() = EcolaName)

                Dim ecolaPerMonth = 0D
                If ecolas.Any() Then
                    Dim ecola = ecolas.FirstOrDefault()

                    ecolaPerMonth = ecola.Amount * (employee.WorkDaysPerYear / CalendarConstants.MonthsInAYear)
                End If

                basisPay = monthlyRate + ecolaPerMonth

            ElseIf calculationBasis = PhilHealthCalculationBasis.Earnings Then

                basisPay = If(previousPaystub?.TotalEarnings, 0) + paystub.TotalEarnings

            ElseIf calculationBasis = PhilHealthCalculationBasis.GrossPay Then

                basisPay = If(previousPaystub?.GrossPay, 0) + paystub.GrossPay

            ElseIf calculationBasis = PhilHealthCalculationBasis.BasicMinusDeductions Then

                basisPay = If(previousPaystub?.TotalDaysPayWithOutOvertimeAndLeave, 0) + paystub.TotalDaysPayWithOutOvertimeAndLeave

            End If

            Dim totalContribution = ComputePhilHealth(basisPay)

            Return totalContribution
        End Function

        Private Function ComputePhilHealth(basis As Decimal) As Decimal
            Dim minimum = _policy.MinimumContribution
            Dim maximum = _policy.MaximumContribution
            Dim rate = _policy.Rate / 100

            ' Contribution should be bounded by the minimum and maximum
            Dim contribution = {{basis * rate, minimum}.Max(), maximum}.Min()
            ' Round to the nearest cent
            contribution = AccuMath.CommercialRound(contribution)

            Return contribution
        End Function

        <Obsolete>
        Private Function FindMatchingBracket(amount As Decimal) As PhilHealthBracket
            Return _philHealthBrackets.FirstOrDefault(
                Function(p) p.SalaryRangeFrom <= amount And p.SalaryRangeTo >= amount)
        End Function

        Private Function IsPhilHealthPaidOnFirstHalf(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FIRST_HALF)
        End Function

        Private Function IsPhilHealthPaidOnEndOfTheMonth(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.END_OF_THE_MONTH)
        End Function

        Private Function IsPhilHealthPaidPerPayPeriod(deductionSchedule As String) As Boolean
            Return deductionSchedule = ContributionSchedule.PER_PAY_PERIOD
        End Function

    End Class

End Namespace