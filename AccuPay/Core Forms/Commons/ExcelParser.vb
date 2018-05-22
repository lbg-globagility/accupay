Option Strict On

Imports System.IO
Imports AccuPay.Utils
Imports OfficeOpenXml

Namespace Global.AccuPay

    Public Class ExcelParser(Of T As {Class, New})

        Private _worksheetName As String

        Public Sub New(worksheetName As String)
            _worksheetName = worksheetName
        End Sub

        Public Sub Read(filePath As String)
            Dim stream = New MemoryStream()

            Using fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                fileStream.CopyTo(stream)
            End Using

            Dim salaryRecords As List(Of T) = Nothing

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

                salaryRecords = rowValues.
                    Select(
                        Function(row)
                            Dim newRecord = New T()

                            colNames.ForEach(
                                Sub(column)
                                    If column.Index >= row.Count Then
                                        Return
                                    End If

                                    Dim originalValue = row(column.Index)
                                    Dim prop = tprops.First(
                                        Function(t)
                                            Return StringUtils.ToPascal(t.Name) = StringUtils.ToPascal(column.Name)
                                        End Function)

                                    If TypeOf originalValue Is Double Then
                                        Dim value = DirectCast(originalValue, Double)

                                        If prop.PropertyType Is GetType(Double) Then
                                            prop.SetValue(newRecord, value)
                                        ElseIf prop.PropertyType Is GetType(Decimal) Then
                                            prop.SetValue(newRecord, CDec(value))
                                        ElseIf prop.PropertyType Is GetType(Integer) Then
                                            prop.SetValue(newRecord, CInt(value))
                                        ElseIf prop.PropertyType Is GetType(Date) Then
                                            prop.SetValue(newRecord, Date.FromOADate(value))
                                        End If
                                    Else
                                        prop.SetValue(newRecord, originalValue)
                                    End If
                                End Sub)

                            Return newRecord
                        End Function).
                    ToList()
            End Using
        End Sub

    End Class

End Namespace
