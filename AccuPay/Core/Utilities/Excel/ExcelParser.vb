Option Strict On

Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports AccuPay
Imports AccuPay.Attributes
Imports AccuPay.Helpers
Imports AccuPay.Utilities
Imports AccuPay.Utils
Imports OfficeOpenXml

Namespace Global.Globagility.AccuPay

    Public Class ExcelParser(Of T As {IExcelRowRecord, New})

        Public Shared Function Parse(Optional workSheetName As String = Nothing) As ExcelParseOutput(Of T)

            Dim browseFileOut = OpenFileDialogImportHelper.BrowseFile()

            If browseFileOut.IsSuccess = False Then Return ExcelParseOutput(Of T).Failed()

            Dim parser = New ExcelParser(Of T)(workSheetName)
            Dim records = parser.Read(browseFileOut.FileName)

            Return ExcelParseOutput(Of T).Success(records)

        End Function

        Private ReadOnly _worksheetName As String

        Public Sub New()
        End Sub

        Public Sub New(worksheetName As String)
            _worksheetName = worksheetName
        End Sub

        Public Function Read(filePath As String) As IList(Of T)

            If Path.GetExtension(filePath) = ".xls" Then
                Dim tempFileName = Path.GetTempFileName() + ".xlsx"

                Dim app = New Microsoft.Office.Interop.Excel.Application()
                Dim workbook = app.Workbooks.Open(filePath)
                workbook.SaveAs(Filename:=tempFileName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook)
                workbook.Close()
                app.Quit()
                filePath = tempFileName
            End If

            Dim stream = GetFileContents(filePath)

            Dim records As New List(Of T)

            Using excel = New ExcelPackage(stream)
                Dim worksheet As ExcelWorksheet

                If _worksheetName Is Nothing Then
                    worksheet = excel.Workbook.Worksheets(1)
                Else
                    worksheet = excel.Workbook.Worksheets(_worksheetName)
                End If

                If worksheet Is Nothing Then
                    Throw New WorkSheetNotFoundException($"We can't find the worksheet{If(String.IsNullOrWhiteSpace(_worksheetName), "", $"`{_worksheetName}`")}.")
                End If

                Dim tprops = GetType(T).GetProperties().ToList()

                Dim groups = worksheet.Cells.
                    GroupBy(Function(g) g.Start.Row).
                    ToList()

                If groups.Count < 1 Then

                    Throw New WorkSheetIsEmptyException()

                End If

                Dim columns = groups.
                    First().
                    Select(Function(col, index) New Column(col, index)).
                    ToList()

                Dim rows = GetRows(worksheet, columns)

                Dim rowIndex = 1 'starts at 1 because of the headers
                For Each row In rows

                    rowIndex += 1

                    Dim newRow = ParseRow(row, columns, tprops)
                    If newRow Is Nothing Then Continue For

                    newRow.LineNumber = rowIndex
                    records.Add(newRow)
                Next
            End Using

            Return records
        End Function

        Private Function ParseRow(row As List(Of Object), colNames As IList(Of Column), tprops As List(Of PropertyInfo)) As T
            Dim newRecord = New T()

            Dim rowIsBlank As Boolean = True

            For Each column In colNames
                If column.Index >= row.Count Then
                    Continue For
                End If

                Dim originalValue = row(column.Index)

                If String.IsNullOrWhiteSpace(ObjectUtils.ToStringOrNull(originalValue)) = False Then
                    rowIsBlank = False
                End If

                Dim prop As PropertyInfo

                'Check by Property Attribute Name
                prop = tprops.FirstOrDefault(
                    Function(t)
                        If Attribute.IsDefined(t, GetType(ColumnNameAttribute)) = False Then
                            Return False
                        End If

                        Dim attr = CType(t.GetCustomAttributes(GetType(ColumnNameAttribute), False), ColumnNameAttribute())

                        Return StringUtils.ToPascal(attr(0).Value) = StringUtils.ToPascal(column.Name)
                    End Function)

                If prop Is Nothing Then
                    'Check by Property Name
                    prop = tprops.FirstOrDefault(
                        Function(t)
                            Return StringUtils.ToPascal(t.Name) = StringUtils.ToPascal(column.Name)
                        End Function)
                End If

                If prop IsNot Nothing Then
                    If Attribute.IsDefined(prop, GetType(IgnoreAttribute)) Then
                        Continue For
                    End If

                    ParseValue(newRecord, prop, originalValue)
                End If
            Next

            Return If(rowIsBlank, Nothing, newRecord)

        End Function

        Private Function GetRows(worksheet As ExcelWorksheet, columns As ICollection(Of Column)) As List(Of List(Of Object))
            Dim firstRow = worksheet.Dimension.Start.Row + 1
            Dim endRow = worksheet.Dimension.End.Row

            Dim rows = New List(Of List(Of Object))

            For i = firstRow To endRow
                Dim row = New List(Of Object)

                For Each column In columns
                    row.Add(worksheet.Cells($"{column.Letter}{i}").Value)
                Next

                rows.Add(row)
            Next

            Return rows
        End Function

        Private Function GetFileContents(filePath As String) As Stream
            Dim contents = New MemoryStream()

            Using fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                fileStream.CopyTo(contents)
            End Using

            Return contents
        End Function

        Private Sub ParseValue(newRecord As T, prop As PropertyInfo, originalValue As Object)

            If TypeOf originalValue Is Double Then
                Dim value = DirectCast(originalValue, Double)

                If prop.PropertyType Is GetType(Double) Then
                    prop.SetValue(newRecord, value)
                ElseIf prop.PropertyType Is GetType(Double?) Then
                    prop.SetValue(newRecord, ObjectUtils.ToNullableDouble(value))

                ElseIf prop.PropertyType Is GetType(Decimal) Then
                    prop.SetValue(newRecord, CDec(value))
                ElseIf prop.PropertyType Is GetType(Decimal?) Then
                    prop.SetValue(newRecord, ObjectUtils.ToNullableDecimal(value))

                ElseIf prop.PropertyType Is GetType(Integer) Then
                    prop.SetValue(newRecord, CInt(value))
                ElseIf prop.PropertyType Is GetType(Integer?) Then
                    prop.SetValue(newRecord, ObjectUtils.ToNullableInteger(value))

                ElseIf prop.PropertyType Is GetType(Date) Or prop.PropertyType Is GetType(Date?) Then
                    prop.SetValue(newRecord, Date.FromOADate(value))

                ElseIf prop.PropertyType Is GetType(String) Then
                    prop.SetValue(newRecord, CStr(value))

                ElseIf prop.PropertyType Is GetType(TimeSpan) Or prop.PropertyType Is GetType(TimeSpan?) Then
                    prop.SetValue(newRecord, Date.FromOADate(value).TimeOfDay)
                End If
            Else
                If prop.PropertyType Is GetType(Date) Then
                    Dim dateInput = ObjectUtils.ToDateTime(originalValue)
                    If dateInput <> Date.MinValue AndAlso Date.MinValue.ToString <> originalValue?.ToString Then
                        prop.SetValue(newRecord, dateInput)
                    End If

                ElseIf prop.PropertyType Is GetType(Date?) Then
                    prop.SetValue(newRecord, ObjectUtils.ToNullableDateTime(originalValue))

                ElseIf prop.PropertyType Is GetType(TimeSpan) Then
                    prop.SetValue(newRecord, ObjectUtils.ToTimeSpan(originalValue))

                ElseIf prop.PropertyType Is GetType(TimeSpan?) Then
                    prop.SetValue(newRecord, ObjectUtils.ToNullableTimeSpan(originalValue))
                Else

                    prop.SetValue(newRecord, originalValue)

                End If
            End If
        End Sub

        Private Class Column

            Public ReadOnly Property Letter As String

            Public ReadOnly Property Name As String

            Public ReadOnly Property Index As Integer

            Public Sub New(cell As ExcelRangeBase, index As Integer)
                Me.Letter = GetLettersOnly(cell.Address)
                Me.Name = cell.Value?.ToString()
                Me.Index = index
            End Sub

            Private Function GetLettersOnly(address As String) As String
                Return Regex.Replace(address, "[\d-]", String.Empty)
            End Function

        End Class

        Public Class ExcelParseOutput(Of E As {IExcelRowRecord, New})

            Property IsSuccess As Boolean
            Property Records As IList(Of E)

            Public Shared Function Success(records As IList(Of E)) As ExcelParseOutput(Of E)

                Return New ExcelParseOutput(Of E)(True, records)

            End Function

            Public Shared Function Failed() As ExcelParseOutput(Of E)

                Return New ExcelParseOutput(Of E)(False, Nothing)

            End Function

            Private Sub New(isSuccess As Boolean, records As IList(Of E))

                Me.IsSuccess = isSuccess
                Me.Records = records

            End Sub

        End Class

    End Class

End Namespace