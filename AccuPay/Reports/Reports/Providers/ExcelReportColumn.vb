Public Class ExcelReportColumn

    Public Property Name As String
    Public Property Type As ColumnType
    Public Property Source As String
    Public Property [Optional] As Boolean

    Public Sub New(name As String,
                       source As String,
                       Optional type As ColumnType = ColumnType.Numeric,
                       Optional [optional] As Boolean = False)
        Me.Name = name
        Me.Source = source
        Me.Type = type
        Me.Optional = [optional]
    End Sub

    Public Enum ColumnType
        Text
        Numeric
    End Enum

End Class