Option Strict On

Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Entity
Public Class TimeLogsTempOut
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property rowID As Integer

    Public Property TimeOut As TimeSpan?

    Public Property [DateOut] As DateTime?

    Public Property EmployeeID As Integer


End Class
