Option Strict On

Imports System.IO
Imports AccuPay.Data.Entities

Public Interface IBankExporter

    Function Export(paystubs As IList(Of Paystub)) As Stream

End Interface