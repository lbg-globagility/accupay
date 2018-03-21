Imports System.IO

Public Class DebugUtility

    Public Shared Sub DumpTable(dataTable As DataTable, fileName As String)
        System.IO.Directory.CreateDirectory("logs")
        dataTable.TableName = fileName
        Using file = New FileStream($"{fileName}.xml", FileMode.Create)
            dataTable.WriteXml(file)
        End Using
    End Sub

End Class
