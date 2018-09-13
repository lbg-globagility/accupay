Option Strict On

Imports System.IO
Imports AccuPay
Imports AccuPay.Entity

Public Class BDOBankExporter
    Implements IBankExporter

    Public Function Export(paystubs As IList(Of Paystub)) As Stream Implements IBankExporter.Export
        Dim stream = New MemoryStream()

        Using writer = New StreamWriter(stream, System.Text.Encoding.UTF8, 1024, True)
            For Each paystub In paystubs
                Dim employee = paystub.Employee
                Dim atmNo = employee.AtmNo
                Dim netPay = paystub.NetPay.ToString()

                Dim line = $"{atmNo.PadLeft(13, " "c)}    {netPay.PadLeft(9, " "c)}"

                writer.WriteLine(line)
            Next

            writer.Flush()
        End Using

        stream.Seek(0, SeekOrigin.Begin)

        Return stream
    End Function

End Class
