Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.JobLevels

Namespace Global.AccuPay.Entity

    <Table("position")>
    Public Class Position

        <Key>
        Public Property RowID As Integer?

        Public Property Name As String

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property ParentPositionID As Integer?

        Public Property DivisionID As Integer?

        Public Property JobLevelID As Integer?

        Public Property LevelNumber As Integer

        <ForeignKey("JobLevelID")>
        Public Overridable Property JobLevel As JobLevel

    End Class

End Namespace
