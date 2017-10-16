Imports System
Imports System.Data.Entity
Imports System.Linq

Namespace Auditing

    Public Class AuditContext
        Inherits DbContext

        Public Sub New()
            MyBase.New("name=AuditContext")
        End Sub

        Public Overridable Property AuditTrails As DbSet(Of AuditTrail)

    End Class

End Namespace
