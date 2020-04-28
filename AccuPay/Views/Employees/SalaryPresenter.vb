Option Strict On

Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports AccuPay.Utilities
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Namespace Global.AccuPay.Views.Employees

    Public Class SalaryPresenter

        Private Const StandardPagIbigContribution As Decimal = 100

        Private WithEvents _view As SalaryTab2

        Private _employee As Data.Entities.Employee

        Private _philHealthPolicy As PhilHealthPolicy

        Private _socialSecurityPolicy As SocialSecurityPolicy

        Private _salaries As IList(Of Salary)

        Private _currentSalary As Salary

        Public Sub New(view As SalaryTab2)
            _view = view
        End Sub

        Private Sub OnLoad() Handles _view.Init
            LoadPhilHealthBrackets()
            LoadSocialSecurityBrackets()
            _view.ChangeMode(SalaryTab2.Mode.Disabled)
        End Sub

        Private Sub OnSelectedEmployee(employee As Data.Entities.Employee) Handles _view.SelectEmployee
            _employee = employee
            _view.ShowEmployee(employee)
            LoadSalaries()
        End Sub

        Private Sub OnNew() Handles _view.NewSalary
            _currentSalary = New Salary() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .EmployeeID = _employee.RowID,
                .PositionID = _employee.PositionID,
                .HDMFAmount = StandardPagIbigContribution,
                .EffectiveFrom = Date.Today,
                .EffectiveTo = Nothing
            }

            _view.DisplaySalary(_currentSalary)
            _view.DisableSalarySelection()
            _view.ChangeMode(SalaryTab2.Mode.Creating)
        End Sub

        Private Sub OnCancel() Handles _view.CancelChanges
            If _view.CurrentMode = SalaryTab2.Mode.Creating Then
                _currentSalary = _salaries.FirstOrDefault()
                _view.DisplaySalary(_currentSalary)
                _view.ChangeMode(SalaryTab2.Mode.Editing)
                _view.EnableSalarySelection()
            ElseIf _view.CurrentMode = SalaryTab2.Mode.Editing Then
                _view.DisplaySalary(_currentSalary)
            End If
        End Sub

        Private Sub OnDelete() Handles _view.DeleteSalary
            Using context = New PayrollContext()
                context.Salaries.Attach(_currentSalary)
                context.Salaries.Remove(_currentSalary)
                context.SaveChanges()
            End Using
            _currentSalary = Nothing
            LoadSalaries()
        End Sub

        Private Sub OnSave() Handles _view.SaveSalary
            Using context = New PayrollContext()
                Try
                    Dim effectiveTo = _view.EffectiveTo

                    With _currentSalary
                        .BasicSalary = _view.BasicSalary
                        .AllowanceSalary = _view.AllowanceSalary
                        .TotalSalary = (.BasicSalary + .AllowanceSalary)
                        .EffectiveFrom = _view.EffectiveFrom
                        .EffectiveTo = effectiveTo
                        .PhilHealthDeduction = If(_view.PhilHealth, 0D)
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
            Dim basicPay = 0D

            If _employee.IsDaily Then
                monthlyRate = amount * PayrollTools.GetWorkDaysPerMonth(_employee.WorkDaysPerYear)

                basicPay = amount
            ElseIf _employee.IsMonthly Or _employee.IsFixed Then
                monthlyRate = amount

                If _employee.PayFrequency.IsSemiMonthly Then
                    basicPay = monthlyRate / 2
                End If
            End If

            If _currentSalary Is Nothing Then
                Return
            End If

            _view.BasicPay = basicPay
            UpdateSss(monthlyRate)
            UpdatePhilHealth(monthlyRate)
        End Sub

        Private Sub OnSalarySelected(salary As Salary) Handles _view.SelectSalary
            _currentSalary = salary

            _view.ChangeMode(SalaryTab2.Mode.Editing)
            _view.DisplaySalary(_currentSalary)
        End Sub

        Private Sub OnSssDelete() Handles _view.DeleteSss
            _view.Sss = Nothing
        End Sub

        Private Sub LoadPhilHealthBrackets()

            Dim values = ListOfValueCollection.Create("PhilHealth")

            Using context = New PayrollContext()

                _philHealthPolicy = New PhilHealthPolicy(
                    values.GetStringOrDefault("DeductionType", "Bracket"),
                    values.GetDecimal("Rate"),
                    values.GetDecimal("MinimumContribution"),
                    values.GetDecimal("MaximumContribution"),
                    context.PhilHealthBrackets.ToList())
            End Using
        End Sub

        Private Sub LoadSalaries()
            If _employee Is Nothing Then
                Return
            End If

            Using context = New PayrollContext()
                _salaries = context.Salaries.
                    Where(Function(s) Nullable.Equals(s.EmployeeID, _employee.RowID)).
                    OrderByDescending(Function(s) s.EffectiveFrom).
                    ToList()
            End Using

            If _currentSalary Is Nothing OrElse _currentSalary.EmployeeID <> _employee.RowID Then
                _currentSalary = _salaries.FirstOrDefault()
            End If

            _view.DisplaySalaries(_salaries)

            If _currentSalary IsNot Nothing Then
                _view.DisplaySalary(_currentSalary)
                _view.ActivateSelectedSalary(_currentSalary)
                _view.ChangeMode(SalaryTab2.Mode.Editing)
            Else
                _view.ChangeMode(SalaryTab2.Mode.Empty)
            End If
        End Sub

        Private Sub LoadSocialSecurityBrackets()
            Using context = New PayrollContext()
                Dim socialSecurityBrackets = context.SocialSecurityBrackets.ToList()

                _socialSecurityPolicy = New SocialSecurityPolicy(socialSecurityBrackets)
            End Using
        End Sub

        Private Sub UpdateSss(monthlyRate As Decimal)
            Dim bracket = _socialSecurityPolicy.GetBracket(monthlyRate)

            _view.Sss = bracket?.EmployeeContributionAmount
        End Sub

        Private Sub UpdatePhilHealth(monthlyRate As Decimal)
            Dim philHealthContribution = _philHealthPolicy.GetContribution(monthlyRate)

            _view.PhilHealth = philHealthContribution
        End Sub

        Private Class PhilHealthPolicy

            Private Property _deductionType As String

            Private Property _contributionRate As Decimal

            Private Property _minimumContribution As Decimal

            Private Property _maximumContribution As Decimal

            Private Property _brackets As IList(Of PhilHealthBracket)

            Public Sub New(deductionType As String,
                           contributionRate As Decimal,
                           minimumContribution As Decimal,
                           maximumContribution As Decimal,
                           brackets As IList(Of PhilHealthBracket))
                _deductionType = deductionType
                _contributionRate = contributionRate
                _minimumContribution = minimumContribution
                _maximumContribution = maximumContribution
                _brackets = brackets
            End Sub

            Public Function GetContribution(monthlyRate As Decimal) As Decimal
                Dim contribution = 0D

                If _deductionType = "Formula" Then
                    contribution = monthlyRate * (_contributionRate / 100)

                    contribution = {contribution, _minimumContribution}.Max()
                    contribution = {contribution, _maximumContribution}.Min()

                    contribution = AccuMath.Truncate(contribution, 2)
                Else
                    Dim philHealthBracket = _brackets?.FirstOrDefault(
                        Function(p) p.SalaryRangeFrom <= monthlyRate And monthlyRate <= p.SalaryRangeTo)

                    contribution = If(philHealthBracket?.TotalMonthlyPremium, 0)
                End If

                Return contribution
            End Function

        End Class

        Private Class SocialSecurityPolicy

            Public _socialSecurityBrackets As IList(Of SocialSecurityBracket)

            Public Sub New(socialSecurityBrackets As IList(Of SocialSecurityBracket))
                _socialSecurityBrackets = socialSecurityBrackets
            End Sub

            Public Function GetBracket(monthlyRate As Decimal) As SocialSecurityBracket
                Dim socialSecurityBracket = _socialSecurityBrackets?.FirstOrDefault(
                        Function(s) s.RangeFromAmount <= monthlyRate And monthlyRate <= s.RangeToAmount)

                Return socialSecurityBracket
            End Function

        End Class

    End Class

End Namespace