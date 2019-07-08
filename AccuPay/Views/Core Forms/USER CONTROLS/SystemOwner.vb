Public Class SystemOwner

    Const text_goldwings As String = "Goldwings"

    Const text_hyundai As String = "Hyundai"

    Const text_cinema2000 As String = "Cinema 2000"

    Const text_benchmark As String = "Benchmark"

    Const text_default As String = "Default"

    Dim sql As New SQL("SELECT Name FROM systemowner WHERE IsCurrentOwner='1' LIMIT 1;")

    Private _currentSystemOwner As String = Convert.ToString(sql.GetFoundRow)

    Public Shared ReadOnly Property Goldwings As String
        Get
            Return text_goldwings
        End Get
    End Property

    Public Shared ReadOnly Property Hyundai As String
        Get
            Return text_hyundai
        End Get
    End Property

    Public Shared ReadOnly Property Cinema2000 As String
        Get
            Return text_cinema2000
        End Get
    End Property

    Public Shared ReadOnly Property Benchmark As String
        Get
            Return text_benchmark
        End Get
    End Property

    Public Shared ReadOnly Property DefaultOwner As String
        Get
            Return text_default
        End Get
    End Property

    Public ReadOnly Property CurrentSystemOwner As String
        Get
            Return _currentSystemOwner
        End Get

    End Property

End Class