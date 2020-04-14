Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports log4net
Imports Microsoft.EntityFrameworkCore

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport
    Private _logger As ILog = LogManager.GetLogger("EmployeeFormAppender")

    Private reportDocument As BpiInsuranceAmountReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try
            reportDocument = New BpiInsuranceAmountReport

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Function LoadingPayrollDataOnSuccess(t As Task(Of PayrollResources)) As Task
        Await ThreadingPayrollGeneration(t.Result)
    End Function

    Private Sub LoadingPayrollDataOnError(t As Task)
        _logger.Error("Error loading one of the payroll data.", t.Exception)
        MsgBox("Something went wrong while loading the payroll data needed for computation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Payroll Resources")

    End Sub

    Private Async Function ThreadingPayrollGeneration(resources As PayrollResources) As Task

        Dim bpiInsuranceProductID = resources.BpiInsuranceProduct.RowID.Value

        Using context = New PayrollContext
            Dim periods = Await context.PayPeriods.Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                Where(Function(p) p.Year = _selectedDate.Year).
                Where(Function(p) p.Month = _selectedDate.Month).
                Where(Function(p) p.PayFrequencyID = 1).
                ToListAsync()

            Dim periodIDs = periods.Select(Function(p) p.RowID.Value).ToArray()

            Dim adjustmens = Await context.Adjustments.
                    Include(Function(a) a.Paystub.Employee).
                    Where(Function(a) periodIDs.Contains(a.Paystub.PayPeriodID.Value)).
                    Where(Function(a) bpiInsuranceProductID = a.ProductID.Value).
                    ToListAsync()

            If Not adjustmens.Any Then
                MessageBox.Show($"No record found.", "BPI Insurance Payment Report", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Return
            End If

            Dim source = adjustmens.
                    GroupBy(Function(a) a.Paystub.EmployeeID).
                    Select(Function(a) ConvertToDataSource(a)).
                    OrderBy(Function(a) a.Column2).
                    ToList()

            reportDocument.SetDataSource(source)

            Dim parameterSetter = New CrystalReportParameterValueSetter(reportDocument)
            With parameterSetter
                .SetParameter("organizationName", z_CompanyName)

            End With

            Dim form = New LaGlobalEmployeeReportForm
            form.SetReportSource(reportDocument)
            form.Show()

        End Using
    End Function

    Private Async Sub SetDataSource()
        Using context = New PayrollContext
            Dim periods = Await context.PayPeriods.Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                Where(Function(p) p.Year = _selectedDate.Year).
                Where(Function(p) p.Month = _selectedDate.Month).
                Where(Function(p) p.PayFrequencyID = 1).
                ToListAsync()

            Dim periodIDs = periods.Select(Function(p) p.RowID.Value).ToArray()

            Dim firstPeriod = periods.FirstOrDefault

            LoadPayrollResource(firstPeriod)

            'Dim payrollResource = New PayrollResources(firstPeriod.RowID.Value, firstPeriod.PayFromDate, firstPeriod.PayToDate)

        End Using

    End Sub

    Private Sub LoadPayrollResource(firstPeriod As PayPeriod)
        Dim loadTask = Task.Factory.StartNew(
            Function()
                Dim resources = New PayrollResources(firstPeriod.RowID.Value, firstPeriod.PayFromDate, firstPeriod.PayToDate)
                Dim resourcesTask = resources.Load()
                resourcesTask.Wait()

                Return resources
            End Function,
            0)

        loadTask.ContinueWith(
        AddressOf LoadingPayrollDataOnSuccess,
        CancellationToken.None,
        TaskContinuationOptions.OnlyOnRanToCompletion,
        TaskScheduler.FromCurrentSynchronizationContext)

        loadTask.ContinueWith(
            AddressOf LoadingPayrollDataOnError,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Function ConvertToDataSource(a As IGrouping(Of Integer?, Adjustment)) As BpiInsuranceDataSource
        Dim e = a.FirstOrDefault.Paystub.Employee
        Dim middleName = If(Not String.IsNullOrWhiteSpace(e.MiddleName), $"{Left(e.MiddleName, 1)}.", String.Empty)
        Dim nameParts = {e.LastName, e.FirstName, middleName}
        Dim fullName = String.Join(", ", nameParts.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray())

        Dim result As New BpiInsuranceDataSource With {
            .Column1 = e.EmployeeNo,
            .Column2 = fullName,
            .Column3 = a.Sum(Function(adj) adj.Amount),
            .Column4 = _selectedDate.ToShortDateString()}

        Return result
    End Function

    Private Class BpiInsuranceDataSource

        'Employee ID
        Public Property Column1 As String

        'Employee Fullname
        Public Property Column2 As String

        'Payment/Amount
        Public Property Column3 As String

        'Selected Month - in a value of date
        Public Property Column4 As String

    End Class

End Class