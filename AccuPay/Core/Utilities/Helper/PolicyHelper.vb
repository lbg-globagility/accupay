Option Strict On

Namespace Global.AccuPay.Helpers

    Public NotInheritable Class PolicyHelper

        Private _policy As TimeEntryPolicy

        Public Sub New()

            Using context = New PayrollContext()

                Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

                _policy = New TimeEntryPolicy(settings)

            End Using

        End Sub

        Public Function ComputeBreakTimeLate() As Boolean

            Return _policy.ComputeBreakTimeLate

        End Function

        Public Function UseShiftSchedule() As Boolean

            Return _policy.UseShiftSchedule

        End Function

        Public Function RespectDefaultRestDay() As Boolean

            Return _policy.RespectDefaultRestDay

        End Function

        Public Function ValidateLeaveBalance() As Boolean

            Return _policy.ValidateLeaveBalance

        End Function



    End Class

End Namespace
