Imports FluentNHibernate.Mapping

Public Class OvertimeMap
    Inherits ClassMap(Of Overtime)

    Public Sub New()
        Table("employeeovertime")

        Id(Function(x) x.RowID)
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpdBy)
        Map(Function(x) x.OTStartDate)
        Map(Function(x) x.OTEndDate)
        Map(Function(x) x.OTStartTime).CustomType("TimeAsTimeSpan")
        Map(Function(x) x.OTEndTime).CustomType("TimeAsTimeSpan")
    End Sub

End Class
