Option Strict On

Imports System.IO
Imports AccuPay.Core.Entities

Public Interface IBankExporter

    Function Export(paystubs As IList(Of Paystub)) As Stream

End Interface