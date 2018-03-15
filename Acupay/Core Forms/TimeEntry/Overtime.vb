
Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("employeeovertime")>
Public Class Overtime

    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property Created As DateTime

    Public Property CreatedBy As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
    Public Property LastUpd As DateTime?

    Public Property EmployeeID As Integer?

    Public Property OTStartTime As TimeSpan?

    Public Property OTEndTime As TimeSpan?

    Public Property OTStartDate As Date

    Public Property OTEndDate As Date

    <NotMapped>
    Public Property RangeStart As Date

    <NotMapped>
    Public Property RangeEnd As Date

    Public Sub New()
    End Sub

End Class
