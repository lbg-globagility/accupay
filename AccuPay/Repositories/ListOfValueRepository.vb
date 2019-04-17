Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class ListOfValueRepository

        Public Async Function GetDeductionSchedules() _
            As Task(Of IEnumerable(Of ListOfValue))

            Return Await GetListOfValues("Government deduction schedule")

        End Function

        Public Async Function GetDutyShiftPolicies() _
            As Task(Of IEnumerable(Of ListOfValue))

            Return Await GetListOfValues("DutyShift")

        End Function

        Public Async Function GetShiftPolicies() _
            As Task(Of IEnumerable(Of ListOfValue))

            Return Await GetListOfValues("ShiftPolicy")

        End Function

#Region "Private Functions"
        Private Async Function GetListOfValues(type As String) _
            As Task(Of IEnumerable(Of ListOfValue))

            Using context = New PayrollContext()

                Dim listOfValues = Await context.ListOfValues.
                                Where(Function(l) l.Type = type).
                                Where(Function(l) l.Active = "Yes").
                                ToListAsync

                Return listOfValues

            End Using

        End Function

        Public Function ConvertToStringList(listOfValues As IEnumerable(Of ListOfValue), Optional columnName As String = "DisplayValue") _
            As List(Of String)

            Dim stringList As List(Of String)
            stringList = New List(Of String)

            For Each listOfValue In listOfValues

                Select Case columnName
                    Case "LIC"
                        stringList.Add(listOfValue.LIC)

                    Case Else
                        stringList.Add(listOfValue.DisplayValue)
                End Select
            Next

            Return stringList

        End Function
#End Region

    End Class

End Namespace
