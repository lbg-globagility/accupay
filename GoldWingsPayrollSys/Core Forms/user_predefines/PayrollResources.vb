Option Strict On

Public Class PayrollResources

    Private _payDateFrom As Date

    Private _payDateTo As Date

    Private _timeEntries As DataTable

    Public ReadOnly Property TimeEntries As DataTable
        Get
            Return _timeEntries
        End Get
    End Property

    Public Async Sub LoadTimeEntries()
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
            .Replace("@DateFrom", CStr(_payDateFrom)) _
            .Replace("@DateTo", CStr(_payDateTo))

        _timeEntries = Await New SqlToDataTable(timeEntrySql).ReadAsync()
    End Sub

End Class
