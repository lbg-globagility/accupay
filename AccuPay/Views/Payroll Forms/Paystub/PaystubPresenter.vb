Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class PaystubPresenter

    Private _currentPayperiod As PayPeriod

    Private _currentPaystub As Paystub

    Private _paystubs As IList(Of Paystub)

    Private _isActual As Boolean

    Private WithEvents _view As PaystubView

    Public Sub New(view As PaystubView)
        _view = view

    End Sub

    Private Async Sub OnInit() Handles _view.Init
        Await LoadPayperiods()
        Await LoadAdjustmentTypes()
    End Sub

    Private Async Function LoadPayperiods() As Task
        Dim payperiods As IList(Of PayPeriod) = Nothing

        Using repository = New Repository()
            payperiods = Await repository.GetPayperiods()
        End Using

        _view.ShowPayperiods(payperiods)
    End Function

    Private Async Function LoadAdjustmentTypes() As Task
        Using repository = New Repository()
            Dim adjustmentTypes = Await repository.GetAdjustmentTypes()

            _view.SetAdjustmentTypes(adjustmentTypes.Select(Function(a) a.PartNo).ToList())
        End Using
    End Function

    Private Async Sub OnPayperiodSelected(payperiod As PayPeriod) Handles _view.SelectPayperiod
        _currentPayperiod = payperiod

        Using repository = New Repository()
            _paystubs = Await repository.GetPaystubs(_currentPayperiod)
        End Using

        _view.ShowPaystubs(_paystubs)
    End Sub

    Private Async Sub OnPaystubSelected(paystub As Paystub) Handles _view.SelectPaystub
        _currentPaystub = paystub

        Dim salary As Salary = Nothing
        Dim paystubActual As PaystubActual = Nothing
        Dim adjustments As ICollection(Of Adjustment) = Nothing
        Dim allowanceItems As ICollection(Of AllowanceItem) = Nothing
        Dim loanTransactions As ICollection(Of LoanTransaction) = Nothing

        Using repository = New Repository()
            salary = Await repository.GetSalary(_currentPaystub)
            paystubActual = Await repository.GetPaystubActual(_currentPaystub.RowID)
            adjustments = Await repository.GetAdjustments(_currentPaystub.RowID)
            allowanceItems = Await repository.GetAllowanceItems(_currentPaystub.RowID)
            loanTransactions = Await repository.GetLoanTransactions(_currentPaystub.RowID)
        End Using

        _view.ShowSalary(_currentPaystub.Employee, salary, _isActual)
        _view.ShowPaystub(_currentPaystub, paystubActual, _isActual)
        _view.ShowAllowanceItems(allowanceItems)
        _view.ShowLoanTransactions(loanTransactions)
        _view.ShowAdjustments(adjustments)
    End Sub

    Private Sub OnToogleActual() Handles _view.ToggleActual
        _isActual = Not _isActual
        OnPaystubSelected(_currentPaystub)
    End Sub

    Private Sub OnSearch(term As String) Handles _view.Search
        Dim filteredPaystubs = FilterPaystubs(term)
        _view.ShowPaystubs(filteredPaystubs)
    End Sub

    Private Function FilterPaystubs(term As String) As IList(Of Paystub)
        If _paystubs Is Nothing Then
            Return New List(Of Paystub)()
        End If

        Dim matches =
            Function(employee As Employee)
                Dim containsEmployeeId = employee.EmployeeNo.ToLower().Contains(term)
                Dim containsFullName = employee.FullNameWithMiddleInitialLastNameFirst.ToLower().Contains(term)

                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                Dim containsFullNameInReverse = reverseFullName.Contains(term)

                Return containsEmployeeId Or containsFullName Or containsFullNameInReverse
            End Function

        Return _paystubs.Where(Function(p) matches(p.Employee)).ToList()
    End Function

    ''' <summary>
    ''' Repository
    ''' </summary>
    Private Class Repository
        Inherits DbRepository

        Private _employeeRepository As EmployeeRepository

        Private _payPeriodRepository As PayPeriodRepository

        Private _paystubRepository As PaystubRepository

        Private _paystubActualRepository As PaystubActualRepository

        Private _productRepository As ProductRepository

        Private _timeEntryRepository As TimeEntryRepository

        Sub New()

            _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

            _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

            _paystubRepository = MainServiceProvider.GetRequiredService(Of PaystubRepository)

            _paystubActualRepository = MainServiceProvider.GetRequiredService(Of PaystubActualRepository)

            _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

            _timeEntryRepository = MainServiceProvider.GetRequiredService(Of TimeEntryRepository)

        End Sub

        Public Async Function GetSalary(paystub As Paystub) As Task(Of Salary)

            If paystub.EmployeeID.HasValue = False Then
                Return Nothing
            End If

            Return Await _employeeRepository.GetCurrentSalaryAsync(
                paystub.EmployeeID.Value,
                paystub.PayToDate)
        End Function

        Public Async Function GetTimeEntries(employeeID As Integer?, dateFrom As Date, dateTo As Date) As Task(Of IList(Of TimeEntry))

            Return (Await _timeEntryRepository.
                            GetByEmployeeAndDatePeriodAsync(z_OrganizationID,
                                                            employeeID.Value,
                                                            New TimePeriod(dateFrom, dateTo))).
            ToList()

        End Function

        Public Async Function GetAllowanceItems(paystubId As Integer?) As Task(Of ICollection(Of AllowanceItem))

            If paystubId Is Nothing Then Return Nothing

            Return (Await _paystubRepository.GetAllowanceItemsAsync(paystubId.Value)).ToList()

        End Function

        Public Async Function GetLoanTransactions(paystubId As Integer?) As Task(Of IList(Of LoanTransaction))
            If paystubId Is Nothing Then Return Nothing

            Return (Await _paystubRepository.GetLoanTransactionsAsync(paystubId.Value)).ToList()
        End Function

        Public Async Function GetAdjustments(paystubId As Integer?) As Task(Of IList(Of Adjustment))

            If paystubId Is Nothing Then Return Nothing

            Return (Await _paystubRepository.GetAdjustmentsAsync(paystubId.Value)).ToList()
        End Function

        Public Async Function GetAdjustmentTypes() As Task(Of IList(Of Product))

            Return (Await _productRepository.GetAdjustmentTypesAsync(z_OrganizationID)).ToList()
        End Function

        Public Async Function GetPayperiods() As Task(Of IList(Of PayPeriod))

            Return (Await _payPeriodRepository.
                            GetAllSemiMonthlyThatHasPaystubsAsync(z_OrganizationID)).
                    ToList()
        End Function

        Public Async Function GetPaystubs(payPeriod As PayPeriod) As Task(Of IList(Of Paystub))

            If payPeriod?.RowID Is Nothing Then Return Nothing

            Return (Await _paystubRepository.
                            GetByPayPeriodWithEmployeeAsync(payPeriod.RowID.Value)).
                            OrderBy(Function(p) p.Employee.LastName).
                            ThenBy(Function(p) p.Employee.FirstName).
                            ToList()
        End Function

        Friend Function GetPaystubActual(paystubId As Integer?) As Task(Of PaystubActual)
            If paystubId Is Nothing Then Return Nothing
            Return _paystubActualRepository.GetByIdAsync(paystubId.Value)
        End Function

    End Class

End Class