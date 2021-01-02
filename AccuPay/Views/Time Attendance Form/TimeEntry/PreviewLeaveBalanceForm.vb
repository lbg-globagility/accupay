Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class PreviewLeaveBalanceForm

    Private payPeriodId As Integer

    Private periodDateFrom, periodDateTo As Date

    Private _employees As IList(Of Employee)

    Private _employeeModels As IList(Of EmployeeModel)

    Private policy As New RenewLeaveBalancePolicy

    Private _listOfValueService As ListOfValueService

    Private _employeeRepository As IEmployeeRepository

    Sub New()

        InitializeComponent()

        _listOfValueService = MainServiceProvider.GetRequiredService(Of ListOfValueService)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
    End Sub

    Private Async Sub PreviewLeaveBalanceForm_LoadAsync(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim basis = Await _listOfValueService.CreateAsync("ResetLeaveBalancePolicy")

        Dim leaveAllowanceAmountBasis = basis.GetValue("LeaveAllowanceAmountBasis")

        If RenewLeaveBalancePolicy.LeaveAllowanceAmountBasis.NumberOfService.ToString = leaveAllowanceAmountBasis Then
        Else
            policy.LeaveAllowanceAmount = RenewLeaveBalancePolicy.LeaveAllowanceAmountBasis.Default
        End If

        policy.ProrateOnFirstAnniversary = basis.GetBoolean("ResetLeaveBalancePolicy", "ProrateOnFirstAnniversary", False)

        Await LoadEmployees()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub btnReset_ClickAsync(sender As Object, e As EventArgs) Handles btnReset.Click
        'RenewLeaveBalance does not update when the payperiod chosen is not the start of the year.
        'Remove the Return statement when that is fixed.

        Return

        'Dim result As DialogResult
        'Dim isOk As Boolean = False

        'Using dialog = New StartNewPayPeriodDialog()
        '    result = dialog.ShowDialog()

        '    If result = DialogResult.OK Then
        '        payPeriodId = dialog.Id
        '        periodDateFrom = dialog.Start
        '        periodDateTo = dialog.End
        '        isOk = True
        '    End If
        'End Using

        'If isOk Then

        '    If policy.ProrateOnFirstAnniversary = False Then

        '        If policy.LeaveAllowanceAmount = RenewLeaveBalancePolicy.LeaveAllowanceAmountBasis.Default Then

        '            'Await RenewLeaveBalances()

        '            MessageBoxHelper.Information($"Leave balances of the employees of {orgNam} were successfully reset.")
        '        End If
        '    Else

        '    End If

        '    Await LoadEmployees()
        'End If

    End Sub

#Region "RenewLeaveBalances"

    'This code does not update when the payperiod chosen is not the start of the year
    'Uncomment this when this is fixed
    'Private Async Function RenewLeaveBalances() As Threading.Tasks.Task

    'RemovePreviousLeaveCreditsAsync()

    'Dim categoryRepo = New Repositories.CategoryRepository
    'Dim leaveCategory = Await categoryRepo.GetByNameAsync(z_OrganizationID,
    '                                                      ProductConstant.LEAVE_TYPE_CATEGORY)

    'Using context = New PayrollContext()
    '    Dim leaveCategoryId = leaveCategory.RowID

    '    Dim leaveTypeList = New String() {ProductConstant.VACATION_LEAVE, ProductConstant.SICK_LEAVE}
    '    Dim vacationLeaveType = leaveTypeList(0)
    '    Dim sickLeaveType = leaveTypeList(1)

    '    Dim leaveTypes =
    '        Await context.Products.
    '        Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
    '        Where(Function(p) Nullable.Equals(p.CategoryID, leaveCategoryId)).
    '        Where(Function(p) leaveTypeList.Any(Function(type) Equals(type, p.PartNo))).
    '        Select(Function(p) New LeaveTypeModel(p)).
    '        ToListAsync()

    '    Dim leaveTypeIds = leaveTypes.Select(Function(p) p.RowID).ToList()

    '    Dim employeeIds = _employeeModels.Select(Function(e) e.RowID).ToArray()

    '    Dim leaveLedgers =
    '        Await context.LeaveLedgers.
    '        Where(Function(ll) Nullable.Equals(ll.OrganizationID, z_OrganizationID)).
    '        Where(Function(ll) leaveTypeIds.Any(Function(leaveId) Nullable.Equals(leaveId, ll.ProductID))).
    '        Where(Function(ll) employeeIds.Any(Function(id) Nullable.Equals(id, ll.EmployeeID))).
    '        ToListAsync()

    '    Dim vacationLeaveTypeId = leaveTypes.Where(Function(l) l.PartNo = vacationLeaveType).FirstOrDefault.RowID
    '    Dim sickLeaveTypeId = leaveTypes.Where(Function(l) l.PartNo = sickLeaveType).FirstOrDefault.RowID

    '    Dim vacationLeaveTransactions = New List(Of LeaveTransaction)
    '    Dim sickLeaveTransactions = New List(Of LeaveTransaction)

    '    For Each employee In _employeeModels
    '        Dim employeeId = employee.RowID

    '        Dim leaveLedgerId As Integer
    '        Dim ll As LeaveLedger

    '        'Vacation Leave
    '        ll = GetLeaveLedger(leaveLedgers, vacationLeaveTypeId, employeeId)
    '        leaveLedgerId = ll.RowID

    '        ll.LastTransaction = CreateCreditLeaveTransaction(context, employee, employeeId, leaveLedgerId, LeaveType.VacationLeave)
    '        ll.LastUpd = Date.Now
    '        vacationLeaveTransactions.Add(ll.LastTransaction)

    '        'Sick Leave
    '        ll = GetLeaveLedger(leaveLedgers, sickLeaveTypeId, employeeId)
    '        leaveLedgerId = ll.RowID

    '        ll.LastTransaction = CreateCreditLeaveTransaction(context, employee, employeeId, leaveLedgerId, LeaveType.SickLeave)
    '        ll.LastUpd = Date.Now
    '        sickLeaveTransactions.Add(ll.LastTransaction)

    '    Next

    '    Dim empProfiles = Await _employeeRepo.GetByMultipleIdAsync(employeeIds)

    '    For Each employee In empProfiles
    '        Dim eId = employee.RowID

    '        Dim lt = vacationLeaveTransactions.Where(Function(l) Nullable.Equals(l.EmployeeID, eId)).FirstOrDefault
    '        If lt IsNot Nothing Then
    '            employee.LeaveBalance = lt.Balance
    '        End If

    '        lt = sickLeaveTransactions.Where(Function(l) Nullable.Equals(l.EmployeeID, eId)).FirstOrDefault
    '        If lt IsNot Nothing Then
    '            employee.SickLeaveBalance = lt.Balance
    '        End If

    '    Next

    '    Await context.SaveChangesAsync()

    'End Using

    'End Function

    'Private Async Sub RemovePreviousLeaveCreditsAsync()
    '    Dim employeeIds = _employeeModels.Select(Function(e) e.RowID).ToList()

    '    Using context = New PayrollContext()
    '        Dim prevLeaveTrans = Await context.LeaveTransactions.
    '            Where(Function(lt) Nullable.Equals(lt.OrganizationID, z_OrganizationID)).
    '            Where(Function(lt) Nullable.Equals(lt.PayPeriodID, payPeriodId)).
    '            Where(Function(lt) employeeIds.Any(Function(id) Nullable.Equals(lt.EmployeeID, id))).
    '            Where(Function(lt) Equals(lt.Type, "Credit")).
    '            ToListAsync()

    '        context.LeaveTransactions.RemoveRange(prevLeaveTrans.ToArray())

    '        Await context.SaveChangesAsync()
    '    End Using
    'End Sub

    'Private Function CreateCreditLeaveTransaction(context As PayrollContext, employee As EmployeeModel, employeeId As Integer, leaveLedgerId As Integer, leaveType As LeaveType) As LeaveTransaction
    '    Dim lt As New LeaveTransaction With {
    '    .OrganizationID = z_OrganizationID,
    '    .Created = Date.Now,
    '    .CreatedBy = z_User,
    '    .LastUpd = Date.Now,
    '    .LastUpdBy = z_User,
    '    .EmployeeID = employeeId,
    '    .LeaveLedgerID = leaveLedgerId,
    '    .PayPeriodID = payPeriodId,
    '    .ReferenceID = Nothing,
    '    .TransactionDate = periodDateFrom,
    '    .Type = "Credit"
    '    }

    '    Dim _amount As Decimal
    '    If leaveType = LeaveType.VacationLeave Then
    '        _amount = employee.VacationLeaveAllowance
    '    ElseIf leaveType = LeaveType.SickLeave Then
    '        _amount = employee.SickLeaveAllowance
    '    End If

    '    lt.Balance = _amount
    '    lt.Amount = _amount

    '    context.LeaveTransactions.Add(lt)

    '    Return lt
    'End Function

    'Private Function GetLeaveLedger(leaveLedgers As List(Of LeaveLedger), leaveTypeId As Integer, employeeId As Integer) As LeaveLedger
    '    Dim leaveLedger = leaveLedgers.
    '                        Where(Function(ll) Nullable.Equals(ll.EmployeeID, employeeId)).
    '                        Where(Function(ll) Nullable.Equals(ll.ProductID, leaveTypeId)).FirstOrDefault

    '    If leaveLedger Is Nothing Then
    '        Using context = New PayrollContext()
    '            Dim lLedger As New LeaveLedger With {
    '                .Created = Date.Now,
    '                .CreatedBy = z_User,
    '                .EmployeeID = employeeId,
    '                .LastUpd = Date.Now,
    '                .LastUpdBy = z_User,
    '                .OrganizationID = z_OrganizationID,
    '                .ProductID = leaveTypeId
    '            }

    '            context.LeaveLedgers.Add(lLedger)

    '            context.SaveChanges()

    '            leaveLedger = lLedger
    '        End Using

    '        Return leaveLedger
    '    End If

    '    Return leaveLedger
    'End Function

#End Region

    Private Async Function LoadEmployees() As Threading.Tasks.Task
        Dim unemployedStatuses = New String() {"Resigned", "Terminated"}

        Dim activeEmployees = Await _employeeRepository.GetAllActiveAsync(z_OrganizationID)

        _employees = activeEmployees.
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst).
            ToList()

        _employeeModels = _employees.Select(Function(e) New EmployeeModel(e)).ToList()

        dgvEmployees.DataSource = _employeeModels
    End Function

    Private Enum LeaveType
        VacationLeave
        SickLeave
    End Enum

    Private Class EmployeeModel

        Private _employee As Employee

        Sub New(employee As Employee)
            _employee = employee
        End Sub

        Public ReadOnly Property RowID As Integer
            Get
                Return _employee.RowID.Value
            End Get
        End Property

        Public ReadOnly Property EmployeeNo As String
            Get
                Return _employee.EmployeeNo
            End Get
        End Property

        Public ReadOnly Property LastName As String
            Get
                Return _employee.LastName
            End Get
        End Property

        Public ReadOnly Property FirstName As String
            Get
                Return _employee.FirstName
            End Get
        End Property

        Public ReadOnly Property VacationLeaveAllowance As Decimal
            Get
                Return _employee.VacationLeaveAllowance
            End Get
        End Property

        Public ReadOnly Property VacationLeaveBalance As Decimal
            Get
                Return _employee.LeaveBalance
            End Get
        End Property

        Public ReadOnly Property SickLeaveAllowance As Decimal
            Get
                Return _employee.SickLeaveAllowance
            End Get
        End Property

        Public ReadOnly Property SickLeaveBalance As Decimal
            Get
                Return _employee.SickLeaveBalance
            End Get
        End Property

    End Class

    Private Class LeaveTypeModel

        Private _product As Product

        Sub New(product As Product)
            _product = product
        End Sub

        Public ReadOnly Property RowID As Integer
            Get
                Return _product.RowID.Value
            End Get
        End Property

        Public ReadOnly Property PartNo As String
            Get
                Return _product.PartNo
            End Get
        End Property

    End Class

End Class
