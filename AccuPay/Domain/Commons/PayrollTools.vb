Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Entity
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore

Public Class PayrollTools

    Public Shared Sub UpdateLoanSchedule(paypRowID As Integer)

        Dim param_array = New Object() {orgztnID, paypRowID, z_User}

        Static strquery_recompute_13monthpay As String =
                        "call recompute_thirteenthmonthpay(?organizid, ?payprowid, ?userrowid);"

        Dim n_ExecSQLProcedure = New SQL(strquery_recompute_13monthpay, param_array)
        n_ExecSQLProcedure.ExecuteQuery()
    End Sub

    Public Shared Sub DeletePaystub(employeeId As Integer, payPeriodId As Integer)

        Dim n_ExecuteQuery As New ExecuteQuery("SELECT RowID" &
                                                       " FROM paystub" &
                                                       " WHERE EmployeeID='" & employeeId & "'" &
                                                       " AND OrganizationID='" & orgztnID & "'" &
                                                       " AND PayPeriodID='" & payPeriodId & "'" &
                                                       " LIMIT 1;")

        Dim paystubRowID As Object = Nothing

        paystubRowID = n_ExecuteQuery.Result

        If paystubRowID IsNot Nothing Then
            n_ExecuteQuery = New ExecuteQuery("CALL DEL_specificpaystub('" & paystubRowID.ToString & "');")
        End If

    End Sub

    Public Shared Async Function ValidatePayPeriodAction(payPeriodId As Integer?) As Task(Of Boolean)

        Dim sys_ownr As New SystemOwnerService()

        If sys_ownr.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then

            'Add temporarily. Consult maam mely first as she is still testing the system with multiple pay periods
            Return True

        End If

        Using context As New PayrollContext

            If payPeriodId Is Nothing Then
                MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
                Return False
            End If

            Dim payPeriod = Await New PayPeriodRepository().GetByIdAsync(payPeriodId.Value)

            If payPeriod Is Nothing Then
                MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
                Return False
            End If

            Dim otherProcessingPayPeriod = Await context.Paystubs.
                        Include(Function(p) p.PayPeriod).
                        Where(Function(p) p.PayPeriod.RowID.Value <> payPeriodId.Value).
                        Where(Function(p) p.PayPeriod.IsClosed = False).
                        Where(Function(p) p.PayPeriod.OrganizationID.Value = z_OrganizationID).
                        FirstOrDefaultAsync()

            If payPeriod.IsClosed Then

                MessageBoxHelper.Warning("The pay period you selected is already closed. Please reopen so you can alter the data for that pay period. If there are ""Processing"" pay periods, make sure to close them first.")
                Return False

            ElseIf Not payPeriod.IsClosed AndAlso otherProcessingPayPeriod IsNot Nothing Then

                MessageBoxHelper.Warning("There is currently a pay period with ""PROCESSING"" status. Please finish that pay period first then close it to process other open pay periods.")
                Return False

            End If
        End Using

        Return True

    End Function

    Public Shared Function GetOrganizationAddress() As String

        Dim str_quer_address As String =
            String.Concat("SELECT CONCAT_WS(', '",
                          ", IF(LENGTH(TRIM(ad.StreetAddress1)) = 0, NULL, ad.StreetAddress1)",
                          ", IF(LENGTH(TRIM(ad.StreetAddress2)) = 0, NULL, ad.StreetAddress2)",
                          ", IF(LENGTH(TRIM(ad.Barangay)) = 0, NULL, ad.Barangay)",
                          ", IF(LENGTH(TRIM(ad.CityTown)) = 0, NULL, ad.CityTown)",
                          ", IF(LENGTH(TRIM(ad.Country)) = 0, NULL, ad.Country)",
                          ", IF(LENGTH(TRIM(ad.State)) = 0, NULL, ad.State)",
                          ") `Result`",
                          " FROM organization og",
                          " LEFT JOIN address ad ON ad.RowID = og.PrimaryAddressID",
                          " WHERE og.RowID = ", orgztnID, ";")

        Return Convert.ToString(New SQL(str_quer_address).GetFoundRow)

    End Function

End Class