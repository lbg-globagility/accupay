
Imports MySql.Data.MySqlClient

Public Class CashOutUnusedLeave

    Private _settings As ListOfValueCollection

    Private adjUnusedVacationLeave As String = String.Join(Space(1), "Unused", LeaveType.Vacation.ToString())

    Private adjUnusedSickLeave As String = String.Join(Space(1), "Unused", LeaveType.Sick.ToString())

    Private isVLOnly As Boolean

    Private asAdjustment As Boolean

    Private _payPeriodFromId, _payPeriodToId As Integer

    Public Sub New(PayPeriodFromId As Integer,
                   PayPeriodToId As Integer)

        Dim policy = _settings.GetSublist("LeaveConvertiblePolicy")

        isVLOnly = policy.GetValue("LeaveType") = LeaveType.Vacation.ToString()

        asAdjustment = policy.GetValue("AmountTreatment") = AmountTreatment.Adjustment.ToString()

        _payPeriodFromId = PayPeriodFromId
        _payPeriodToId = PayPeriodToId
    End Sub

    Private Function GetLatestLeaveLedger() As DataTable
        Dim query1 = $"CALL CALL RPT_LeaveConvertibles(@orgId, @leaveTypeId, @payPeriodFromId, @payPeriodToId, NULL);"

        Dim leaveTypeId? As Integer = 0

        Dim params = New Dictionary(Of String, Object) From {
            {"@orgId", z_OrganizationID},
            {"@leaveTypeId", If(isVLOnly, 1, DBNull.Value)},
            {"@payPeriodFromId", _payPeriodFromId},
            {"@payPeriodToId", _payPeriodToId}
        }

        Dim query = New SqlToDataTable(query1, params)

        Return query.Read()
    End Function

    Private Function GetVacationLeaveId() As Integer
        Dim sql As New SQL("SELECT p.RowID FROM product p WHERE p.OrganizationID=@orgId AND p.PartNo='Vacation Leave';", New Object() {z_OrganizationID})

        Dim result = sql.GetFoundRow

        Return Convert.ToInt32(result)
    End Function

    Enum LeaveType
        Vacation
        Sick
    End Enum

    Enum AmountTreatment
        Adjustment
        Gross
    End Enum

End Class
