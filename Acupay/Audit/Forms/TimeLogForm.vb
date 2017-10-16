Option Strict On

Imports System.Collections.Generic
Imports System.ComponentModel

Namespace Auditing

    Public Class TimeLogForm

        Private _viewID As Integer = 124
        Private Shared _payrollContext As PayrollContext

        Private Sub TimeLogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            _payrollContext = New PayrollContext()

            Dim logGroups As IEnumerable(Of IGrouping(Of String, AuditTrail))

            Using context = New AuditContext()
                Dim logs = (From t In context.AuditTrails
                            Where t.ViewID = 124).ToList()

                logGroups = logs.GroupBy(Function(l) l.Created & l.ChangedRowID)
            End Using

            AuditDataGridView.AutoGenerateColumns = False
            AuditDataGridView.DataSource = New BindingList(Of TimeLogView)(
                logGroups.Select(Function(l) New TimeLogView(l)).ToList()
            )
        End Sub

        Private Shared Function GetTimeLog(timeLogID As Integer?) As TimeLog
            Return _payrollContext.TimeLogs.Find(timeLogID)
        End Function

        Private Class TimeLogView

            Private _logGroup As IGrouping(Of String, AuditTrail)

            Private _timeLog As TimeLog

            Public Sub New(logGroup As IGrouping(Of String, AuditTrail))
                _logGroup = logGroup
                _timeLog = GetTimeLog(_logGroup.First().ChangedRowID)
            End Sub

            Public ReadOnly Property DateOccurred As Date
                Get
                    Return _logGroup.First().Created
                End Get
            End Property

            Public ReadOnly Property Record As String
                Get
                    Return $"{_timeLog.LogDate.ToString("s")} {_timeLog.Employee.FirstName} {_timeLog.Employee.LastName}"
                End Get
            End Property

            Public ReadOnly Property Details As String
                Get
                    Dim output = String.Empty
                    Dim firstLog = _logGroup.First()

                    For Each log In _logGroup
                        If log IsNot firstLog Then
                            output &= vbCrLf
                        End If

                        output &= $"{log.FieldChanged}={log.NewValue}"
                    Next

                    Return output
                End Get
            End Property

        End Class

    End Class

End Namespace
