Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class PhilHealthCalculator

        Private Class ContributionSchedule
            Public Const FirstHalf As String = "First half"
            Public Const EndOfTheMonth As String = "End of the month"
            Public Const PerPayPeriod As String = "Per pay period"
        End Class

        Private ReadOnly _philHealthBrackets As ICollection(Of PhilHealthBracket)

        Private ReadOnly ecolaName As String = "ecola"

        Public Sub New(philHealthBrackets As ICollection(Of PhilHealthBracket))
            _philHealthBrackets = philHealthBrackets
        End Sub

        Public Sub Calculate(settings As ListOfValueCollection, salary As Salary, paystub As Paystub, previousPaystub As Paystub, employee As Employee, payperiod As PayPeriod, allowances As ICollection(Of Allowance))
            If salary.PhilHealthDeduction = 0 Then
                paystub.PhilHealthEmployeeShare = 0
                paystub.PhilHealthEmployerShare = 0
                Return
            End If

            Dim deductionSchedule = employee.PhilHealthSchedule

            Dim philHealthCalculation = settings.GetEnum(
                "PhilHealth.CalculationBasis",
                PhilHealthCalculationBasis.BasicSalary)

            Dim isPhilHealthProrated =
                (philHealthCalculation = PhilHealthCalculationBasis.Earnings) Or
                (philHealthCalculation = PhilHealthCalculationBasis.GrossPay)

            Dim totalContribution = 0D
            If philHealthCalculation = PhilHealthCalculationBasis.BasicSalary Then
                ' If philHealth calculation is based on the basic salary, get it from the salary record
                totalContribution = salary.PhilHealthDeduction
            ElseIf philHealthCalculation = PhilHealthCalculationBasis.BasicAndEcola Then
                totalContribution = salary.PhilHealthDeduction

                Dim ecolas = allowances.Any(Function(ea) ea.Product.PartNo.ToLower() = ecolaName)
                If ecolas Then
                    Dim ecola = allowances.FirstOrDefault(Function(ea) ea.Product.PartNo.ToLower() = ecolaName)

                    Dim phHSetting = settings.GetSublist("PhilHealth")
                    Dim rate = phHSetting.GetDecimal("Rate") / 100

                    Dim ecolaPhHContribAmount = (ecola.Amount * (employee.WorkDaysPerYear / CalendarConstants.MonthsInAYear)) * rate
                    totalContribution += ecolaPhHContribAmount
                End If

            ElseIf isPhilHealthProrated Then
                Dim basisPay = 0D

                If philHealthCalculation = PhilHealthCalculationBasis.Earnings Then
                    basisPay = If(previousPaystub?.TotalEarnings, 0) + paystub.TotalEarnings
                ElseIf philHealthCalculation = PhilHealthCalculationBasis.GrossPay Then
                    basisPay = If(previousPaystub?.GrossPay, 0) + paystub.GrossPay
                End If

                totalContribution = ComputePhilHealth(basisPay, settings)
            End If

            Dim halfContribution = AccuMath.Truncate(totalContribution / 2, 2)

            Dim philHealthNoRemainder = settings.GetBoolean("PhilHealth.Remainder", True)

            Dim remainder = 0D
            ' Account for any division loss by putting the missing value to the employer share
            If philHealthNoRemainder Then
                Dim expectedTotal = halfContribution * 2

                If expectedTotal < totalContribution Then
                    remainder = totalContribution - expectedTotal
                End If
            End If

            Dim employeeShare = halfContribution
            Dim employerShare = halfContribution + remainder

            If employee.IsWeeklyPaid Then
                Dim is_deduct_sched_to_thisperiod = If(
                    employee.IsUnderAgency,
                    payperiod.PhHWeeklyAgentContribSched,
                    payperiod.PhHWeeklyContribSched)

                If is_deduct_sched_to_thisperiod Then
                    paystub.PhilHealthEmployeeShare = employeeShare
                    paystub.PhilHealthEmployerShare = employerShare
                Else
                    paystub.PhilHealthEmployeeShare = 0
                    paystub.PhilHealthEmployerShare = 0
                End If
            Else
                If IsPhilHealthPaidOnFirstHalf(deductionSchedule, payperiod) Or IsPhilHealthPaidOnEndOfTheMonth(deductionSchedule, payperiod) Then
                    paystub.PhilHealthEmployeeShare = employeeShare
                    paystub.PhilHealthEmployerShare = employerShare
                ElseIf IsPhilHealthPaidPerPayPeriod(deductionSchedule) Then
                    paystub.PhilHealthEmployeeShare = employeeShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                    paystub.PhilHealthEmployerShare = employerShare / CalendarConstants.SemiMonthlyPayPeriodsPerMonth
                Else
                    paystub.PhilHealthEmployeeShare = 0
                    paystub.PhilHealthEmployerShare = 0
                End If
            End If
        End Sub

        Private Function ComputePhilHealth(basis As Decimal, settings As ListOfValueCollection) As Decimal
            Dim philHealthSettings = settings.GetSublist("PhilHealth")

            Dim minimum = philHealthSettings.GetDecimal("MinimumContribution")
            Dim maximum = philHealthSettings.GetDecimal("MaximumContribution")
            Dim rate = philHealthSettings.GetDecimal("Rate") / 100

            ' Contribution should be bounded by the minimum and maximum
            Dim contribution = {{basis * rate, minimum}.Max(), maximum}.Min()
            ' Truncate to the nearest cent
            contribution = AccuMath.Truncate(contribution, 2)

            Return contribution
        End Function

        Private Function FindMatchingBracket(amount As Decimal) As PhilHealthBracket
            Return _philHealthBrackets.FirstOrDefault(
                Function(p) p.SalaryRangeFrom <= amount And p.SalaryRangeTo >= amount)
        End Function

        Private Function IsPhilHealthPaidOnFirstHalf(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsFirstHalf And (deductionSchedule = ContributionSchedule.FirstHalf)
        End Function

        Private Function IsPhilHealthPaidOnEndOfTheMonth(deductionSchedule As String, payperiod As PayPeriod) As Boolean
            Return payperiod.IsEndOfTheMonth And (deductionSchedule = ContributionSchedule.EndOfTheMonth)
        End Function

        Private Function IsPhilHealthPaidPerPayPeriod(deductionSchedule As String) As Boolean
            Return deductionSchedule = ContributionSchedule.PerPayPeriod
        End Function

    End Class

End Namespace
