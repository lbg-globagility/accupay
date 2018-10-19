Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Namespace Global.AccuPay.Views.Employees

    Public Class SalaryPresenter

        Private WithEvents _view As SalaryTab2
        Private _employee As Employee
        Private _salaries As List(Of Salary)
        Private StandardPagIbigContribution As Decimal = 100
        Private _philHealthDeductionType As String
        Private _philHealthContributionRate As Decimal
        Private _philHealthMinimumContribution As Decimal
        Private _philHealthMaximumContribution As Decimal
        Private _philHealthBrackets As List(Of PhilHealthBracket)
        Private _currentSalary As Salary
        Private _socialSecurityBrackets As List(Of SocialSecurityBracket)

        Public Sub New(view As SalaryTab2)
            _view = view
        End Sub

        Private Sub OnLoad() Handles _view.Init
            LoadPhilHealthBrackets()
            LoadSocialSecurityBrackets()
        End Sub

        Private Sub OnNew() Handles _view.NewSalary
            Dim salary = New Salary() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .EmployeeID = _employee.RowID,
                .PositionID = _employee.PositionID,
                .HDMFAmount = StandardPagIbigContribution,
                .EffectiveFrom = Date.Today,
                .EffectiveTo = Nothing
            }

            _view.DisplaySalary(salary)
            _view.DisableSalarySelection()
        End Sub

        Private Sub OnCancel() Handles _view.CancelChanges

        End Sub

        Private Sub OnDelete() Handles _view.DeleteSalary
            Using context = New PayrollContext()
                context.Salaries.Attach(_currentSalary)
                context.Salaries.Remove(_currentSalary)
                context.SaveChanges()
            End Using
        End Sub

        Private Sub OnSave() Handles _view.SaveSalary
            Using context = New PayrollContext()
                Try
                    Dim sssAmount = If(_view.Sss, 0)

                    Dim socialSecurityBracket = context.SocialSecurityBrackets.
                        FirstOrDefault(Function(s) s.EmployeeContributionAmount = sssAmount)

                    With _currentSalary
                        .BasicSalary = _view.BasicSalary
                        .AllowanceSalary = _view.AllowanceSalary
                        .TotalSalary = (.BasicSalary + .AllowanceSalary)
                        .EffectiveFrom = _view.EffectiveFrom
                        .EffectiveTo = _view.EffectiveTo
                        .PhilHealthDeduction = If(_view.PhilHealth, 0D)
                        .PaySocialSecurityID = socialSecurityBracket?.RowID
                        .SocialSecurityBracket = socialSecurityBracket
                        .HDMFAmount = _view.PagIBIG
                    End With

                    If _currentSalary.RowID.HasValue Then
                        _currentSalary.LastUpdBy = z_User
                        context.Entry(_currentSalary).State = EntityState.Modified
                    Else
                        context.Salaries.Add(_currentSalary)
                    End If

                    context.SaveChanges()
                Catch ex As Exception
                    MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation) ' Remove this
                    Throw
                End Try
            End Using
            LoadSalaries()
        End Sub

        Private Sub OnAmountChanged(amount As Decimal) Handles _view.SalaryChanged
            Dim monthlyRate = 0D

            If _employee.IsDaily Then
                monthlyRate = amount * PayrollTools.GetWorkDaysPerMonth(_employee.WorkDaysPerYear)
                _view.BasicSalary = amount
            ElseIf _employee.EmployeeType = "Monthly" Or _employee.EmployeeType = "Fixed" Then
                monthlyRate = amount

                If _employee.PayFrequency.Type = "SEMI-MONTHLY" Then

                End If
            End If

            If _currentSalary Is Nothing Then
                Return
            End If

            UpdateSss(monthlyRate)
            UpdatePhilHealth(monthlyRate)
        End Sub

        Private Sub LoadPhilHealthBrackets()
            Using context = New PayrollContext()
                Dim listOfValues = context.ListOfValues.
                    Where(Function(l) l.Type = "PhilHealth").
                    ToList()

                Dim values = New ListOfValueCollection(listOfValues)

                _philHealthDeductionType = If(values.GetValue("DeductionType"), "Bracket")
                _philHealthContributionRate = values.GetDecimal("Rate")
                _philHealthMinimumContribution = values.GetDecimal("MinimumContribution")
                _philHealthMaximumContribution = values.GetDecimal("MaximumContribution")

                _philHealthBrackets = context.PhilHealthBrackets.ToList()
            End Using
        End Sub

        Private Sub LoadSalaries()
            If _employee Is Nothing Then
                Return
            End If

            Dim salaries As IList(Of Salary)

            Using context = New PayrollContext()
                salaries = (From s In context.Salaries.Include(Function(s) s.SocialSecurityBracket)
                            Where CBool(s.EmployeeID = _employee.RowID)
                            Order By s.EffectiveFrom Descending).
                             ToList()
            End Using

            _view.DisplaySalaries(salaries)

            If _currentSalary IsNot Nothing Then
                _view.ActivateSelectedSalary(_currentSalary)
            End If
        End Sub

        Private Sub LoadSocialSecurityBrackets()
            Using context = New PayrollContext()
                _socialSecurityBrackets = context.SocialSecurityBrackets.ToList()
            End Using
        End Sub

        Private Sub UpdateSss(monthlyRate As Decimal)
            Dim socialSecurityBracket = _socialSecurityBrackets?.FirstOrDefault(
                    Function(s) s.RangeFromAmount <= monthlyRate And monthlyRate <= s.RangeToAmount)

            _view.Sss = socialSecurityBracket.EmployeeContributionAmount
        End Sub

        Private Sub UpdatePhilHealth(monthlyRate As Decimal)
            Dim philHealthContribution = 0D
            If _philHealthDeductionType = "Formula" Then
                philHealthContribution = monthlyRate * (_philHealthContributionRate / 100)

                philHealthContribution = {philHealthContribution, _philHealthMinimumContribution}.Max()
                philHealthContribution = {philHealthContribution, _philHealthMaximumContribution}.Min()
                philHealthContribution = AccuMath.Truncate(philHealthContribution, 2)
            Else
                Dim philHealthBracket = _philHealthBrackets?.FirstOrDefault(
                    Function(p) p.SalaryRangeFrom <= monthlyRate And monthlyRate <= p.SalaryRangeTo)

                _currentSalary.PayPhilHealthID = philHealthBracket?.RowID

                philHealthContribution = If(philHealthBracket?.TotalMonthlyPremium, 0)
            End If

            _view.PhilHealth = philHealthContribution
        End Sub

    End Class

End Namespace
