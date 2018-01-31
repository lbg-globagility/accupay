Public Class ExcelWriter
    Private stream As IO.Stream
    Private writer As IO.BinaryWriter

    Private clBegin As UShort() = {&H809, 8, 0, &H10, 0, 0}
    Private clEnd As UShort() = {&HA, 0}


    Private Sub WriteUshortArray(value As UShort())
        For i As Integer = 0 To value.Length - 1
            writer.Write(value(i))
        Next
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="ExcelWriter"/> class.
    ''' </summary>
    ''' <param name="stream">The stream.</param>
    Public Sub New(stream As IO.Stream)
        Me.stream = stream
        writer = New IO.BinaryWriter(stream)
    End Sub

    ''' <summary>
    ''' Writes the text cell value.
    ''' </summary>
    ''' <param name="row">The row.</param>
    ''' <param name="col">The col.</param>
    ''' <param name="value">The string value.</param>
    Public Sub WriteCell(row As Integer, col As Integer, value As String)
        Dim clData As UShort() = {&H204, 0, 0, 0, 0, 0}
        Dim iLen As Integer = value.Length
        Dim plainText As Byte() = System.Text.Encoding.ASCII.GetBytes(value)
        clData(1) = CUShort(8 + iLen)
        clData(2) = CUShort(row)
        clData(3) = CUShort(col)
        clData(5) = CUShort(iLen)
        WriteUshortArray(clData)
        writer.Write(plainText)
    End Sub

    ''' <summary>
    ''' Writes the integer cell value.
    ''' </summary>
    ''' <param name="row">The row number.</param>
    ''' <param name="col">The column number.</param>
    ''' <param name="value">The value.</param>
    Public Sub WriteCell(row As Integer, col As Integer, value As Integer)
        Dim clData As UShort() = {&H27E, 10, 0, 0, 0}
        clData(2) = CUShort(row)
        clData(3) = CUShort(col)
        WriteUshortArray(clData)
        Dim iValue As Integer = (value << 2) Or 2
        writer.Write(iValue)
    End Sub

    ''' <summary>
    ''' Writes the double cell value.
    ''' </summary>
    ''' <param name="row">The row number.</param>
    ''' <param name="col">The column number.</param>
    ''' <param name="value">The value.</param>
    Public Sub WriteCell(row As Integer, col As Integer, value As Double)
        Dim clData As UShort() = {&H203, 14, 0, 0, 0}
        clData(2) = CUShort(row)
        clData(3) = CUShort(col)
        WriteUshortArray(clData)
        writer.Write(value)
    End Sub

    ''' <summary>
    ''' Writes the empty cell.
    ''' </summary>
    ''' <param name="row">The row number.</param>
    ''' <param name="col">The column number.</param>
    Public Sub WriteCell(row As Integer, col As Integer)
        Dim clData As UShort() = {&H201, 6, 0, 0, &H17}
        clData(2) = CUShort(row)
        clData(3) = CUShort(col)
        WriteUshortArray(clData)
    End Sub

    ''' <summary>
    ''' Must be called once for creating XLS file header
    ''' </summary>
    Public Sub BeginWrite()
        WriteUshortArray(clBegin)
    End Sub

    ''' <summary>
    ''' Ends the writing operation, but do not close the stream
    ''' </summary>
    Public Sub EndWrite()
        WriteUshortArray(clEnd)
        writer.Flush()
    End Sub

End Class
