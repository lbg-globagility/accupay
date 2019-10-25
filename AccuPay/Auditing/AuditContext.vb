Imports Microsoft.EntityFrameworkCore

Namespace Auditing

    Public Class AuditContext
        Inherits DbContext

        Public Sub New()
        End Sub

        Public Overridable Property Views As DbSet(Of View)

        Public Overridable Property AuditTrails As DbSet(Of AuditTrail)

    End Class

End Namespace