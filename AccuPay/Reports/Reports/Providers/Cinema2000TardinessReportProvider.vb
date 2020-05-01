Imports AccuPay.Entity
Imports AccuPay.Utils
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore

Public Class Cinema2000TardinessReportProvider
    Implements IReportProvider

    Private Const JanuaryMonth As Integer = 1
    Private Const DecemberMonth As Integer = 12
    Public Property Name As String = "Tardiness Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim n_selectMonth As New selectMonth

        If Not n_selectMonth.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim firstDate = CDate(n_selectMonth.MonthValue)

        Dim isLimitedReport As Boolean = False

        Dim question = $"Do you want to show only employees with 8 or more days late for {firstDate.ToString("MMMM")} {firstDate.Year}?"
        If MessageBoxHelper.Confirm(Of Boolean) _
            (question, "Filter Tardiness Report", messageBoxIcon:=MessageBoxIcon.Question) Then

            isLimitedReport = True

        End If

        Try

            Dim report = New Cinemas_Tardiness_Report

            Dim txtOrgName As TextObject = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtOrgName"), TextObject)
            txtOrgName.Text = orgNam.ToUpper

            Dim txtReportName As TextObject = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtReportName"), TextObject)
            txtReportName.Text = $"Tadiness Report - {firstDate.ToString("MMMM yyyy")}"

            Dim txtAddress = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtAddress"), TextObject)

            txtAddress.Text = PayrollTools.GetOrganizationAddress()

            Dim tardinessReportModels = Await GetTardinessReportModels(firstDate, isLimitedReport)

            report.SetDataSource(tardinessReportModels)

            Dim crvwr As New CrysRepForm
            crvwr.crysrepvwr.ReportSource = report
            crvwr.Show()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

    Private Async Function GetTardinessReportModels([date] As Date, isLimitedReport As Boolean) As Threading.Tasks.Task(Of List(Of CinemaTardinessReportModel))

        Dim firstDayOfTheMonth = New Date([date].Year, [date].Month, 1)
        Dim lastDayOfTheMonth = New Date([date].Year, [date].Month, Date.DaysInMonth([date].Year, [date].Month))

        Dim previousMonth As Integer = [date].Month - 1

        If firstDayOfTheMonth.Month = JanuaryMonth Then
            'if the firstDayOfTheMonth is 01/01/2020, previousMonth should be December (12)
            previousMonth = DecemberMonth
        Else
            'if the firstDayOfTheMonth is 02/01/2020, previousMonth should be January (1)
            previousMonth = [date].Month - 1
        End If

        Dim firstDayOneYear As Date
        If firstDayOfTheMonth.Month = DecemberMonth Then
            'if the firstDayOfTheMonth is 12/01/2019, lastYearDate is 1/01/2019
            firstDayOneYear = New Date(firstDayOfTheMonth.Year, JanuaryMonth, 1)
        Else
            'if the firstDayOfTheMonth is 09/01/2019, lastYearDate is 10/01/2018
            firstDayOneYear = New Date(firstDayOfTheMonth.Year - 1, firstDayOfTheMonth.Month + 1, 1)
        End If

        Dim tardinessReportModels As New List(Of CinemaTardinessReportModel)

        '#1. Get all the employee that have lates on the selected month
        '#2. If the affected employee has no record on the database for tardiness (for this year) yet, create a tardiness record for the employee
        '#3. Get all the tardiness record of the affected employees
        '#4. Update the value of [NumberOfOffense] from the data retrieved from #3

        Dim daysRequirement = If(isLimitedReport, CinemaTardinessReportModel.DaysLateLimit, 0)

        Using context As New PayrollContext

            '#1.
            Dim tardinessReportModelQuery = context.TimeEntries.
                                            Include(Function(t) t.Employee).
                                            Where(Function(t) t.Date >= firstDayOfTheMonth AndAlso t.Date <= lastDayOfTheMonth).
                                            Where(Function(t) t.OrganizationID.Value = z_OrganizationID).
                                            Where(Function(t) t.LateHours > 0).
                                            GroupBy(Function(t) t.Employee).
                                            Select(Function(gt) New CinemaTardinessReportModel With {
                                                    .EmployeeId = gt.Key.RowID.Value,
                                                    .EmployeeName = $"{gt.Key.LastName}, {gt.Key.FirstName}{If(gt.Key.MiddleInitial Is Nothing, "", ", " & gt.Key.MiddleInitial)}",
                                                    .Days = gt.Count(),
                                                    .Hours = gt.Sum(Function(t) t.LateHours)
                                            })

            If isLimitedReport Then
                tardinessReportModelQuery = tardinessReportModelQuery.
                        Where(Function(t) t.Days >= CinemaTardinessReportModel.DaysLateLimit)
            End If

            tardinessReportModels = Await tardinessReportModelQuery.ToListAsync

            '#2
            'this list contains the first offense dates per employee within the year of the report
            Dim employeeTardinessDates = Await GetEmployeeTardinessRecordList(
                                                firstDayOfTheMonth,
                                                lastDayOfTheMonth,
                                                firstDayOneYear,
                                                tardinessReportModels,
                                                context)

            If tardinessReportModels.Count <> employeeTardinessDates.Count Then
                Throw New Exception("Error creating new tardiness records.")
            End If

            '#3
            Dim earliestFirstOffenseDate = employeeTardinessDates.
                                            OrderBy(Function(t) t.FirstOffenseDate).
                                            FirstOrDefault?.FirstOffenseDate

            If earliestFirstOffenseDate Is Nothing Then

                earliestFirstOffenseDate = firstDayOfTheMonth

            End If

            Dim previousOffenseList = Await context.TimeEntries.
                                            Include(Function(t) t.Employee).
                                            Where(Function(t) t.Date >= earliestFirstOffenseDate.Value AndAlso t.Date <= lastDayOfTheMonth).
                                            Where(Function(t) t.OrganizationID.Value = z_OrganizationID).
                                            Where(Function(t) t.LateHours > 0).
                                            ToListAsync

            '#4
            For Each tardinessReportModel In tardinessReportModels

                tardinessReportModel.NumberOfOffense = 0

                Dim firstOffenseDate = employeeTardinessDates.
                                                    Where(Function(e) e.EmployeeId = tardinessReportModel.EmployeeId).
                                                    FirstOrDefault?.FirstOffenseDate

                If firstOffenseDate Is Nothing Then
                    Throw New Exception("Error creating new tardiness records.")
                End If

                Dim employeeOffenseList = previousOffenseList.
                                            Where(Function(o) o.EmployeeID = tardinessReportModel.EmployeeId).
                                            Where(Function(o) o.Date >= firstOffenseDate).
                                            Where(Function(o) o.Date <= lastDayOfTheMonth).
                                            ToList

                Dim employeeOffenseListPerMonth = employeeOffenseList.
                                                    GroupBy(Function(o) o.Date.Month).
                                                    Select(Function(m) New CinemaTardinessReportModel.PerMonth With {
                                                        .EmployeeId = tardinessReportModel.EmployeeId,
                                                        .Month = m.Key,
                                                        .Days = m.Count(),
                                                        .Hours = m.Sum(Function(t) t.LateHours)
                                                    }).
                                                    ToList

                Dim previousNumberOfOffense = employeeOffenseListPerMonth.
                                                Where(Function(o) o.Days >= CinemaTardinessReportModel.DaysLateLimit).
                                                Count

                tardinessReportModel.NumberOfOffense += previousNumberOfOffense

            Next

            Await context.SaveChangesAsync()
        End Using

        'tardinessReportModels = GetSampleTardinessReportModels()

        Return tardinessReportModels

    End Function

    Private Async Function GetEmployeeTardinessRecordList(
                            firstDayOfTheMonth As Date,
                            lastDayOfTheMonth As Date,
                            firstDayOneYear As Date,
                            tardinessReportModels As List(Of CinemaTardinessReportModel),
                            context As PayrollContext) _
                            As Threading.Tasks.Task(Of List(Of EmployeeTardinessDate))

        Dim employeeIds = tardinessReportModels.Select(Function(t) t.EmployeeId).ToArray
        'Original span [10/01/2018 to 09/30/2019]
        'Get the records that is within 2 years from now
        'ex. 11/01/2017 - 09/30/2019
        'because for example if there is a first offense tardiness record for Jan. 2018
        'That should only reset on Jan. 2019
        'That employee's tardiness span would be from Jan. 2019 to Sep. 2019
        'So the offenses that the employee got from Oct. 2018 to Dec. 2018 should not be included in this report
        'Those offenses will be included on the span of Jan. 2018 to Dec. 2018

        'Example 2
        '--
        'Jan 2018 to Dec 2018
        'Dec 2017 to Nov 2018
        'Nov 2017 to Oct 2018
        '-------------------------
        'Oct 2017 to Sept 2018
        '--

        'The [Nov. 2017 to Oct. 2018] timespan can still affect our September 2019 report
        'since if there is a first offense tardiness record on November 2017
        'an offense recorded on October 2018 would fit in that time span
        'that employees tardiness record would reset on November 2018
        'so the September 2019 report should not count that October 2018 offense

        Dim firstDayTwoYearsBefore = firstDayOfTheMonth.AddMonths(-22) '-22 because -2years which is -24 months +2months (Nov17-Sep19 span)

        Await context.TardinessRecords.LoadAsync()

        Dim employeeTardinessRecords = context.TardinessRecords.Local.
                                                Where(Function(t) employeeIds.Contains(t.EmployeeId)).
                                                Where(Function(t) t.FirstOffenseDate >= firstDayTwoYearsBefore).
                                                Where(Function(t) t.FirstOffenseDate <= lastDayOfTheMonth).
                                                ToList

        Dim employeesWithNoTardinessRecordsYet = employeeIds.Except(employeeTardinessRecords.Select(Function(t) t.EmployeeId))

        For Each employeeId In employeesWithNoTardinessRecordsYet

            context.TardinessRecords.Add(New TardinessRecord With {
                .EmployeeId = employeeId,
                .FirstOffenseDate = firstDayOfTheMonth,
                .Year = firstDayOfTheMonth.Year
            })

        Next

        employeeTardinessRecords = context.TardinessRecords.Local.
                                            Where(Function(t) employeeIds.Contains(t.EmployeeId)).
                                            Where(Function(t) t.FirstOffenseDate >= firstDayTwoYearsBefore).
                                            Where(Function(t) t.FirstOffenseDate <= lastDayOfTheMonth).
                                            ToList

        If employeeIds.Count <> employeeTardinessRecords.Count Then
            Throw New Exception("Error creating new tardiness records.")
        End If

        'This will create the employee tardiness date within the year of the reports
        Dim employeeTardinessDates = CreateEmployeeTardinessDates(employeeTardinessRecords, firstDayOneYear)

        If employeeIds.Count <> employeeTardinessDates.Count Then
            Throw New Exception("Error creating new tardiness records.")
        End If

        Return employeeTardinessDates
    End Function

    Private Function CreateEmployeeTardinessDates(
                        employeeTardinessRecords As List(Of TardinessRecord),
                        firstDayOneYear As Date) _
                        As List(Of EmployeeTardinessDate)

        Dim employeeTardinessDates As New List(Of EmployeeTardinessDate)
        Dim groupedTardinessRecords = employeeTardinessRecords.GroupBy(Function(e) e.EmployeeId)

        For Each groupedTardinessRecord In groupedTardinessRecords

            Dim latestTardinessRecordDate = groupedTardinessRecord.
                                                OrderByDescending(Function(t) t.FirstOffenseDate).
                                                FirstOrDefault

            If latestTardinessRecordDate Is Nothing Then Continue For

            'if the latest tardiness record is from last 2 years, get its equivalent date for this year
            'Ex: Tardiness Report is Septemper 2019
            'firstDayOneYear would be 10/01/2018
            'if firstOffenseDate is 09/01/2018
            'it will reset to 09/01/2019
            'so the employee will start gaining new tardiness offense on 09/01/2019

            Dim tardinessRecordDate = latestTardinessRecordDate.FirstOffenseDate

            If latestTardinessRecordDate.FirstOffenseDate < firstDayOneYear Then
                tardinessRecordDate = latestTardinessRecordDate.FirstOffenseDate.AddYears(1)
            End If

            employeeTardinessDates.Add(New EmployeeTardinessDate With {
                .EmployeeId = groupedTardinessRecord.Key,
                .FirstOffenseDate = tardinessRecordDate
            })

        Next

        Return employeeTardinessDates

    End Function

    Private Shared Function GetSampleTardinessReportModels() As List(Of CinemaTardinessReportModel)
        Dim tardinessReportModels As New List(Of CinemaTardinessReportModel)

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Andal, Myrna, B.",
                .Days = 2,
                .Hours = 4,
                .NumberOfOffense = 0
        })

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Banal, Joseph Brian, A.",
                .Days = 8,
                .Hours = 8.5,
                .NumberOfOffense = 2
        })

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Banga, Jessa, O.",
                .Days = 9,
                .Hours = 10,
                .NumberOfOffense = 4
        })

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Santos, Joshua Noel C.",
                .Days = 0,
                .Hours = 0,
                .NumberOfOffense = 0
        })
        Return tardinessReportModels
    End Function

    Private Class EmployeeTardinessDate

        Public Property EmployeeId As Integer

        Private _firstOffenseDate As Date

        Public Property FirstOffenseDate() As Date 'First offense within the year
            Get
                Return _firstOffenseDate
            End Get
            Set(ByVal value As Date)
                'always convert to first day of the month
                _firstOffenseDate = New Date(value.Year, value.Month, 1)
            End Set
        End Property

    End Class

End Class