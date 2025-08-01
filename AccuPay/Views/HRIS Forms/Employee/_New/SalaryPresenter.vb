Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Namespace Global.AccuPay.Views.Employees

    Public Class SalaryPresenter

        Private Const StandardPagIbigContribution As Decimal = 100

        Private WithEvents _view As SalaryTab2

        Private _employee As Employee

        Private _socialSecurityPolicy As SocialSecurityPolicy

        Private _listOfValueService As IListOfValueService

        Private _salaries As IList(Of Salary)

        Private _currentSalary As Salary

        Private ReadOnly _philHealthBracketRepository As IPhilHealthBracketRepository

        Private ReadOnly _salaryRepository As ISalaryRepository

        Private ReadOnly _socialSecurityBracketRepository As ISocialSecurityBracketRepository

        Public Sub New(view As SalaryTab2)
            _view = view

            _philHealthBracketRepository = MainServiceProvider.GetRequiredService(Of IPhilHealthBracketRepository)

            _salaryRepository = MainServiceProvider.GetRequiredService(Of ISalaryRepository)

            _socialSecurityBracketRepository = MainServiceProvider.GetRequiredService(Of ISocialSecurityBracketRepository)

            _listOfValueService = MainServiceProvider.GetRequiredService(Of IListOfValueService)

        End Sub

        Private Sub OnLoad() Handles _view.Init
            LoadSocialSecurityBrackets()
            _view.ChangeMode(SalaryTab2.Mode.Disabled)
        End Sub

        Private Async Sub OnSelectedEmployee(employee As Employee) Handles _view.SelectEmployee
            _employee = employee
            _view.ShowEmployee(employee)
            Await LoadSalaries()
        End Sub

        Private Sub OnNew() Handles _view.NewSalary
            _currentSalary = New Salary() With {
                .OrganizationID = z_OrganizationID,
                .EmployeeID = _employee.RowID,
                .PositionID = _employee.PositionID,
                .HDMFAmount = StandardPagIbigContribution,
                .EffectiveFrom = Date.Today
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

        Private Async Sub OnDelete() Handles _view.DeleteSalary

            If _currentSalary?.RowID Is Nothing Then Return

            Dim dataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)
            Await dataService.DeleteAsync(
                id:=_currentSalary.RowID.Value,
                currentlyLoggedInUserId:=z_User)
            _currentSalary = Nothing
            Await LoadSalaries()
        End Sub

        Private Async Sub OnSave() Handles _view.SaveSalary
            Try
                With _currentSalary
                    .BasicSalary = _view.BasicSalary
                    .AllowanceSalary = _view.AllowanceSalary
                    .EffectiveFrom = _view.EffectiveFrom
                    .PhilHealthDeduction = If(_view.PhilHealth, 0D)
                    .HDMFAmount = _view.PagIBIG
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of ISalaryDataService)
                Await dataService.SaveAsync(_currentSalary, z_User)
            Catch ex As Exception
                MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation) ' Remove this
                Throw
            End Try
            Await LoadSalaries()
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
        End Sub

        Private Sub OnSalarySelected(salary As Salary) Handles _view.SelectSalary
            _currentSalary = salary

            _view.ChangeMode(SalaryTab2.Mode.Editing)
            _view.DisplaySalary(_currentSalary)
        End Sub

        Private Sub OnSssDelete() Handles _view.DeleteSss
            _view.Sss = Nothing
        End Sub

        Private Async Function LoadSalaries() As Task
            If _employee?.RowID Is Nothing Then
                Return
            End If

            _salaries = (Await _salaryRepository.GetByEmployeeAsync(_employee.RowID.Value)).
                OrderByDescending(Function(s) s.EffectiveFrom).
                ToList()

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
        End Function

        Private Sub LoadSocialSecurityBrackets()
            Dim socialSecurityBrackets = _socialSecurityBracketRepository.GetAll().ToList()

            _socialSecurityPolicy = New SocialSecurityPolicy(socialSecurityBrackets)
        End Sub

        Private Sub UpdateSss(monthlyRate As Decimal)
            Dim bracket = _socialSecurityPolicy.GetBracket(monthlyRate)

            _view.Sss = bracket?.EmployeeContributionAmount
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
