Imports System.Threading.Tasks
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class CashOutUnusedLeave

    Private Const _defaultWorkHours As Decimal = 8.0

    Private _settings As ListOfValueCollection

    'Private _adjUnusedVacationLeave As String = String.Join(Space(1), "Unused", LeaveType.Vacation.ToString(), "Leaves")
    Private _adjUnusedVacationLeave As String = String.Join(Space(1), "Unused", "Leaves")

    Private _adjUnusedSickLeave As String = String.Join(Space(1), "Unused", LeaveType.Sick.ToString(), "Leaves")

    Private _isVLOnly, _isSLOnly, _asAdjustment As Boolean

    Private _payPeriodFromId,
        _payPeriodToId,
        _adjUnusedVacationLeaveId,
        _adjUnusedSickLeaveId,
        _currentPeriodId,
        _organizationId,
        _vacationLeaveId,
        _sickLeaveId As Integer

    Private _leaveLedger As DataTable
    Private _categoryRepository As CategoryRepository
    Private Const strAdjType = "Adjustment Type"

    Private Const strLeaveType = "Leave Type"

    Private _employeeRepository As EmployeeRepository

    Private _listOfValRepository As ListOfValueRepository

    Public Sub New(PayPeriodFromId As Integer,
                   PayPeriodToId As Integer,
                   CurrentPeriodID As Integer)

        _categoryRepository = New CategoryRepository

        _employeeRepository = New EmployeeRepository

        _organizationId = Convert.ToInt32(z_OrganizationID)

        Dim listOfValues = _listOfValRepository.GetLeaveConvertiblePolicies()

        Dim _leaveType = listOfValues.Where(Function(l) Equals(l.LIC, "LeaveType")).FirstOrDefault

        _isVLOnly = _leaveType.DisplayValue = LeaveType.Vacation.ToString()

        _isSLOnly = _leaveType.DisplayValue = LeaveType.Sick.ToString()

        Dim _amountTreatment = listOfValues.Where(Function(l) Equals(l.LIC, "AmountTreatment")).FirstOrDefault

        _asAdjustment = _amountTreatment.DisplayValue = AmountTreatment.Adjustment.ToString()

        'Dim policy = _settings.GetSublist("LeaveConvertiblePolicy")

        '_isVLOnly = policy.GetValue("LeaveType") = LeaveType.Vacation.ToString()

        '_isSLOnly = policy.GetValue("LeaveType") = LeaveType.Sick.ToString()

        '_asAdjustment = policy.GetValue("AmountTreatment") = AmountTreatment.Adjustment.ToString()

        _payPeriodFromId = PayPeriodFromId
        _payPeriodToId = PayPeriodToId
        _currentPeriodId = CurrentPeriodID

        LoadAdjUnusedVacationLeaveId()

        LoadVacationLeaveId()

    End Sub

    Private Async Sub LoadVacationLeaveId()
        _vacationLeaveId = Await GetVacationLeaveId()
    End Sub

    Private Async Sub LoadAdjUnusedVacationLeaveId()
        _adjUnusedVacationLeaveId = Await GetAdjUnusedVacationLeaveId()
    End Sub

    Private Function GetLatestLeaveLedger() As DataTable
        Dim query1 = $"CALL RPT_LeaveConvertibles(@orgId, @leaveTypeId, @payPeriodFromId, @payPeriodToId, NULL);"

        Dim leaveTypeId? As Integer = 0

        Dim params = New Dictionary(Of String, Object) From {
            {"@orgId", _organizationId},
            {"@leaveTypeId", If(_isVLOnly, _vacationLeaveId, DBNull.Value)},
            {"@payPeriodFromId", _payPeriodFromId},
            {"@payPeriodToId", _payPeriodToId}
        }

        Dim query = New SqlToDataTable(query1, params)

        Return query.Read()
    End Function

    Private Async Function GetVacationLeaveId() As Threading.Tasks.Task(Of Integer)
        Dim value As Integer
        Dim strVacationLeave = String.Join(Space(1), LeaveType.Vacation.ToString(), "Leave")

        Dim leaveCategory = Await _categoryRepository.GetByNameAsync(z_OrganizationID, strVacationLeave)
        Dim leaveCategoryId = leaveCategory.RowID

        Using context = New PayrollContext()

            Dim product = context.Products.
                Where(Function(p) Equals(p.PartNo.ToLower, strVacationLeave.ToLower) _
                       And Equals(p.OrganizationID.Value, _organizationId) _
                       And Equals(p.CategoryID, leaveCategoryId) _
                       And Equals(p.Category, strLeaveType)).FirstOrDefault

            value = product.RowID
        End Using

        Return value
    End Function

    Private Async Function GetAdjUnusedVacationLeaveId() As Threading.Tasks.Task(Of Integer)
        Dim value As Integer

        Dim adjustmentCategory = Await _categoryRepository.GetByNameAsync(z_OrganizationID, strAdjType)
        Dim adjustmentCategoryId = adjustmentCategory.RowID

        Using context = New PayrollContext()
            Dim product = context.Products.
                Where(Function(p) Equals(p.PartNo.ToLower, _adjUnusedVacationLeave.ToLower) _
                       And Equals(p.OrganizationID.Value, _organizationId) _
                       And Equals(p.CategoryID, adjustmentCategoryId) _
                       And Equals(p.Category, strAdjType)).FirstOrDefault

            If product Is Nothing Then
                value = CreateProduct(context, _adjUnusedVacationLeave, strAdjType, adjustmentCategoryId)
            Else
                value = product.RowID
            End If

        End Using

        Return value
    End Function

    Private Async Function GetAdjUnusedSickLeaveId() As Threading.Tasks.Task(Of Integer)
        Dim value As Integer

        Dim adjustmentCategory = Await _categoryRepository.GetByNameAsync(z_OrganizationID, strAdjType)
        Dim adjustmentCategoryId = adjustmentCategory.RowID

        Using context = New PayrollContext()
            Dim product = context.Products.
                Where(Function(p) Equals(p.PartNo.ToLower, _adjUnusedSickLeave.ToLower) _
                       And Equals(p.OrganizationID.Value, _organizationId) _
                       And Equals(p.CategoryID, adjustmentCategoryId) _
                       And Equals(p.Category, strAdjType)).FirstOrDefault

            If product Is Nothing Then
                value = CreateProduct(context, _adjUnusedSickLeave, strAdjType, adjustmentCategoryId)
            Else
                value = product.RowID
            End If

        End Using

        Return value
    End Function

    Private Function CreateProduct(context As PayrollContext, ProductName As String, CategoryName As String, CategoryRowId As Integer) As Integer
        Dim product As New Product With {
            .Name = ProductName,
            .OrganizationID = _organizationId,
            .Description = String.Empty,
            .PartNo = ProductName,
            .Created = Date.Now,
            .LastUpd = Date.Now,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Category = CategoryName,
            .CategoryID = CategoryRowId,
            .Status = "Active",
            .Fixed = False
        }

        context.Products.Add(product)

        context.SaveChanges()

        Return product.RowID
    End Function

    Public Sub Execute()
        _leaveLedger = GetLatestLeaveLedger()

        Dim payperiod As New PayPeriod

        If _isVLOnly And _asAdjustment Then

            Dim success As Boolean = False

            Using context = New PayrollContext()
                Dim paystubs =
                    (From p In context.Paystubs.Include(Function(p) p.ActualAdjustments)
                     Where p.PayPeriodID.Value = _currentPeriodId And p.OrganizationID = _organizationId).
                     ToList()

                payperiod = context.PayPeriods.Where(Function(pp) Equals(pp.RowID.Value, _currentPeriodId)).FirstOrDefault

                For Each row As DataRow In _leaveLedger.Rows
                    Dim employeePrimKey = Convert.ToInt32(row("EmployeeID"))
                    Dim llRowId = Convert.ToInt32(row("RowID"))

                    Dim ll As New LeaveLedger With {.RowID = llRowId, .EmployeeID = employeePrimKey}

                    Dim unusedLeaveAmount = Convert.ToDecimal(row("Balance")) * Convert.ToDecimal(row("HourlyRate"))

                    Dim leaveBalance = Convert.ToDecimal(row("Balance"))
                    Dim leaveDayBalance = leaveBalance / _defaultWorkHours

                    Dim paystub = paystubs.Where(Function(p) Equals(p.EmployeeID.Value, employeePrimKey)).FirstOrDefault

                    If paystub IsNot Nothing Then
                        Dim adjustmentsExceptUnusedLeave =
                            paystub.ActualAdjustments.Where(Function(a) Equals(a.ProductID.Value, _adjUnusedVacationLeaveId) = False)

                        paystub.ActualAdjustments.Clear()

                        Dim aa As New ActualAdjustment With {
                        .Created = Date.Now,
                        .Comment = String.Concat(leaveDayBalance, If(leaveDayBalance > 1, " days", " day")),
                        .CreatedBy = z_User,
                        .IsActual = True,
                        .LastUpd = Date.Now,
                        .LastUpdBy = z_User,
                        .OrganizationID = _organizationId,
                        .PayAmount = unusedLeaveAmount,
                        .PayStubID = paystub.RowID,
                        .ProductID = _adjUnusedVacationLeaveId
                        }

                        paystub.ActualAdjustments.Add(aa)

                        For Each adj In adjustmentsExceptUnusedLeave
                            paystub.ActualAdjustments.Add(adj)
                        Next

                        CreateLeaveTransaction(context, LeaveTransactionType.Debit, ll, payperiod, leaveBalance)
                    Else

                        Continue For
                    End If
                Next

                context.SaveChanges()
                success = True
            End Using

            If success Then
                Dim strCutOff As String
                Using context = New PayrollContext()
                    If payperiod Is Nothing Then
                        payperiod = context.PayPeriods.Where(Function(pp) Equals(pp.RowID.Value, _currentPeriodId)).FirstOrDefault
                    End If

                    strCutOff = String.Join(" to ", payperiod.PayFromDate.ToShortDateString, payperiod.PayToDate.ToShortDateString)
                End Using
                MsgBox(String.Concat("Unused leaves were successfully computed.", vbNewLine, "Please generate the ", strCutOff, " payroll."), MsgBoxStyle.Information)
            End If
        Else

        End If

    End Sub

    Private Async Sub ZeroOutEmployeeLeaveBalance(context As PayrollContext, employeeRowId As Integer)

        Dim employee = Await _employeeRepository.GetByIdAsync(employeeRowId)

        If employee IsNot Nothing Then
            With employee
                .LeaveBalance = 0
                .SickLeaveBalance = 0
            End With

        End If

    End Sub

    Private Function CreateLeaveTransaction(context As PayrollContext, leaveTransactionType As LeaveTransactionType, leaveLedger As LeaveLedger, payPeriod As PayPeriod, unusedLeaveHours As Decimal) As LeaveTransaction
        Dim employeeRowId = leaveLedger.EmployeeID

        Dim lt = context.LeaveTransactions.
            Where(Function(lTrans) Nullable.Equals(lTrans.OrganizationID, z_OrganizationID)).
            Where(Function(lTrans) Nullable.Equals(lTrans.EmployeeID, employeeRowId)).
            Where(Function(lTrans) Nullable.Equals(lTrans.LeaveLedgerID, leaveLedger.RowID)).
            Where(Function(lTrans) Nullable.Equals(lTrans.PayPeriodID, payPeriod.RowID)).
            Where(Function(lTrans) Equals(lTrans.Type, leaveTransactionType.ToString())).
            Where(Function(lTrans) Equals(lTrans.Balance, 0)).
            Where(Function(lTrans) Equals(lTrans.Amount, unusedLeaveHours)).
            Where(Function(lTrans) Equals(lTrans.TransactionDate, payPeriod.PayToDate)).
            FirstOrDefault

        If lt Is Nothing Then
            lt = New LeaveTransaction With {
                .OrganizationID = z_OrganizationID,
                .Created = Date.Now,
                .CreatedBy = z_User,
                .LastUpd = Date.Now,
                .LastUpdBy = z_User,
                .EmployeeID = employeeRowId,
                .LeaveLedgerID = leaveLedger.RowID,
                .PayPeriodID = payPeriod.RowID,
                .ReferenceID = Nothing,
                .TransactionDate = payPeriod.PayToDate,
                .Type = leaveTransactionType.ToString(),
                .Balance = 0,
                .Amount = unusedLeaveHours
                }

            context.LeaveTransactions.Add(lt)

            ZeroOutEmployeeLeaveBalance(context, employeeRowId)

        End If

        Return lt
    End Function

    Enum LeaveTransactionType
        Credit
        Debit
    End Enum

    Enum LeaveType
        Vacation
        Sick
        All
    End Enum

    Enum AmountTreatment
        Adjustment
        Gross
    End Enum

End Class