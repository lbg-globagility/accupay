Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("organization")>
    Public Class Organization
        Implements IOrganization

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer? Implements IOrganization.RowID

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date Implements IOrganization.Created

        Public Property CreatedBy As Integer? Implements IOrganization.CreatedBy

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date? Implements IOrganization.LastUpd

        Public Property LastUpdBy As Integer? Implements IOrganization.LastUpdBy

        Public Property Name As String Implements IOrganization.Name

        Public Property NightDifferentialTimeFrom As TimeSpan Implements IOrganization.NightDifferentialTimeFrom

        Public Property NightDifferentialTimeTo As TimeSpan Implements IOrganization.NightDifferentialTimeTo

        Public Property IsAgency As Boolean Implements IOrganization.IsAgency

    End Class

End Namespace