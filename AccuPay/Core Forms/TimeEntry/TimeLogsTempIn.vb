Option Strict On

Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Entity


Public Class TimeLogsTempIn

    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property rowID As Integer

    Public Property TimeIn As TimeSpan?

    Public Property [DateIn] As DateTime?

    Public Property EmployeeID As Integer

End Class
