Option Strict On

Imports AccuPay.Entity
Imports PayrollSys

Namespace Global.AccuPay.Payroll

    Public Class ThirteenthMonthPayCalculator

        Public Sub Calculate(_employee As Employee,
                           paystub As Paystub,
                           timeEntries As ICollection(Of TimeEntry),
                           actualtimeentries As ICollection(Of ActualTimeEntry),
                           salary As Salary,
                           settings As ListOfValueCollection,
                           allowanceItems As ICollection(Of AllowanceItem))

            If paystub.ThirteenthMonthPay Is Nothing Then
                paystub.ThirteenthMonthPay = New ThirteenthMonthPay() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User
                }
            Else
                paystub.ThirteenthMonthPay.LastUpdBy = z_User
            End If

            Dim contractualEmployementStatuses = New String() {"Contractual", "SERVICE CONTRACT"}

            Dim thirteenthMonthAmount = 0D

            If _employee.IsDaily Then

                If contractualEmployementStatuses.Contains(_employee.EmploymentStatus) Then

                    thirteenthMonthAmount = timeEntries.
                                                Where(Function(t) Not t.IsRestDay).
                                                Sum(Function(t) t.BasicDayPay + t.LeavePay)
                Else

                    Dim thirteenthMonthAmountRunningTotal As Decimal = 0

                    For Each actualTimeEntry In actualtimeentries

                        Dim timeEntry = timeEntries.Where(Function(t) t.Date = actualTimeEntry.Date).FirstOrDefault

                        If timeEntry Is Nothing OrElse timeEntry.IsRestDay Then Continue For

                        thirteenthMonthAmount += actualTimeEntry.BasicDayPay + actualTimeEntry.LeavePay

                    Next

                End If

            ElseIf _employee.IsMonthly Or _employee.IsFixed Then

                Dim trueSalary = salary.TotalSalary
                Dim basicPay = trueSalary / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

                Dim totalDeductions = actualtimeentries.Sum(Function(t) t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction)

                Dim additionalAmount As Decimal
                If (settings.GetBoolean("ThirteenthMonthPolicy.IsAllowancePaid")) Then

                    additionalAmount = allowanceItems.Sum(Function(a) a.Amount)

                End If

                thirteenthMonthAmount = ((basicPay + additionalAmount) - totalDeductions)
            End If

            Dim allowanceAmount = allowanceItems.Where(Function(a) a.IsThirteenthMonthPay).Sum(Function(a) a.Amount)
            thirteenthMonthAmount += allowanceAmount

            paystub.ThirteenthMonthPay.BasicPay = thirteenthMonthAmount
            paystub.ThirteenthMonthPay.Amount = thirteenthMonthAmount / CalendarConstants.MonthsInAYear
            paystub.ThirteenthMonthPay.Paystub = paystub
        End Sub

    End Class

End Namespace