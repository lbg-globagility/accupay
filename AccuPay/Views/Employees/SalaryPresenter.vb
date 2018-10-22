Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Namespace Global.AccuPay.Views.Employees

    Public Class SalaryPresenter

        Private Const StandardPagIbigContribution As Decimal = 100

        Private WithEvents _view As SalaryTab2

        Private _employee As Employee

        Private _philHealthPolicy As PhilHealthPolicy

        Private _currentSalary As Salary

        Private _socialSecurityBrackets As List(Of SocialSecurityBracket)

        Public Sub New(view As SalaryTab2)
            _view = view
        End Sub

        Private Sub OnLoad() Handles _view.Init
            LoadPhilHealthBrackets()
            LoadSocialSecurityBrackets()
            _view.ChangeMode(SalaryTab2.Mode.Disabled)
        End Sub

        Private Sub OnSelectedEmployee(employee As Employee) Handles _view.SelectEmployee
            _employee = employee
            _view.ShowEmployee(employee)
            LoadSalaries()
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
            _view.ChangeMode(SalaryTab2.Mode.Creating)
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

        Private Sub OnSalarySelected(salary As Salary) Handles _view.SelectSalary
            _currentSalary = salary

            _view.DisplaySalary(_currentSalary)
        End Sub

        Private Sub LoadPhilHealthBrackets()
            Using context = New PayrollContext()
                Dim listOfValues = context.ListOfValues.
                    Where(Function(l) l.Type = "PhilHealth").
                    ToList()

                Dim values = New ListOfValueCollection(listOfValues)

                _philHealthPolicy = New PhilHealthPolicy() With {
                    .DeductionType = values.GetStringOrDefault("DeductionType", "Bracket"),
                    .ContributionRate = values.GetDecimal("Rate"),
                    .MinimumContribution = values.GetDecimal("MinimumContribution"),
                    .MaximumContribution = values.GetDecimal("MaximumContribution"),
                    .Brackets = context.PhilHealthBrackets.ToList()
                }
            End Using
        End Sub

        Private Sub LoadSalaries()
            If _employee Is Nothing Then
                Return
            End If

            Dim salaries As IList(Of Salary)

            Using context = New PayrollContext()
                salaries = context.Salaries.
                    Include(Function(s) s.SocialSecurityBracket).
                    Where(Function(s) Nullable.Equals(s.EmployeeID, _employee.RowID)).
                    OrderByDescending(Function(s) s.EffectiveFrom).
                    ToList()
            End Using

            If _currentSalary Is Nothing Then
                _currentSalary = salaries.FirstOrDefault()
            End If

            _view.DisplaySalary(_currentSalary)
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
            If _philHealthPolicy.DeductionType = "Formula" Then
                philHealthContribution = monthlyRate * (_philHealthPolicy.ContributionRate / 100)

                philHealthContribution = {philHealthContribution, _philHealthPolicy.MinimumContribution}.Max()
                philHealthContribution = {philHealthContribution, _philHealthPolicy.MaximumContribution}.Min()
                philHealthContribution = AccuMath.Truncate(philHealthContribution, 2)
            Else
                Dim philHealthBracket = _philHealthPolicy.Brackets?.FirstOrDefault(
                    Function(p) p.SalaryRangeFrom <= monthlyRate And monthlyRate <= p.SalaryRangeTo)

                _currentSalary.PayPhilHealthID = philHealthBracket?.RowID

                philHealthContribution = If(philHealthBracket?.TotalMonthlyPremium, 0)
            End If

            _view.PhilHealth = philHealthContribution
        End Sub

        Private Class PhilHealthPolicy

            Public Property DeductionType As String

            Public Property ContributionRate As Decimal

            Public Property MinimumContribution As Decimal

            Public Property MaximumContribution As Decimal

            Public Property Brackets As List(Of PhilHealthBracket)

        End Class

    End Class

End Namespace
