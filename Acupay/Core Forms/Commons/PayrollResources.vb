Option Strict On

Imports System.Data.Entity
Imports System.Threading.Tasks
Imports AccuPay.Entity
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

    Private _timeEntries As DataTable

    Private _timeEntries2 As ICollection(Of TimeEntry)

    Private _loanSchedules As ICollection(Of PayrollSys.LoanSchedule)

    Private _loanTransactions As ICollection(Of PayrollSys.LoanTransaction)

    Private _fixedTaxableMonthlyAllowances As DataTable

    Private _fixedNonTaxableMonthlyAllowances As DataTable

    Private _products As IEnumerable(Of Product)

    Private _socialSecurityBrackets As ICollection(Of SocialSecurityBracket)

    Private _philHealthBrackets As ICollection(Of PhilHealthBracket)

    Private _withholdingTaxBrackets As ICollection(Of WithholdingTaxBracket)

    Private _listOfValues As ICollection(Of ListOfValue)

    Private _paystubs As IEnumerable(Of Paystub)

    Private _previousPaystubs As IEnumerable(Of Paystub)

    Private _isEndOfMonth As Boolean

    Private _payPeriod As PayPeriod

    Private _payRates As ICollection(Of PayRate)

    Private _allowances As ICollection(Of Allowance)

    Public ReadOnly Property Employees As ICollection(Of Employee)
        Get
            Return _employees
        End Get
    End Property

    Public ReadOnly Property TimeEntries As DataTable
        Get
            Return _timeEntries
        End Get
    End Property

    Public ReadOnly Property TimeEntries2 As ICollection(Of TimeEntry)
        Get
            Return _timeEntries2
        End Get
    End Property

    Public ReadOnly Property LoanSchedules As ICollection(Of PayrollSys.LoanSchedule)
        Get
            Return _loanSchedules
        End Get
    End Property

    Public ReadOnly Property LoanTransactions As ICollection(Of PayrollSys.LoanTransaction)
        Get
            Return _loanTransactions
        End Get
    End Property

    Public ReadOnly Property Salaries As DataTable
        Get
            Return _salaries
        End Get
    End Property

    Public ReadOnly Property FixedTaxableMonthlyAllowances As DataTable
        Get
            Return _fixedTaxableMonthlyAllowances
        End Get
    End Property

    Public ReadOnly Property FixedNonTaxableMonthlyAllowances As DataTable
        Get
            Return _fixedNonTaxableMonthlyAllowances
        End Get
    End Property

    Public ReadOnly Property Products As IEnumerable(Of Product)
        Get
            Return _products
        End Get
    End Property

    Public ReadOnly Property Paystubs As IEnumerable(Of AccuPay.Entity.Paystub)
        Get
            Return _paystubs
        End Get
    End Property

    Public ReadOnly Property PreviousPaystubs As IEnumerable(Of Paystub)
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

    Public Sub New(payPeriodID As String, payDateFrom As Date, payDateTo As Date)
        _payPeriodID = Integer.Parse(payPeriodID)
        _payDateFrom = payDateFrom
        _payDateTo = payDateTo
    End Sub

    Public Async Function Load() As Task
        Dim loadTimeEntriesTask = LoadTimeEntries()
        Dim loadSalariesTask = LoadSalaries()
        Dim loadLoanSchedulesTask = LoadLoanSchedules()
        Dim loadLoanTransactionsTask = LoadLoanTransactions()

        Await Task.WhenAll({
            LoadEmployees(),
            loadTimeEntriesTask,
            loadLoanSchedulesTask,
            loadLoanTransactionsTask,
            loadSalariesTask,
            LoadProducts(),
            LoadPreviousPaystubs(),
            LoadFixedTaxableMonthlyAllowances(),
            LoadFixedNonTaxableMonthlyAllowancesTask(),
            LoadSocialSecurityBrackets(),
            LoadPhilHealthBrackets(),
            LoadWithholdingTaxBrackets(),
            LoadSettings(),
            LoadPayPeriod(),
            LoadPayRates(),
            LoadAllowances(),
            LoadTimeEntries2()
        })
    End Function

    Public Async Function LoadEmployees() As Task
        Using context = New PayrollContext()
            Dim query = From e In context.Employees
                        Where e.OrganizationID = z_OrganizationID And
                            e.EmploymentStatus <> "Resigned" And
                            e.EmploymentStatus <> "Terminated"

            _employees = Await query.ToListAsync()
        End Using
    End Function

    Public Async Function LoadTimeEntries() As Task
        Dim timeEntrySql = <![CDATA[
            SELECT
                SUM(COALESCE(ete.TotalDayPay,0)) 'TotalDayPay',
                ete.EmployeeID,
                ete.Date,
                SUM(COALESCE(ete.RegularHoursAmount, 0)) 'RegularHoursAmount',
                SUM(COALESCE(ete.RegularHoursWorked, 0)) 'RegularHoursWorked',
                SUM(COALESCE(ete.OvertimeHoursWorked, 0)) 'OvertimeHoursWorked',
                SUM(COALESCE(ete.OvertimeHoursAmount, 0)) 'OvertimeHoursAmount',
                SUM(COALESCE(ete.NightDifferentialHours, 0)) 'NightDifferentialHours',
                SUM(COALESCE(ete.NightDiffHoursAmount, 0)) 'NightDiffHoursAmount',
                SUM(COALESCE(ete.NightDifferentialOTHours, 0)) 'NightDifferentialOTHours',
                SUM(COALESCE(ete.NightDiffOTHoursAmount, 0)) 'NightDiffOTHoursAmount',
                SUM(COALESCE(ete.RestDayHours, 0)) 'RestDayHours',
                SUM(COALESCE(ete.RestDayAmount, 0)) 'RestDayAmount',
                SUM(COALESCE(ete.Leavepayment, 0)) 'Leavepayment',
                SUM(COALESCE(ete.HolidayPayAmount, 0)) 'HolidayPayAmount',
                SUM(COALESCE(ete.HoursLate, 0)) 'HoursLate',
                SUM(COALESCE(ete.HoursLateAmount, 0)) 'HoursLateAmount',
                SUM(COALESCE(ete.UndertimeHours, 0)) 'UndertimeHours',
                SUM(COALESCE(ete.UndertimeHoursAmount, 0)) 'UndertimeHoursAmount',
                SUM(COALESCE(ete.Absent, 0)) AS 'Absent',
                SUM(COALESCE(ete.VacationLeaveHours, 0)) AS 'VacationLeaveHours', 
                SUM(COALESCE(ete.SickLeaveHours, 0)) As 'SickLeaveHours',
                SUM(COALESCE(ete.OtherLeaveHours, 0)) AS 'OtherLeaveHours'
            FROM employeetimeentry ete
            LEFT JOIN employee e
            ON e.RowID = ete.EmployeeID
            LEFT JOIN payrate pr
            ON pr.RowID = ete.PayRateID AND
                pr.OrganizationID = ete.OrganizationID
            WHERE ete.OrganizationID='@OrganizationID' AND
                ete.Date BETWEEN IF('@DateFrom' < e.StartDate, e.StartDate, '@DateFrom') AND '@DateTo'
            GROUP BY ete.EmployeeID
            ORDER BY ete.EmployeeID;
        ]]>.Value

        timeEntrySql = timeEntrySql.Replace("@OrganizationID", orgztnID) _
            .Replace("@DateFrom", _payDateFrom.ToString("s")) _
            .Replace("@DateTo", _payDateTo.ToString("s"))

        _timeEntries = Await New SqlToDataTable(timeEntrySql).ReadAsync()
    End Function

    Private Async Function LoadTimeEntries2() As Task
        Using context = New PayrollContext()
            Dim query = From t In context.TimeEntries.Include(Function(t) t.ShiftSchedule.Shift)
                        Where t.OrganizationID = z_OrganizationID And
                            _payDateFrom <= t.EntryDate And
                            t.EntryDate <= _payDateTo
                        Select t

            _timeEntries2 = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadLoanSchedules() As Task
        Using context = New PayrollContext()
            Dim query = From l In context.LoanSchedules
                        Select l
                        Where l.OrganizationID = z_OrganizationID And
                                                      l.DedEffectiveDateFrom <= _payDateTo And
                                                      l.Status = "In Progress" And
                                                      l.BonusID Is Nothing
            _loanSchedules = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadLoanTransactions() As Task
        Using context = New PayrollContext()
            Dim query = From t In context.LoanTransactions
                        Select t
                        Where t.OrganizationID = z_OrganizationID And
                            t.PayPeriodID = CType(_payPeriodID, Integer?)
            _loanTransactions = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadSalaries() As Task
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
    End Function

    Private Async Function LoadFixedTaxableMonthlyAllowances() As Task
        Dim fixedTaxableMonthlyAllowanceSql = String.Empty

        If _isEndOfMonth Then
            fixedTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
            " AND (ea.EffectiveStartDate >= '" & _payDateFrom.ToString("s") & "' OR ea.EffectiveEndDate >= '" & _payDateFrom.ToString("s") & "')" &
            " AND (ea.EffectiveStartDate <= '" & _payDateTo.ToString("s") & "' OR ea.EffectiveEndDate <= '" & _payDateTo.ToString("s") & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 1;"
        Else
            fixedTaxableMonthlyAllowanceSql =
                "SELECT ea.* " &
                "FROM employeeallowance ea " &
                "INNER JOIN product p " &
                "ON p.RowID = ea.ProductID " &
                "WHERE ea.OrganizationID = '" & orgztnID & "' " &
                "AND ea.AllowanceFrequency = 'Semi-monthly' " &
                " AND (ea.EffectiveStartDate >= '" & _payDateFrom.ToString("s") & "' OR ea.EffectiveEndDate >= '" & _payDateFrom.ToString("s") & "')" &
                " AND (ea.EffectiveStartDate <= '" & _payDateTo.ToString("s") & "' OR ea.EffectiveEndDate <= '" & _payDateTo.ToString("s") & "')" &
                " AND p.`Fixed` = 1 " &
                "AND p.`Status` = 1;"
        End If

        _fixedTaxableMonthlyAllowances = Await New SqlToDataTable(fixedTaxableMonthlyAllowanceSql).ReadAsync()
    End Function

    Private Async Function LoadFixedNonTaxableMonthlyAllowancesTask() As Task
        Dim fixedNonTaxableMonthlyAllowanceSql = String.Empty
        If _isEndOfMonth Then
            fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "On p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency IN ('Monthly','Semi-monthly') " &
            " AND (ea.EffectiveStartDate >= '" & _payDateFrom.ToString("s") & "' OR ea.EffectiveEndDate >= '" & _payDateFrom.ToString("s") & "')" &
            " AND (ea.EffectiveStartDate <= '" & _payDateTo.ToString("s") & "' OR ea.EffectiveEndDate <= '" & _payDateTo.ToString("s") & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 0;"
        Else '                      'means first half of the month
            fixedNonTaxableMonthlyAllowanceSql =
            "SELECT ea.* " &
            "FROM employeeallowance ea " &
            "INNER JOIN product p " &
            "ON p.RowID = ea.ProductID " &
            "WHERE ea.OrganizationID = '" & orgztnID & "' " &
            "AND ea.AllowanceFrequency = 'Semi-monthly' " &
            " AND (ea.EffectiveStartDate >= '" & _payDateFrom.ToString("s") & "' OR ea.EffectiveEndDate >= '" & _payDateFrom.ToString("s") & "')" &
            " AND (ea.EffectiveStartDate <= '" & _payDateTo.ToString("s") & "' OR ea.EffectiveEndDate <= '" & _payDateTo.ToString("s") & "')" &
            " AND p.`Fixed` = 1 " &
            "AND p.`Status` = 0;"
        End If

        _fixedNonTaxableMonthlyAllowances = Await New SqlToDataTable(fixedNonTaxableMonthlyAllowanceSql).ReadAsync()
    End Function

    Private Async Function LoadProducts() As Task
        Using context = New PayrollContext()
            Dim query = From p In context.Products
                        Where p.OrganizationID = z_OrganizationID
            _products = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadPaystubs() As Task
        Using context = New PayrollContext()
            Dim query = From p In context.Paystubs
                        Where p.PayFromdate = _payDateFrom And p.PayToDate = _payDateTo

            _paystubs = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadPreviousPaystubs() As Task
        Dim previousCutoffEnd = _payDateFrom.AddDays(-1)

        Using context = New PayrollContext()
            Dim query = From p In context.Paystubs
                        Where p.PayToDate = previousCutoffEnd And
                            p.OrganizationID = z_OrganizationID

            _previousPaystubs = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadSocialSecurityBrackets() As Task
        Using context = New PayrollContext()
            Dim query = From s In context.SocialSecurityBrackets

            _socialSecurityBrackets = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadPhilHealthBrackets() As Task
        Using context = New PayrollContext()
            Dim query = From p In context.PhilHealthBrackets

            _philHealthBrackets = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadWithholdingTaxBrackets() As Task
        Using context = New PayrollContext()
            Dim query = From w In context.WithholdingTaxBrackets
                        Select w

            _withholdingTaxBrackets = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadSettings() As Task
        Using context = New PayrollContext()
            Dim query = From s In context.ListOfValues

            _listOfValues = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadPayPeriod() As Task
        Using context = New PayrollContext()
            Dim query = From p In context.PayPeriods
                        Where p.RowID = _payPeriodID

            _payPeriod = Await query.FirstOrDefaultAsync()
        End Using
    End Function

    Private Async Function LoadPayRates() As Task
        Using context = New PayrollContext()
            Dim query = From p In context.PayRates
                        Where p.OrganizationID = z_OrganizationID And
                            _payDateFrom <= p.RateDate And
                            p.RateDate <= _payDateTo

            _payRates = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function LoadAllowances() As Task
        Using context = New PayrollContext()
            Dim query = From a In context.Allowances
                        Where a.EffectiveStartDate <= _payDateTo And
                            _payDateFrom <= a.EffectiveEndDate And
                            a.OrganizationID = z_OrganizationID

            _allowances = Await query.ToListAsync()
        End Using
    End Function

End Class
