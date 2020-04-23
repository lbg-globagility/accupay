Option Strict On

Public Class WorkSheetRowParseValueException
    Inherits Exception

    Private ReadOnly _columnName As String
    Private ReadOnly _rowNumber As Integer
    Private ReadOnly _message As String
    Private ReadOnly _innerException As Exception

    Public ReadOnly Property RowNumber As Integer
        Get
            Return _rowNumber
        End Get
    End Property

    Public ReadOnly Property ColumnName As String
        Get
            Return _columnName
        End Get
    End Property

    Public Overloads ReadOnly Property Message As String
        Get
            Return _message
        End Get
    End Property

    Public Overloads ReadOnly Property InnerException As Exception
        Get
            Return _innerException
        End Get
    End Property

    Public Sub New(innerException As Exception, columnName As String, rowNumber As Integer)
        'We needed to overload the base properties Message and InnerException since we can't call the base constructor.
        'We can't call the base constructor because we need to create the message first before calling the base constructor and that is prohibited.

        _columnName = columnName
        _rowNumber = rowNumber

        _message = $"Cannot parse value of column '{columnName}' on row {rowNumber}"
        _innerException = innerException

    End Sub

End Class