Imports AccuPay.DB

Public Class SystemOwner
    Inherits Reference.BaseSystemOwner

    Dim sql As New Sql("SELECT Name FROM systemowner WHERE IsCurrentOwner='1' LIMIT 1;")

    Private _currentSystemOwner As String = Convert.ToString(sql.GetFoundRow)

    Public Overrides ReadOnly Property CurrentSystemOwner As String
        Get
            Return _currentSystemOwner
        End Get

    End Property

End Class