Option Strict On

Imports System.Threading.Tasks
Imports System.Data.Entity

''' <summary>
''' Takes care of loading all the information needed to produce the payroll for a given pay period.
''' </summary>
Public Class PayrollResources

    Private _payPeriodID As String

    Private _payDateFrom As Date

    Private _payDateTo As Date

    Private _salaries As DataTable

    Private _timeEntries As DataTable

    Private _loanSchedules As ICollection(Of PayrollSys.LoanSchedule)

    Private _loanTransactions As ICollection(Of PayrollSys.LoanTransaction)

    Private _fixedTaxableMonthlyAllowances As DataTable

    Private _fixedNonTaxableMonthlyAllowances As DataTable

    Private _isEndOfMonth As Boolean

    Public ReadOnly Property TimeEntries As DataTable
        Get
            Return _timeEntries
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

    Public Sub New(payPeriodID As String, payDateFrom As Date, payDateTo As Date)
        _payPeriodID = payPeriodID
        _payDateFrom = payDateFrom
        _payDateTo = payDateTo
    End Sub

    Public Async Function Load() As Task
        Dim loadTimeEntriesTask = LoadTimeEntries()
        Dim loadSalariesTask = LoadSalaries()
        Dim loadLoanSchedulesTask = LoadLoanSchedules()
        Dim loadLoanTransactionsTask = LoadLoanTransactions()

        Await Task.WhenAll({
            loadTimeEntriesTask,
            loadLoanSchedulesTask,
            loadLoanTransactionsTask,
            loadSalariesTask
        })
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
                IFNULL(emt.emtAmount,0) AS emtAmount
            FROM employeetimeentry ete
            LEFT JOIN employee e
            ON e.RowID = ete.EmployeeID
            LEFT JOIN payrate pr
            ON pr.RowID = ete.PayRateID AND
                pr.OrganizationID = ete.OrganizationID
            LEFT JOIN (
                SELECT
                    ete.RowID,
                    e.RowID AS eRowID,
                    (SUM(ete.RegularHoursAmount) * (pr.`PayRate` - 1.0)) AS emtAmount
                FROM employeetimeentry ete
                INNER JOIN employee e
                ON e.RowID = ete.EmployeeID AND
                    e.OrganizationID = ete.OrganizationID AND
                    (e.CalcSpecialHoliday = '1' OR e.CalcHoliday = '1')
                INNER JOIN payrate pr
                ON pr.RowID = ete.PayRateID AND
                    pr.PayType != 'Regular Day'
                WHERE ete.OrganizationID='@OrganizationID' AND
                    ete.`Date` BETWEEN '@DateFrom' AND '@DateTo'
            ) emt
            ON emt.RowID IS NOT NULL AND
                emt.eRowID = ete.EmployeeID
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

    Private Sub LoadFixedMonthlyAllowances()
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

        _fixedTaxableMonthlyAllowances = New MySQLQueryToDataTable(fixedTaxableMonthlyAllowanceSql).ResultTable

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

        _fixedNonTaxableMonthlyAllowances = New MySQLQueryToDataTable(fixedNonTaxableMonthlyAllowanceSql).ResultTable
    End Sub

End Class
