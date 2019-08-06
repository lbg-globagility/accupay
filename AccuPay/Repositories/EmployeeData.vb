Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.ModelData

    Public Class EmployeeData

        Public Shared Async Function GetVacationLeaveBalance(employeeId As Integer?) As Task(Of Decimal)

            Return Await GetLeaveBalance(employeeId, ProductConstant.VACATION_LEAVE)

        End Function

        Public Shared Async Function GetSickLeaveBalance(employeeId As Integer?) As Task(Of Decimal)

            Return Await GetLeaveBalance(employeeId, ProductConstant.SICK_LEAVE)

        End Function


        Private Shared Async Function GetLeaveBalance(employeeId As Integer?, partNo As String) As Task(Of Decimal)

            Dim defaultBalance = 0

            Using context = New PayrollContext()

                Dim leaveledger = Await context.LeaveLedgers.
                                        Include(Function(l) l.Product).
                                        Where(Function(l) l.Product.PartNo = partNo).
                                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
                                        FirstOrDefaultAsync

                If leaveledger?.LastTransactionID Is Nothing Then Return defaultBalance


                Dim leaveTransaction = Await context.LeaveTransactions.
                                    Where(Function(l) Nullable.Equals(l.RowID, leaveledger.LastTransactionID)).
                                    FirstOrDefaultAsync

                If leaveTransaction?.Balance Is Nothing Then Return defaultBalance

                Return leaveTransaction.Balance

            End Using

        End Function

    End Class

End Namespace
