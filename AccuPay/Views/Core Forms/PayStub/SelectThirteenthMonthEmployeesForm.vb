Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils

Public Class SelectThirteenthMonthEmployeesForm

    Private ReadOnly _currentPayPeriodId As Integer

    Private _employeeModels As List(Of EmployeeModel)

    Private _currentPayPeriod As PayPeriod

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _paystubRepository As PaystubRepository

    Private ReadOnly _timeEntryRepository As TimeEntryRepository

    Private ReadOnly _actualTimeEntryRepository As ActualTimeEntryRepository

    Private ReadOnly _salaryRepository As SalaryRepository

    Sub New(currentPayPeriodId As Integer, systemOwnerService As SystemOwnerService)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentPayPeriodId = currentPayPeriodId

        _employeeModels = New List(Of EmployeeModel)

        _systemOwnerService = systemOwnerService

        _payPeriodRepository = New PayPeriodRepository()

        _paystubRepository = New PaystubRepository()

        _timeEntryRepository = New TimeEntryRepository()

        _actualTimeEntryRepository = New ActualTimeEntryRepository()

        _salaryRepository = New SalaryRepository()

    End Sub

    Private Async Sub SelectThirteenthMonthEmployeesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EmployeeGridView.AutoGenerateColumns = False

        _currentPayPeriod = Await _payPeriodRepository.GetByIdAsync(_currentPayPeriodId)

        Await ShowEmployees()
    End Sub

    Private Async Function ShowEmployees() As Task

        Dim paystubs = Await _paystubRepository.
                                GetByPayPeriodWithEmployeeDivisionAndThirteenthMonthPayDetailsAsync(_currentPayPeriodId)

        Dim employeeModels As New List(Of EmployeeModel)

        For Each paystub In paystubs
            employeeModels.Add(New EmployeeModel(paystub))
        Next

        _employeeModels = employeeModels.OrderBy(Function(e) e.FullName).ToList

        EmployeeGridView.DataSource = _employeeModels

    End Function

    Private Async Sub RecalculateButton_Click(sender As Object, e As EventArgs) Handles RecalculateButton.Click

        Me.Cursor = Cursors.WaitCursor

        Dim calculator As New ThirteenthMonthPayCalculator(z_OrganizationID, z_User)

        Try

            Dim settings = ListOfValueCollection.Create()

            Dim datePeriod = New TimePeriod(_currentPayPeriod.PayFromDate, _currentPayPeriod.PayToDate)

            Dim timeEntries = (Await _timeEntryRepository.
                                    GetByDatePeriodAsync(z_OrganizationID, datePeriod)).
                                    ToList()

            Dim actualTimeEntries = (Await _actualTimeEntryRepository.
                                    GetByDatePeriodAsync(z_OrganizationID, datePeriod)).
                                    ToList()

            Dim salaries = (Await _salaryRepository.
                                    GetByCutOffAsync(z_OrganizationID, datePeriod.Start)).
                                    ToList()

            For Each employee In _employeeModels

                If employee.IsSelected = False Then

                    employee.ResetNewThirteenthMonthPayAmount()
                    Continue For
                End If

                Dim employeeTimeEntries = timeEntries.
                                            Where(Function(t) t.EmployeeID.Value = employee.EmployeeId).
                                            ToList()

                Dim employeeActualTimeEntries = actualTimeEntries.
                                            Where(Function(t) t.EmployeeID.Value = employee.EmployeeId).
                                            ToList()

                Dim salary = salaries.
                                Where(Function(s) s.EmployeeID.Value = employee.EmployeeId).
                                FirstOrDefault()

                Dim newPaystubThirteenthMonthPay = calculator.Calculate(employee.EmployeeObject,
                                                                          employee.PaystubObject,
                                                                          employeeTimeEntries,
                                                                          employeeActualTimeEntries,
                                                                          salary,
                                                                          settings,
                                                                          employee.PaystubObject.AllowanceItems,
                                                                        _systemOwnerService)

                employee.UpdateNewThirteenthMonthPayAmount(amount:=newPaystubThirteenthMonthPay.Amount,
                                                        basicPay:=newPaystubThirteenthMonthPay.BasicPay)

            Next

            UpdateForm()
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage("Recalculate 13th Month Pay")
        Finally

            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub UpdateForm()
        UpdateGridView()
        UpdateSaveButton()
    End Sub

    Private Sub UpdateGridView()
        EmployeeGridView.DataSource = _employeeModels
        EmployeeGridView.Refresh()
    End Sub

    Private Sub UpdateSaveButton()
        Dim forSavingCount = _employeeModels.Where(Function(m) m.ForSaving).Count()
        SaveButton.Enabled = forSavingCount > 0
        SaveButton.Text = $"&Save New 13th Month Pay ({forSavingCount})"

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Dim confirmation = MessageBoxHelper.Confirm(Of Boolean) _
                            ("Are you sure you want to update the selected 13th month amount(s)?")

        If confirmation = False Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Update 13th Month Amount",
            Async Function()
                Dim thirteenthMonthPays = _employeeModels.
                                                Where(Function(m) m.ForSaving).
                                                Select(Function(m)

                                                           Dim thirteenthMonthPay = m.PaystubObject.
                                                                                        ThirteenthMonthPay

                                                           thirteenthMonthPay.BasicPay = m.NewThirteenthMonthBasicPay
                                                           thirteenthMonthPay.Amount = m.NewThirteenthMonthAmount
                                                           thirteenthMonthPay.PaystubID = m.PaystubObject.RowID.Value
                                                           thirteenthMonthPay.LastUpdBy = z_User

                                                           Return thirteenthMonthPay
                                                       End Function)

                Await _paystubRepository.UpdateManyThirteenthMonthPaysAsync(thirteenthMonthPays)

                _employeeModels.
                        Where(Function(m) m.ForSaving).
                        ToList().
                        ForEach(Sub(m) m.ResetAfterSaving())

                UpdateForm()

                MessageBoxHelper.Information("The selected 13th month amount(s) were successfully updated.")
            End Function)
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click

        Dim forSavingCount = _employeeModels.Where(Function(m) m.ForSaving).Count()

        If forSavingCount = 0 Then
            Me.Close()
            Return
        End If

        Dim confirmation = MessageBoxHelper.Confirm(Of Boolean) _
            ("Are you sure you want to close the form without saving the recalculated 13th month amount(s)?")

        If confirmation = False Then Return

        Me.Close()

    End Sub

    Private Sub EmployeeGridView_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) _
        Handles EmployeeGridView.RowPrePaint

        If e.RowIndex < 0 Then Return

        Dim currentEmployee As EmployeeModel = GetCurrentModelByGridRowIndex(e.RowIndex)

        If currentEmployee Is Nothing Then Return

        Dim currentRow = EmployeeGridView.Rows(e.RowIndex)

        If currentRow Is Nothing Then Return

        If currentEmployee.ForSaving Then
            currentRow.DefaultCellStyle.BackColor = Color.Yellow
        Else
            currentRow.DefaultCellStyle.BackColor = Color.White
        End If

    End Sub

    Private Function GetCurrentModelByGridRowIndex(rowIndex As Integer) As EmployeeModel

        If rowIndex < 0 Then Return Nothing

        If rowIndex > _employeeModels.Count - 1 Then Return Nothing

        Return _employeeModels(rowIndex)

    End Function

    Private Class EmployeeModel
        Public ReadOnly Property EmployeeId As Integer
        Public ReadOnly Property PaystubObject As Paystub
        Public ReadOnly Property EmployeeObject As Employee
        Public ReadOnly Property PaystubAllowanceItems As ICollection(Of AllowanceItem)
        Public ReadOnly Property EmployeeNumber As String
        Public ReadOnly Property FirstName As String
        Public ReadOnly Property MiddleName As String
        Public ReadOnly Property LastName As String
        Public ReadOnly Property FullName As String
        Public ReadOnly Property EmployeeType As String
        Public ReadOnly Property PositionName As String
        Public ReadOnly Property DivisionName As String
        Public ReadOnly Property CurrentThirteenthMonthAmount As Decimal
        Public ReadOnly Property NewThirteenthMonthAmount As Decimal
        Public ReadOnly Property NewThirteenthMonthBasicPay As Decimal
        Public ReadOnly Property IsRecalculated As Boolean
        Public Property IsSelected As Boolean

        Sub New()

        End Sub

        Sub New(paystub As Paystub)

            EmployeeId = paystub.EmployeeID.Value
            PaystubObject = paystub
            EmployeeObject = paystub.Employee
            PaystubAllowanceItems = paystub.AllowanceItems

            EmployeeNumber = paystub.Employee.EmployeeNo
            FirstName = paystub.Employee.FirstName
            MiddleName = paystub.Employee.MiddleName
            LastName = paystub.Employee.LastName
            EmployeeType = paystub.Employee.EmployeeType
            FullName = paystub.Employee.FullNameWithMiddleInitialLastNameFirst

            PositionName = paystub.Employee.Position?.Name
            DivisionName = paystub.Employee.Position?.Division?.Name
            CurrentThirteenthMonthAmount = AccuMath.CommercialRound(paystub.ThirteenthMonthPay.Amount)

            IsSelected = True
            NewThirteenthMonthAmount = 0
            IsRecalculated = False

        End Sub

        Public ReadOnly Property NewThirteenthMonthPayDescription As String
            Get
                Return If(IsRecalculated, NewThirteenthMonthAmount.ToString("#,##0.00"), "-")
            End Get
        End Property

        Public ReadOnly Property ForSaving As Boolean
            Get
                Return IsRecalculated AndAlso CurrentThirteenthMonthAmount <>
                                                AccuMath.CommercialRound(NewThirteenthMonthAmount)
            End Get
        End Property

        Public Sub UpdateNewThirteenthMonthPayAmount(amount As Decimal, basicPay As Decimal)

            _NewThirteenthMonthAmount = amount
            _NewThirteenthMonthBasicPay = basicPay
            _IsRecalculated = True

        End Sub

        Public Sub ResetNewThirteenthMonthPayAmount()

            _NewThirteenthMonthAmount = 0
            _IsRecalculated = False

        End Sub

        Public Sub ResetAfterSaving()

            IsSelected = False
            _IsRecalculated = False
            _CurrentThirteenthMonthAmount = AccuMath.CommercialRound(_PaystubObject.ThirteenthMonthPay.Amount)
            _NewThirteenthMonthBasicPay = 0
            _NewThirteenthMonthAmount = 0

        End Sub

    End Class

End Class