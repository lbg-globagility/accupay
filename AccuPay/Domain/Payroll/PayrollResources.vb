Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Repository
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Logging.Console
Imports PayrollSys

''' <summary>
''' Takes care of loading all the information needed to produce the payroll for a given pay period.
''' </summary>
Public Class PayrollResources

    Dim logger As LoggerFactory = New LoggerFactory(
        {
            New ConsoleLoggerProvider(Function(__, logLevel) logLevel = LogLevel.Information, True)
        })

    Private _payPeriodID As Integer?

    Private _payDateFrom As Date

    Private _payDateTo As Date

    Private _employees As ICollection(Of Employee)

    Private _salaries As ICollection(Of Salary)

    Private _timeEntries As ICollection(Of TimeEntry)

    Private _employeeDutySchedules As ICollection(Of EmployeeDutySchedule)

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

    Private _allowances As ICollection(Of Allowance)

    Private _divisionMinimumWages As ICollection(Of DivisionMinimumWage)

    Private _filingStatuses As DataTable

    Private _systemOwner As SystemOwner

    Private _bpiInsuranceProduct As Product

    Private _calendarCollection As CalendarCollection

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

    Public ReadOnly Property EmployeeDutySchedule As ICollection(Of EmployeeDutySchedule)
        Get
            Return _employeeDutySchedules
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

    Public ReadOnly Property Salaries As ICollection(Of Salary)
        Get
            Return _salaries
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

    Public ReadOnly Property FilingStatuses As DataTable
        Get
            Return _filingStatuses
        End Get
    End Property

    Public ReadOnly Property DivisionMinimumWages As ICollection(Of DivisionMinimumWage)
        Get
            Return _divisionMinimumWages
        End Get
    End Property

    Public ReadOnly Property SystemOwner As SystemOwner
        Get
            Return _systemOwner
        End Get
    End Property

    Public ReadOnly Property CalendarCollection As CalendarCollection
        Get
            Return _calendarCollection
        End Get
    End Property

    Public ReadOnly Property BpiInsuranceProduct As Product
        Get
            Return _bpiInsuranceProduct
        End Get
    End Property

    Public Sub New(payPeriodID As Integer, payDateFrom As Date, payDateTo As Date)
        _payPeriodID = payPeriodID
        _payDateFrom = payDateFrom
        _payDateTo = payDateTo
    End Sub

    Public Async Function Load() As Task

        'LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()

        Await LoadPayPeriod()

        Await Task.WhenAll({
            LoadSystemOwner(),
            LoadEmployees(),
            LoadLoanSchedules(),
            LoadLoanTransactions(),
            LoadSalaries(),
            LoadPaystubs(),
            LoadProducts(),
            LoadPreviousPaystubs(),
            LoadSocialSecurityBrackets(),
            LoadPhilHealthBrackets(),
            LoadWithholdingTaxBrackets(),
            LoadSettings(),
            LoadAllowances(),
            LoadTimeEntries(),
            LoadActualTimeEntries(),
            LoadFilingStatuses(),
            LoadDivisionMinimumWages(),
            LoadEmployeeDutySchedules(),
            LoadCalendarCollection(),
            LoadBpiInsuranceProduct()
        })
    End Function

    Public Async Function LoadSystemOwner() As Task
        Await Task.Run(Sub()
                           _systemOwner = New SystemOwner
                       End Sub)
    End Function

    Public Async Function LoadEmployees() As Task
        Try
            Using context = New PayrollContext()
                Dim query = context.Employees.
                    Include(Function(e) e.Position.Division).
                    Where(Function(e) e.OrganizationID.Value = z_OrganizationID).
                    Where(Function(e) e.IsActive)

                _employees = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Employees", ex)
        End Try
    End Function

    Private Async Function LoadTimeEntries() As Task
        Dim threeDaysBeforeCutoff = GetPreviousThreeDaysBeforeCutoff()

        Try
            Using context = New PayrollContext()
                Dim query = From t In context.TimeEntries
                            Where t.OrganizationID.Value = z_OrganizationID AndAlso
                                threeDaysBeforeCutoff <= t.Date AndAlso
                                t.Date <= _payDateTo
                            Select t

                _timeEntries = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("TimeEntries", ex)
        End Try
    End Function

    Private Async Function LoadEmployeeDutySchedules() As Task
        Dim threeDaysBeforeCutoff = GetPreviousThreeDaysBeforeCutoff()

        Try
            Using context = New PayrollContext(logger)
                Dim query = From e In context.EmployeeDutySchedules
                            Where e.OrganizationID.Value = z_OrganizationID AndAlso
                                e.DateSched >= threeDaysBeforeCutoff AndAlso
                                e.DateSched <= _payDateTo

                _employeeDutySchedules = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("EmployeeDutySchedules", ex)
        End Try
    End Function

    Private Async Function LoadCalendarCollection() As Task
        Dim threeDaysBeforeCutoff = GetPreviousThreeDaysBeforeCutoff()

        Try
            Await Task.Run(
               Sub()
                   Using context = New PayrollContext(logger)

                       Dim settings = New ListOfValueCollection(_listOfValues)
                       Dim calculationBasis = settings.GetEnum("Pay rate.CalculationBasis",
                                                                PayRateCalculationBasis.Organization)

                       If calculationBasis = PayRateCalculationBasis.Branch Then
                           Dim branches = context.Branches.ToList()

                           Dim calendarDays = context.CalendarDays.
                                        Include(Function(t) t.DayType).
                                        Where(Function(t) threeDaysBeforeCutoff <= t.Date AndAlso t.Date <= _payDateTo).
                                        ToList()

                           _calendarCollection = New CalendarCollection(branches, calendarDays)
                       Else
                           Dim payrates = context.PayRates.
                                        Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                        Where(Function(p) threeDaysBeforeCutoff <= p.Date AndAlso p.Date <= _payDateTo).
                                        ToList()

                           _calendarCollection = New CalendarCollection(payrates)
                       End If

                   End Using
               End Sub)
        Catch ex As Exception
            Throw New ResourceLoadingException("EmployeeDutySchedules", ex)
        End Try
    End Function

    Private Async Function LoadActualTimeEntries() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From t In context.ActualTimeEntries.Include(Function(t) t.ShiftSchedule.Shift)
                            Where t.OrganizationID.Value = z_OrganizationID AndAlso
                                _payDateFrom <= t.Date AndAlso
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
            Using context = New PayrollContext(logger)
                Dim query = From l In context.LoanSchedules
                            Select l
                            Where l.OrganizationID.Value = z_OrganizationID AndAlso
                                l.DedEffectiveDateFrom <= _payDateTo AndAlso
                                l.Status = "In Progress" AndAlso
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
                    Where(Function(l) l.OrganizationID.Value = z_OrganizationID).
                    Where(Function(l) CBool(l.PayPeriodID = _payPeriodID))

                _loanTransactions = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("LoanTransactions", ex)
        End Try
    End Function

    Private Async Function LoadSalaries() As Task
        Dim today = DateTime.Today

        Try
            Using context = New PayrollContext(logger)
                Dim query = context.Salaries.
                    Where(Function(s) s.OrganizationID.Value = z_OrganizationID).
                    Where(Function(s) s.EffectiveFrom <= _payDateFrom).
                    OrderByDescending(Function(s) s.EffectiveFrom).
                    GroupBy(Function(s) s.EmployeeID).
                    Select(Function(g) g.FirstOrDefault())

                _salaries = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Salaries", ex)
        End Try
    End Function

    Private Async Function LoadProducts() As Task
        Try
            Using context = New PayrollContext()
                Dim query = From p In context.Products
                            Where p.OrganizationID.Value = z_OrganizationID

                _products = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Products", ex)
        End Try
    End Function

    Private Async Function LoadPaystubs() As Task
        Try
            Using context = New PayrollContext()
                Dim query = context.Paystubs.
                    Include(Function(p) p.Adjustments).
                    ThenInclude(Function(a) a.Product).
                    Include(Function(p) p.ThirteenthMonthPay).
                    Include(Function(p) p.ActualAdjustments).
                    Include(Function(p) p.Actual).
                    Where(Function(p) p.PayFromdate = _payDateFrom).
                    Where(Function(p) p.PayToDate = _payDateTo).
                    Where(Function(p) p.OrganizationID.Value = z_OrganizationID)

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
                    Where(Function(p) p.OrganizationID.Value = z_OrganizationID)

                _previousPaystubs = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("PreviousPaystubs", ex)
        End Try
    End Function

    Private Async Function LoadSocialSecurityBrackets() As Task

        Try
            'LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
            Dim taxEffectivityDate = New Date(_payPeriod.Year, _payPeriod.Month, 1)

            Using context = New PayrollContext()
                Dim query = context.SocialSecurityBrackets.
                            Where(Function(s) taxEffectivityDate >= s.EffectiveDateFrom).
                            Where(Function(s) taxEffectivityDate <= s.EffectiveDateTo)

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
                            Select p

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
            Using context = New PayrollContext(logger)
                Dim query = From p In context.PayPeriods
                            Where Nullable.Equals(p.RowID, _payPeriodID)

                _payPeriod = Await query.FirstOrDefaultAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("PayPeriod", ex)
        End Try
    End Function

    Private Async Function LoadAllowances() As Task
        Try
            Using context = New PayrollContext(logger)
                Dim query As IQueryable(Of Allowance) = New AllowanceRepository().
                                                GetAllowancesWithPayPeriodBaseQuery(context,
                                                                       _payDateFrom:=_payDateFrom,
                                                                       _payDateTo:=_payDateTo)

                _allowances = Await query.ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Allowances", ex)
        End Try
    End Function

    Private Async Function LoadFilingStatuses() As Task
        Try
            Using context = New PayrollContext()
                _filingStatuses = Await (
                    New SqlToDataTable("SELECT * FROM filingstatus;").
                        ReadAsync())
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("Filing Statuses", ex)
        End Try
    End Function

    Private Async Function LoadDivisionMinimumWages() As Task
        Try
            Using context = New PayrollContext()
                _divisionMinimumWages = Await context.DivisionMinimumWages.
                    Where(Function(t) t.OrganizationID.Value = z_OrganizationID).
                    Where(Function(t) t.EffectiveDateFrom <= _payDateTo AndAlso _payDateTo <= t.EffectiveDateTo).
                    ToListAsync()
            End Using
        Catch ex As Exception
            Throw New ResourceLoadingException("DivisionMinimumWage", ex)
        End Try
    End Function

    Private Function GetPreviousThreeDaysBeforeCutoff() As Date
        Return _payDateFrom.AddDays(-3)
    End Function

    Private Async Function LoadBpiInsuranceProduct() As Task
        Try

            _bpiInsuranceProduct = Await New ProductRepository().
                                        GetOrCreateAdjustmentType(ProductConstant.BPI_INSURANCE_ADJUSTMENT)
        Catch ex As Exception
            Throw New ResourceLoadingException("BPI Insurance Adjustment Product ID", ex)
        End Try
    End Function

End Class

Public Class ResourceLoadingException
    Inherits Exception

    Public Sub New(resource As String, ex As Exception)
        MyBase.New($"Failure to load resource `{resource}`", ex)
    End Sub

End Class
