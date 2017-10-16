Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Auditing

    <Table("audittrail")>
    Public Class AuditTrail

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property OrganizationID As Integer?

        Public Property ViewID As Integer?

        Public Property FieldChanged As String

        Public Property ChangedRowID As Integer?

        Public Property OldValue As String

        Public Property NewValue As String

        Public Property ActionPerformed As String

    End Class

End Namespace
