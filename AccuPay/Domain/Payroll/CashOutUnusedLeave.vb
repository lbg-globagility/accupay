
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports MySql.Data.MySqlClient

Public Class CashOutUnusedLeave

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

    Private Const strAdjType = "Adjustment Type"

    Private Const strLeaveType = "Leave Type"

    Public Sub New(PayPeriodFromId As Integer,
                   PayPeriodToId As Integer,
                   CurrentPeriodID As Integer)

        _organizationId = Convert.ToInt32(z_OrganizationID)

        Using context = New PayrollContext()
            Dim listOfValues = context.ListOfValues.Where(Function(l) Equals(l.Type, "LeaveConvertiblePolicy")).ToList()

            Dim _leaveType = listOfValues.Where(Function(l) Equals(l.LIC, "LeaveType")).FirstOrDefault

            _isVLOnly = _leaveType.DisplayValue = LeaveType.Vacation.ToString()

            _isSLOnly = _leaveType.DisplayValue = LeaveType.Sick.ToString()


            Dim _amountTreatment = listOfValues.Where(Function(l) Equals(l.LIC, "AmountTreatment")).FirstOrDefault

            _asAdjustment = _amountTreatment.DisplayValue = AmountTreatment.Adjustment.ToString()

        End Using

        'Dim policy = _settings.GetSublist("LeaveConvertiblePolicy")

        '_isVLOnly = policy.GetValue("LeaveType") = LeaveType.Vacation.ToString()

        '_isSLOnly = policy.GetValue("LeaveType") = LeaveType.Sick.ToString()

        '_asAdjustment = policy.GetValue("AmountTreatment") = AmountTreatment.Adjustment.ToString()

        _payPeriodFromId = PayPeriodFromId
        _payPeriodToId = PayPeriodToId
        _currentPeriodId = CurrentPeriodID

        _adjUnusedVacationLeaveId = GetAdjUnusedVacationLeaveId()

        _vacationLeaveId = GetVacationLeaveId()

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

    Private Function GetVacationLeaveId() As Integer
        Dim value As Integer
        Dim strVacationLeave = String.Join(Space(1), LeaveType.Vacation.ToString(), "Leave")

        Using context = New PayrollContext()
            Dim leaveCategoryId = context.Categories.
                Where(Function(c) Equals(c.OrganizationID, _organizationId) _
                And Equals(c.CategoryName, strLeaveType)).Select(Function(c) c.RowID).FirstOrDefault

            Dim product = context.Products.
                Where(Function(p) Equals(p.PartNo.ToLower, strVacationLeave.ToLower) _
                       And Equals(p.OrganizationID.Value, _organizationId) _
                       And Equals(p.CategoryID, leaveCategoryId) _
                       And Equals(p.Category, strLeaveType)).FirstOrDefault

            value = product.RowID
        End Using

        Return value
    End Function

    Private Function GetAdjUnusedVacationLeaveId() As Integer
        Dim value As Integer

        Using context = New PayrollContext()
            Dim adjustmentCategoryId = context.Categories.
                Where(Function(c) Equals(c.OrganizationID, _organizationId) _
                And Equals(c.CategoryName, strAdjType)).Select(Function(c) c.RowID).FirstOrDefault

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

    Private Function GetAdjUnusedSickLeaveId() As Integer
        Dim value As Integer

        Using context = New PayrollContext()
            Dim adjustmentCategoryId = context.Categories.
                Where(Function(c) Equals(c.OrganizationID, _organizationId) _
                And Equals(c.CategoryName, strAdjType)).Select(Function(c) c.RowID).FirstOrDefault

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

        If _isVLOnly And _asAdjustment Then

            Dim success As Boolean = False

            Using context = New PayrollContext()
                Dim paystubs =
                    (From p In context.Paystubs.Include(Function(p) p.ActualAdjustments)
                     Where p.PayPeriodID.Value = _currentPeriodId And p.OrganizationID = _organizationId).
                     ToList()

                For Each row As DataRow In _leaveLedger.Rows
                    Dim employeePrimKey = Convert.ToInt32(row("EmployeeID"))

                    Dim paystub = paystubs.Where(Function(p) Equals(p.EmployeeID.Value, employeePrimKey)).FirstOrDefault

                    If paystub IsNot Nothing Then
                        Dim adjustmentsExceptUnusedLeave =
                            paystub.ActualAdjustments.Where(Function(a) Equals(a.ProductID.Value, _adjUnusedVacationLeaveId) = False)

                        paystub.ActualAdjustments.Clear()

                        Dim unusedLeaveAmount = Convert.ToDecimal(row("Balance")) * Convert.ToDecimal(row("HourlyRate"))

                        Dim leaveBalance = Convert.ToDecimal(row("Balance"))

                        Dim aa As New ActualAdjustment With {
                        .Created = Date.Now,
                        .Comment = String.Concat(leaveBalance, If(leaveBalance > 1, " hours", " hour"), " balance"),
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
                    Dim payperiod = context.PayPeriods.Where(Function(pp) Equals(pp.RowID.Value, _currentPeriodId)).FirstOrDefault
                    strCutOff = String.Join(" to ", payperiod.PayFromDate.ToShortDateString, payperiod.PayToDate.ToShortDateString)
                End Using
                MsgBox(String.Concat("Unused leaves were successfully computed.", vbNewLine, "Please generate the ", strCutOff, " payroll."), MsgBoxStyle.Information)
            End If
        Else

        End If

    End Sub

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
