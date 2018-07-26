Option Strict On

Imports System.Data.Entity
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports NHibernate.Linq
Imports PayrollSys

''' <summary>
''' Takes care of loading all the information needed to produce the payroll for a given pay period.
''' </summary>
Public Class PayrollResources

    Private _payPeriodID As Integer?

    Private _payDateFrom As Date

    Private _payDateTo As Date

    Private _employees As ICollection(Of Employee)

    Private _salaries As DataTable

    Private _salaries2 As ICollection(Of Salary)

    Private _timeEntries As ICollection(Of TimeEntry)

    Private _actualtimeentries As ICollection(Of ActualTimeEntry)

    Private _loanSchedules As ICollection(Of LoanSchedule)

    Private _loanTransactions As ICollection(Of LoanTransaction)

    Private _products As ICollection(Of Product)

    Private _socialSecurityBrackets As ICollection(Of SocialSecurityBracket)

    Private _philHealthBrackets As ICollection(Of PhilHealthBracket)

    Private _withholdingTaxBrackets As ICollection(Of WithholdingTaxBracket)

    Private _listOfValues As ICollection(Of ListOfValue)

    Private _paystubs As ICollection(Of Paystub)

    Private _previousPaystubs As ICollection(Of Paystub)

    Private _isEndOfMonth As Boolean

    Private _payPeriod As PayPeriod

    Private _payRates As ICollection(Of PayRate)

    Private _allowances As ICollection(Of Allowance)

    Public ReadOnly Property Employees As ICollection(Of Employee)
        Get
            Return _employees
        End Get
    End Property

    Public ReadOnly Property TimeEntries As ICollection(Of TimeEntry)
        Get
            Return _timeEntries
        End Get
    End Property

    Public ReadOnly Property LoanSchedules As ICollection(Of LoanSchedule)
        Get
            Return _loanSchedules
        End Get
    End Property

    Public ReadOnly Property LoanTransactions As ICollection(Of LoanTransaction)
        Get
            Return _loanTransactions
        End Get
    End Property

    Public ReadOnly Property Salaries As DataTable
        Get
            Return _salaries
        End Get
    End Property

    Public ReadOnly Property Salaries2 As ICollection(Of Salary)
        Get
            Return _salaries2
        End Get
    End Property

    Public ReadOnly Property Products As ICollection(Of Product)
        Get
            Return _products
        End Get
    End Property

    Public ReadOnly Property Paystubs As ICollection(Of Paystub)
        Get
            Return _paystubs
        End Get
    End Property

    Public ReadOnly Property PreviousPaystubs As ICollection(Of Paystub)
        Get
            Return _previousPaystubs
        End Get
    End Property

    Public ReadOnly Property SocialSecurityBrackets As ICollection(Of SocialSecurityBracket)
        Get
            Return _socialSecurityBrackets
        End Get
    End Property

    Public ReadOnly Property PhilHealthBrackets As ICollection(Of PhilHealthBracket)
        Get
            Return _philHealthBrackets
        End Get
    End Property

    Public ReadOnly Property WithholdingTaxBrackets As ICollection(Of WithholdingTaxBracket)
        Get
            Return _withholdingTaxBrackets
        End Get
    End Property

    Public ReadOnly Property ListOfValues As ICollection(Of ListOfValue)
        Get
            Return _listOfValues
        End Get
    End Property

    Public ReadOnly Property PayPeriod As PayPeriod
        Get
            Return _payPeriod
        End Get
    End Property

    Public ReadOnly Property PayRates As ICollection(Of PayRate)
        Get
            Return _payRates
        End Get
    End Property

    Public ReadOnly Property Allowances As ICollection(Of Allowance)
        Get
            Return _allowances
        End Get
    End Property

    Public ReadOnly Property ActualTimeEntries As ICollection(Of ActualTimeEntry)
        Get
            Return _actualtimeentries
        End Get
    End Property

    Public Sub New(payPeriodID As String, payDateFrom As Date, payDateTo As Date)
        _payPeriodID = Integer.Parse(payPeriodID)
        _payDateFrom = payDateFrom
        _payDateTo = payDateTo
    End Sub

    Public Async Function Load() As Task
        Dim loadSalariesTask = LoadSalaries()
        Dim loadLoanSchedulesTask = LoadLoanSchedules()
        Dim loadLoanTransactionsTask = LoadLoanTransactions()

        Await Task.WhenAll({
            LoadEmployees(),
            loadLoanSchedulesTask,
            loadLoanTransactionsTask,
            LoadSalaries2(),
            loadSalariesTask,
            LoadPaystubs(),
            LoadProducts(),
            LoadPreviousPaystubs(),
            LoadSocialSecurityBrackets(),
            LoadPhilHealthBrackets(),
            LoadWithholdingTaxBrackets(),
            LoadSettings(),
            LoadPayPeriod(),
            LoadPayRates(),
            LoadAllowances(),
            LoadTimeEntries(),
            LoadActualTimeEntries()
        })
    End Function

    Public Async Function LoadEmployees() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From e In context.Employees
                            Where e.OrganizationID = z_OrganizationID And
                                e.EmploymentStatus <> "Resigned" And
                                e.EmploymentStatus <> "Terminated"

                _employees = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Employees", ex)
        End Try
    End Function

    Private Async Function LoadTimeEntries() As Task
        Dim backDate = _payDateFrom.AddDays(-3)

        Try
            Using context = New PayrollContext()
                Dim query = From t In context.TimeEntries.Include(Function(t) t.ShiftSchedule.Shift)
                            Where t.OrganizationID = z_OrganizationID And
                                backDate <= t.Date And
                                t.Date <= _payDateTo
                            Select t

                _timeEntries = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("TimeEntries", ex)
        End Try
    End Function

    Private Async Function LoadActualTimeEntries() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From t In context.ActualTimeEntries.Include(Function(t) t.ShiftSchedule.Shift)
                            Where t.OrganizationID = z_OrganizationID And
                                _payDateFrom <= t.Date And
                                t.Date <= _payDateTo
                            Select t

                _actualtimeentries = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("ActualTimeEntries", ex)
        End Try
    End Function

    Private Async Function LoadLoanSchedules() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From l In context.LoanSchedules
                            Select l
                            Where l.OrganizationID = z_OrganizationID And
                                                          l.DedEffectiveDateFrom <= _payDateTo And
                                                          l.Status = "In Progress" And
                                                          l.BonusID Is Nothing
                _loanSchedules = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("LoanSchedules", ex)
        End Try
    End Function

    Private Async Function LoadLoanTransactions() As Task
        Try
            Using context = New PayrollContext()
                Dim query = context.LoanTransactions.
                    Where(Function(l) CBool(l.OrganizationID = z_OrganizationID)).
                    Where(Function(l) CBool(l.PayPeriodID = _payPeriodID))

                _loanTransactions = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("LoanTransactions", ex)
        End Try
    End Function

    Private Async Function LoadSalaries() As Task
        Try
            Dim query = New SqlToDataTable($"
                SELECT
                    *,
                    COALESCE((SELECT EmployeeShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployeeShare',
                    COALESCE((SELECT EmployerShare FROM payphilhealth WHERE RowID=employeesalary.PayPhilhealthID),0) 'EmployerShare',
                    COALESCE((SELECT EmployeeContributionAmount FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployeeContributionAmount',
                    COALESCE((SELECT (EmployerContributionAmount + EmployeeECAmount) FROM paysocialsecurity WHERE RowID=employeesalary.PaySocialSecurityID),0) 'EmployerContributionAmount'
                FROM employeesalary
                WHERE OrganizationID = {orgztnID} AND
                    (EffectiveDateFrom >= '{_payDateFrom.ToString("s")}' OR IFNULL(EffectiveDateTo,CURDATE()) >= '{_payDateFrom.ToString("s")}') AND
                    (EffectiveDateFrom <= '{_payDateTo.ToString("s")}' OR IFNULL(EffectiveDateTo,CURDATE()) <= '{_payDateTo.ToString("s")}')
                GROUP BY EmployeeID
                ORDER BY DATEDIFF(CURDATE(), EffectiveDateFrom);
            ")

            _salaries = Await query.ReadAsync()
        Catch ex As Exception
            Throw New ResourceLoadingException("Salaries", ex)
        End Try
    End Function

    Private Async Function LoadSalaries2() As Task
        Dim today = DateTime.Today

        Try
            Using context = New PayrollContext()
                Dim query = context.Salaries.
                    Where(Function(s) s.EffectiveFrom >= _payDateFrom Or If(s.EffectiveTo, today) >= _payDateFrom).
                    Where(Function(s) s.EffectiveFrom <= _payDateTo Or If(s.EffectiveTo, today) <= _payDateTo).
                    GroupBy(Function(s) s.EmployeeID).
                    Select(Function(g) g.FirstOrDefault())

                _salaries2 = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Salaries", ex)
        End Try
    End Function

    Private Async Function LoadProducts() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From p In context.Products
                            Where p.OrganizationID = z_OrganizationID
                _products = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Products", ex)
        End Try
    End Function

    Private Async Function LoadPaystubs() As Task
        Try
            Using context = New PayrollContext()
                Dim query = context.Paystubs.Include(Function(p) p.Adjustments).
                    Where(Function(p) p.PayFromdate = _payDateFrom).
                    Where(Function(p) p.PayToDate = _payDateTo).
                    Where(Function(p) CBool(p.OrganizationID = z_OrganizationID))

                _paystubs = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Paystubs", ex)
        End Try
    End Function

    Private Async Function LoadPreviousPaystubs() As Task
        Dim previousCutoffEnd = _payDateFrom.AddDays(-1)

        Try
            Using context = New PayrollContext()
                Dim query = context.Paystubs.
                    Where(Function(p) p.PayToDate = previousCutoffEnd).
                    Where(Function(p) CBool(p.OrganizationID = z_OrganizationID))

                _previousPaystubs = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("PreviousPaystubs", ex)
        End Try
    End Function

    Private Async Function LoadSocialSecurityBrackets() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From s In context.SocialSecurityBrackets

                _socialSecurityBrackets = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("SocialSecurityBrackets", ex)
        End Try
    End Function

    Private Async Function LoadPhilHealthBrackets() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From p In context.PhilHealthBrackets

                _philHealthBrackets = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("PhilHealthBrackets", ex)
        End Try
    End Function

    Private Async Function LoadWithholdingTaxBrackets() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From w In context.WithholdingTaxBrackets
                            Select w

                _withholdingTaxBrackets = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("WithholdingTaxBrackets", ex)
        End Try
    End Function

    Private Async Function LoadSettings() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From s In context.ListOfValues

                _listOfValues = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("ListOfValues", ex)
        End Try
    End Function

    Private Async Function LoadPayPeriod() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From p In context.PayPeriods
                            Where p.RowID = _payPeriodID

                _payPeriod = Await query.FirstOrDefaultAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("PayPeriod", ex)
        End Try
    End Function

    Private Async Function LoadPayRates() As Task
        Dim cutoffStart = _payDateFrom.AddDays(-3)

        Try
            Using context = New PayrollContext()
                Dim query = From p In context.PayRates
                            Where p.OrganizationID = z_OrganizationID And
                               cutoffStart <= p.RateDate And
                               p.RateDate <= _payDateTo

                _payRates = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("PayRates", ex)
        End Try
    End Function

    Private Async Function LoadAllowances() As Task
        Try
            Using context = New PayrollContext()
                ' Retrieve all allowances whose begin and end date spans the cutoff dates. Or in the case of
                ' one time-allowances, allowances whose start date is between the cutoff dates.
                Dim query = From a In context.Allowances.Include(Function(a) a.Product)
                            Where (
                                (
                                    a.EffectiveStartDate <= _payDateTo And
                                    _payDateFrom <= a.EffectiveEndDate
                                ) Or
                                (
                                    _payDateFrom <= a.EffectiveStartDate And
                                    a.EffectiveStartDate <= _payDateTo And
                                    a.EffectiveEndDate Is Nothing
                                )
                            ) And
                            a.OrganizationID = z_OrganizationID

                _allowances = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Allowances", ex)
        End Try
    End Function

End Class

Public Class ResourceLoadingException
    Inherits Exception

    Public Sub New(resource As String, ex As Exception)
        MyBase.New($"Failure to load resource `{resource}`", ex)
    End Sub

End Class
