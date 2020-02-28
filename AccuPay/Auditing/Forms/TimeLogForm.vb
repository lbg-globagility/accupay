Option Strict On

Imports System.Collections.Generic
Imports System.ComponentModel
Imports AccuPay.Entity

Namespace Auditing

    Public Class TimeLogForm

        Private _view As View

        Private _payrollContext As PayrollContext

        Public Sub New()
            InitializeComponent()
            _payrollContext = New PayrollContext()
        End Sub

        Private Sub TimeLogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            InitializeForm()
            LoadView()
            LoadAuditDataGridView()
        End Sub

        Private Sub LoadView()
            Using context = New AuditContext()
                _view = context.Views.
                    Where(Function(v) v.ViewName = "Employee Time Entry logs").
                    Where(Function(v) CBool(v.OrganizationID = z_OrganizationID)).
                    First()
            End Using
        End Sub

        Private Sub InitializeForm()
            AuditDataGridView.AutoGenerateColumns = False
        End Sub

        Private Sub LoadAuditDataGridView()
            Dim auditTrailPerTimeLog As IEnumerable(Of IGrouping(Of String, AuditTrail))

            Using context = New AuditContext()
                Dim auditTrails = (From t In context.AuditTrails
                                   Where t.ViewID = _view.RowID).ToList()

                auditTrailPerTimeLog = auditTrails.GroupBy(Function(l) l.Created & l.ChangedRowID)
            End Using

            AuditDataGridView.DataSource = New BindingList(Of TimeLogView)(
                auditTrailPerTimeLog.Select(
                    Function(a) New TimeLogView(a, GetTimeLog(a.First().ChangedRowID))
                ).ToList()
            )
        End Sub

        Private Function GetTimeLog(timeLogID As Integer?) As TimeLog
            Return _payrollContext.TimeLogs.Find(timeLogID)
        End Function

        Private Class TimeLogView

            Private _logGroup As IGrouping(Of String, AuditTrail)

            Private _timeLog As TimeLog

            Public Sub New(logGroup As IGrouping(Of String, AuditTrail), timeLog As TimeLog)
                _logGroup = logGroup
                _timeLog = timeLog
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