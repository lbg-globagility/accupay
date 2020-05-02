Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

''' <summary>
''' Takes care of loading all the information needed to produce the payroll for a given pay period.
''' </summary>
Public Class PayrollResources

    Private _payPeriodId As Integer?

    Private _payDateFrom As Date

    Private _payDateTo As Date

    Private _payPeriodSpan As TimePeriod

    Private _employees As ICollection(Of Entities.Employee)

    Private _salaries As ICollection(Of Entities.Salary)

    Private _timeEntries As ICollection(Of Entities.TimeEntry)

    Private _actualtimeentries As ICollection(Of Entities.ActualTimeEntry)

    Private _loanSchedules As ICollection(Of Entities.LoanSchedule)

    Private _loanTransactions As ICollection(Of Entities.LoanTransaction)

    Private _socialSecurityBrackets As IReadOnlyCollection(Of Entities.SocialSecurityBracket)

    Private _philHealthBrackets As IReadOnlyCollection(Of Entities.PhilHealthBracket)

    Private _withholdingTaxBrackets As IReadOnlyCollection(Of Entities.WithholdingTaxBracket)

    Private _paystubs As ICollection(Of Paystub)

    Private _previousPaystubs As ICollection(Of Entities.Paystub)

    Private _payPeriod As Entities.PayPeriod

    Private _allowances As ICollection(Of Entities.Allowance)

    Private _divisionMinimumWages As IReadOnlyCollection(Of Entities.DivisionMinimumWage)

    Private _filingStatuses As IReadOnlyCollection(Of Entities.FilingStatusType)

    Private _systemOwner As SystemOwnerService

    Private _bpiInsuranceProduct As Entities.Product

    Private _sickLeaveProduct As Entities.Product

    Private _vacationLeaveProduct As Entities.Product

    Private _calendarCollection As CalendarCollection

    Private _listOfValueCollection As ListOfValueCollection

    Private _leaves As IReadOnlyCollection(Of Entities.Leave)

    Public ReadOnly Property ListOfValueCollection As ListOfValueCollection
        Get
            Return _listOfValueCollection
        End Get
    End Property

    Public ReadOnly Property Employees As ICollection(Of Entities.Employee)
        Get
            Return _employees
        End Get
    End Property

    Public ReadOnly Property TimeEntries As ICollection(Of Entities.TimeEntry)
        Get
            Return _timeEntries
        End Get
    End Property

    Public ReadOnly Property LoanSchedules As ICollection(Of Entities.LoanSchedule)
        Get
            Return _loanSchedules
        End Get
    End Property

    Public ReadOnly Property LoanTransactions As ICollection(Of Entities.LoanTransaction)
        Get
            Return _loanTransactions
        End Get
    End Property

    Public ReadOnly Property Salaries As ICollection(Of Entities.Salary)
        Get
            Return _salaries
        End Get
    End Property

    Public ReadOnly Property Paystubs As ICollection(Of Paystub)
        Get
            Return _paystubs
        End Get
    End Property

    Public ReadOnly Property PreviousPaystubs As ICollection(Of Entities.Paystub)
        Get
            Return _previousPaystubs
        End Get
    End Property

    Public ReadOnly Property SocialSecurityBrackets As IReadOnlyCollection(Of Entities.SocialSecurityBracket)
        Get
            Return _socialSecurityBrackets
        End Get
    End Property

    Public ReadOnly Property PhilHealthBrackets As IReadOnlyCollection(Of Entities.PhilHealthBracket)
        Get
            Return _philHealthBrackets
        End Get
    End Property

    Public ReadOnly Property WithholdingTaxBrackets As IReadOnlyCollection(Of Entities.WithholdingTaxBracket)
        Get
            Return _withholdingTaxBrackets
        End Get
    End Property

    Public ReadOnly Property PayPeriod As Entities.PayPeriod
        Get
            Return _payPeriod
        End Get
    End Property

    Public ReadOnly Property Allowances As ICollection(Of Entities.Allowance)
        Get
            Return _allowances
        End Get
    End Property

    Public ReadOnly Property ActualTimeEntries As ICollection(Of Entities.ActualTimeEntry)
        Get
            Return _actualtimeentries
        End Get
    End Property

    Public ReadOnly Property Leaves As IReadOnlyCollection(Of Entities.Leave)
        Get
            Return _leaves
        End Get
    End Property

    Public ReadOnly Property FilingStatuses As IReadOnlyCollection(Of Entities.FilingStatusType)
        Get
            Return _filingStatuses
        End Get
    End Property

    Public ReadOnly Property DivisionMinimumWages As IReadOnlyCollection(Of Entities.DivisionMinimumWage)
        Get
            Return _divisionMinimumWages
        End Get
    End Property

    Public ReadOnly Property SystemOwner As SystemOwnerService
        Get
            Return _systemOwner
        End Get
    End Property

    Public ReadOnly Property CalendarCollection As CalendarCollection
        Get
            Return _calendarCollection
        End Get
    End Property

    Public ReadOnly Property BpiInsuranceProduct As Entities.Product
        Get
            Return _bpiInsuranceProduct
        End Get
    End Property

    Public ReadOnly Property SickLeaveProduct As Entities.Product
        Get
            Return _sickLeaveProduct
        End Get
    End Property

    Public ReadOnly Property VacationLeaveProduct As Entities.Product
        Get
            Return _vacationLeaveProduct
        End Get
    End Property

    Public Sub New(payPeriodId As Integer, payDateFrom As Date, payDateTo As Date)
        _payPeriodId = payPeriodId
        _payDateFrom = payDateFrom
        _payDateTo = payDateTo

        _payPeriodSpan = New TimePeriod(_payDateFrom, _payDateTo)
    End Sub

    Public Async Function Load() As Task

        'LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
        Await LoadPayPeriod()

        'LoadSettings() should be executed before LoadCalendarCollection()
        Await LoadListOfValueCollection()

        Await Task.WhenAll({
            LoadSystemOwner(),
            LoadEmployees(),
            LoadLoanSchedules(),
            LoadLoanTransactions(),
            LoadSalaries(),
            LoadPaystubs(),
            LoadPreviousPaystubs(),
            LoadSocialSecurityBrackets(),
            LoadPhilHealthBrackets(),
            LoadWithholdingTaxBrackets(),
            LoadAllowances(),
            LoadTimeEntries(),
            LoadActualTimeEntries(),
            LoadFilingStatuses(),
            LoadDivisionMinimumWages(),
            LoadCalendarCollection(),
            LoadBpiInsuranceProduct(),
            LoadSickLeaveProduct(),
            LoadVacationLeaveProduct(),
            LoadLeaves()
        })
    End Function

    ''' <summary>
    ''' This is the current paystubs that will be updated when the payroll is generated.
    ''' On the first generation or when payroll is deleted, this list will be empty.
    ''' </summary>
    ''' <returns></returns>
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

    Public Async Function LoadListOfValueCollection() As Task
        Try

            _listOfValueCollection = Await ListOfValueCollection.CreateAsync()
        Catch ex As Exception
            Throw New ResourceLoadingException("ListOfValueCollection", ex)

        End Try
    End Function

    Public Async Function LoadSystemOwner() As Task
        Try
            Await Task.Run(Sub()
                               _systemOwner = New SystemOwnerService
                           End Sub)
        Catch ex As Exception
            Throw New ResourceLoadingException("SystemOwner", ex)

        End Try
    End Function

    Public Async Function LoadEmployees() As Task
        Try
            _employees = (Await New EmployeeRepository().
                                    GetAllActiveWithDivisionAndPositionAsync(z_OrganizationID)).ToList
        Catch ex As Exception
            Throw New ResourceLoadingException("Employees", ex)
        End Try
    End Function

    Private Async Function LoadTimeEntries() As Task
        Dim previousCutoffDateForCheckingLastWorkingDay = Data.Helpers.PayrollTools.
                                        GetPreviousCutoffDateForCheckingLastWorkingDay(_payDateFrom)

        Try

            Dim datePeriod = New TimePeriod(previousCutoffDateForCheckingLastWorkingDay, _payDateTo)
            _timeEntries = (Await New TimeEntryRepository().
                                GetByDatePeriodAsync(z_OrganizationID, datePeriod)).
                                ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("TimeEntries", ex)
        End Try
    End Function

    Private Async Function LoadCalendarCollection() As Task
        Dim previousCutoffDateForCheckingLastWorkingDay = Data.Helpers.PayrollTools.
                            GetPreviousCutoffDateForCheckingLastWorkingDay(_payDateFrom)

        Try
            Await Task.Run(
                Sub()
                    Dim calculationBasis = _listOfValueCollection.GetEnum("Pay rate.CalculationBasis",
                                                              Data.Enums.PayRateCalculationBasis.Organization)

                    Dim payPeriod = New TimePeriod(previousCutoffDateForCheckingLastWorkingDay, _payDateTo)

                    _calendarCollection = Data.Helpers.PayrollTools.
                                               GetCalendarCollection(payPeriod,
                                                                   calculationBasis,
                                                                   z_OrganizationID)

                End Sub)
        Catch ex As Exception
            Throw New ResourceLoadingException("EmployeeDutySchedules", ex)
        End Try
    End Function

    Private Async Function LoadActualTimeEntries() As Task
        Try
            Dim datePeriod = New TimePeriod(_payDateFrom, _payDateTo)
            _actualtimeentries = (Await New ActualTimeEntryRepository().
                                GetByDatePeriodAsync(z_OrganizationID, datePeriod)).
                                ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("ActualTimeEntries", ex)
        End Try
    End Function

    Private Async Function LoadLoanSchedules() As Task
        Try
            _loanSchedules = (Await New LoanScheduleRepository().
                                GetCurrentPayrollLoansAsync(z_OrganizationID, _payDateTo)).
                                ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("LoanSchedules", ex)
        End Try
    End Function

    Private Async Function LoadLoanTransactions() As Task
        Try

            If _payPeriodId.HasValue = False Then Return

            _loanTransactions = (Await New LoanTransactionRepository().
                            GetByPayPeriodWithEmployeeAsync(_payPeriodId.Value)).
                            ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("LoanTransactions", ex)
        End Try
    End Function

    Private Async Function LoadSalaries() As Task
        Try
            _salaries = (Await New SalaryRepository().
                    GetByCutOffAsync(z_OrganizationID, _payDateFrom)).
                    ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("Salaries", ex)
        End Try
    End Function

    Private Async Function LoadPreviousPaystubs() As Task
        Try
            _previousPaystubs = (Await New PaystubRepository().
                                GetPreviousCutOffPaystubs(_payDateFrom, z_OrganizationID)).
                                ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("PreviousPaystubs", ex)
        End Try
    End Function

    Private Async Function LoadSocialSecurityBrackets() As Task

        Try
            'LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
            Dim taxEffectivityDate = New Date(_payPeriod.Year, _payPeriod.Month, 1)

            _socialSecurityBrackets = (Await New SocialSecurityBracketRepository().
                                              GetByTimePeriodAsync(taxEffectivityDate)).ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("SocialSecurityBrackets", ex)
        End Try
    End Function

    Private Async Function LoadPhilHealthBrackets() As Task
        Try

            _philHealthBrackets = (Await New PhilHealthBracketRepository().GetAllAsync()).ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("PhilHealthBrackets", ex)
        End Try
    End Function

    Private Async Function LoadWithholdingTaxBrackets() As Task
        Try
            _withholdingTaxBrackets = (Await New WithholdingTaxBracketRepository().GetAllAsync).ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("WithholdingTaxBrackets", ex)
        End Try
    End Function

    Private Async Function LoadPayPeriod() As Task
        Try
            If _payPeriodId.HasValue = False Then Return

            _payPeriod = (Await New PayPeriodRepository().GetByIdAsync(_payPeriodId.Value))
        Catch ex As Exception
            Throw New ResourceLoadingException("PayPeriod", ex)
        End Try
    End Function

    Private Async Function LoadAllowances() As Task
        Try
            Dim allowanceRepo = New AllowanceRepository()

            _allowances = Await (allowanceRepo.
                            GetByPayPeriodWithProductAsync(organizationId:=z_OrganizationID,
                                                      timePeriod:=_payPeriodSpan))
        Catch ex As Exception
            Throw New ResourceLoadingException("Allowances", ex)
        End Try
    End Function

    Private Async Function LoadFilingStatuses() As Task
        Try
            _filingStatuses = (Await New FilingStatusTypeRepository().GetAllAsync()).
                                ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("Filing Statuses", ex)
        End Try
    End Function

    Private Async Function LoadDivisionMinimumWages() As Task
        Try
            _divisionMinimumWages = (Await New DivisionMinimumWageRepository().
                                                GetByDateAsync(z_OrganizationID, _payDateTo)).ToList()
        Catch ex As Exception
            Throw New ResourceLoadingException("DivisionMinimumWage", ex)
        End Try
    End Function

    Private Async Function LoadBpiInsuranceProduct() As Task
        Try

            _bpiInsuranceProduct = Await New ProductRepository().
                                        GetOrCreateAdjustmentTypeAsync(
                                            ProductConstant.BPI_INSURANCE_ADJUSTMENT,
                                            organizationId:=z_OrganizationID,
                                            userId:=z_User)
        Catch ex As Exception
            Throw New ResourceLoadingException("BPI Insurance Adjustment Product", ex)
        End Try
    End Function

    Private Async Function LoadSickLeaveProduct() As Task
        Try

            _sickLeaveProduct = Await New ProductRepository().
                                        GetOrCreateLeaveTypeAsync(
                                            ProductConstant.SICK_LEAVE,
                                            organizationId:=z_OrganizationID,
                                            userId:=z_User)
        Catch ex As Exception
            Throw New ResourceLoadingException("Sick Leave Product", ex)
        End Try
    End Function

    Private Async Function LoadVacationLeaveProduct() As Task
        Try

            _vacationLeaveProduct = Await New ProductRepository().
                                        GetOrCreateLeaveTypeAsync(
                                            ProductConstant.VACATION_LEAVE,
                                            organizationId:=z_OrganizationID,
                                            userId:=z_User)
        Catch ex As Exception
            Throw New ResourceLoadingException("Vacation Leave Product", ex)
        End Try
    End Function

    Private Async Function LoadLeaves() As Task
        Try
            _leaves = (Await New LeaveRepository().
                                GetByTimePeriodAsync(organizationId:=z_OrganizationID,
                                                        timePeriod:=_payPeriodSpan)).
                      ToList()
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