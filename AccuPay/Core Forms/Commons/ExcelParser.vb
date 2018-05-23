Option Strict On

Imports System.IO
Imports System.Reflection
Imports AccuPay.Utils
Imports OfficeOpenXml

Namespace Global.Globagility.AccuPay.Salaries

    Public Class ExcelParser(Of T As {Class, New})

        Private ReadOnly _worksheetName As String

        Public Sub New(worksheetName As String)
            _worksheetName = worksheetName
        End Sub

        Public Function Read(filePath As String) As IList(Of T)
            Dim stream = GetFileContents(filePath)

            Dim records As List(Of T) = Nothing

            Using excel = New ExcelPackage(stream)
                Dim worksheet = excel.Workbook.Worksheets(_worksheetName)

                If worksheet Is Nothing Then
                    Throw New Exception($"We can't find the worksheet `{_worksheetName}`.")
                End If

                Dim tprops = GetType(T).GetProperties().ToList()

                Dim groups = worksheet.Cells.
                    GroupBy(Function(g) g.Start.Row).
                    ToList()

                Dim colNames = groups.
                    First().
                    Select(Function(col, index) New With {.Name = col.Value.ToString(), .Index = index}).
                    ToList()

                Dim rowValues = groups.
                    Skip(1).
                    Select(Function(cfg) cfg.Select(Function(g) g.Value).ToList())

                records = rowValues.
                    Select(
                        Function(row)
                            Dim newRecord = New T()

                            colNames.ForEach(
                                Sub(column)
                                    If column.Index >= row.Count Then
                                        Return
                                    End If

                                    Dim originalValue = row(column.Index)
                                    Dim prop = tprops.FirstOrDefault(
                                        Function(t)
                                            Return StringUtils.ToPascal(t.Name) = StringUtils.ToPascal(column.Name)
                                        End Function)

                                    If prop IsNot Nothing Then
                                        ParseValue(newRecord, prop, originalValue)
                                    End If
                                End Sub)

                            Return newRecord
                        End Function).
                    ToList()
            End Using

            Return records
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
                ElseIf prop.PropertyType Is GetType(Decimal) Then
                    prop.SetValue(newRecord, CDec(value))
                ElseIf prop.PropertyType Is GetType(Integer) Then
                    prop.SetValue(newRecord, CInt(value))
                ElseIf prop.PropertyType Is GetType(Date) Or prop.PropertyType Is GetType(Date?) Then
                    prop.SetValue(newRecord, Date.FromOADate(value))
                End If
            Else
                prop.SetValue(newRecord, originalValue)
            End If
        End Sub

    End Class

End Namespace
