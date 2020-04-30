Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class PaystubPresenter

    Private _currentPayperiod As PayPeriod

    Private _currentPaystub As Paystub

    Private _paystubs As IList(Of Paystub)

    Private _isActual As Boolean

    Private _paystubActualRepository As PaystubActualRepository

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
        Dim paystubActual As Data.Entities.PaystubActual = Nothing
        Dim adjustments As ICollection(Of Adjustment) = Nothing
        Dim allowanceItems As ICollection(Of AllowanceItem) = Nothing
        Dim loanTransactions As ICollection(Of LoanTransaction) = Nothing

        Using repository = New Repository()
            salary = Await repository.GetSalary(_currentPaystub)
            paystubActual = Await _paystubActualRepository.GetByIdAsync(_currentPaystub.RowID.Value)
            adjustments = Await repository.GetAdjustments(_currentPaystub)
            allowanceItems = Await repository.GetAllowanceItems(_currentPaystub.RowID)
            loanTransactions = Await repository.GetLoanTransactions(_currentPaystub.PayPeriodID, _currentPaystub.EmployeeID)
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

        Public Async Function GetSalary(paystub As Paystub) As Task(Of Salary)
            Dim query = _context.Salaries.
                Where(Function(s) Nullable.Equals(s.EmployeeID, paystub.EmployeeID)).
                Where(Function(s) s.EffectiveFrom <= paystub.PayFromdate).
                Where(Function(s) paystub.PayFromdate <= If(s.EffectiveTo, paystub.PayFromdate))

            Return Await query.FirstOrDefaultAsync()
        End Function

        Public Async Function GetTimeEntries(employeeID As Integer?, dateFrom As Date, dateTo As Date) As Task(Of IList(Of TimeEntry))
            Dim query = _context.TimeEntries.
                Where(Function(t) dateFrom <= t.Date And t.Date <= dateTo).
                Where(Function(t) Nullable.Equals(t.EmployeeID, employeeID))

            Return Await query.ToListAsync()
        End Function

        Public Async Function GetAllowanceItems(paystubID As Integer?) As Task(Of ICollection(Of AllowanceItem))
            Dim query = _context.Paystubs.
                Include(Function(p) p.AllowanceItems).
                    ThenInclude(Function(a) a.Allowance).
                        ThenInclude(Function(p) p.Product).
                Where(Function(p) Nullable.Equals(p.RowID, paystubID))

            Dim paystub = Await query.SingleOrDefaultAsync()

            Return paystub.AllowanceItems
        End Function

        Public Async Function GetLoanTransactions(payperiodID As Integer?, employeeID As Integer?) As Task(Of IList(Of LoanTransaction))
            Dim query = _context.LoanTransactions.
                Include(Function(l) l.LoanSchedule).
                    ThenInclude(Function(s) s.LoanType).
                Where(Function(l) Nullable.Equals(l.PayPeriodID, payperiodID)).
                Where(Function(l) Nullable.Equals(l.EmployeeID, employeeID))

            Return Await query.ToListAsync()
        End Function

        Public Async Function GetAdjustments(paystub As Paystub) As Task(Of IList(Of Adjustment))
            Dim query = _context.Adjustments.
                Include(Function(a) a.Product).
                Where(Function(t) Nullable.Equals(t.PaystubID, paystub.RowID))

            Return Await query.ToListAsync()
        End Function

        Public Async Function GetAdjustmentTypes() As Task(Of IList(Of Product))
            Dim query = _context.Products.
                Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                Where(Function(p) p.Category = "Adjustment Type")

            Return Await query.ToListAsync()
        End Function

        Public Async Function GetPayperiods() As Task(Of IList(Of PayPeriod))
            Dim payPeriods As IList(Of PayPeriod) = Nothing

            Dim query =
                (From p In _context.PayPeriods
                 Join ps In _context.Paystubs On p.RowID Equals ps.PayPeriodID
                 Where p.OrganizationID = z_OrganizationID
                 Group By p.RowID Into x = Group
                 Select x.Distinct().FirstOrDefault().p)

            Return Await query.ToListAsync()
        End Function

        Public Async Function GetPaystubs(payperiod As PayPeriod) As Task(Of IList(Of Paystub))
            Dim paystubs As IList(Of Paystub) = Nothing

            Dim query = _context.Paystubs.Include(Function(p) p.Employee).
                 Where(Function(p) p.PayFromdate = payperiod.PayFromDate).
                 Where(Function(p) p.PayToDate = payperiod.PayToDate).
                 Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                 OrderBy(Function(p) p.Employee.LastName).
                 ThenBy(Function(p) p.Employee.FirstName)

            Return Await query.ToListAsync()
        End Function

    End Class

End Class