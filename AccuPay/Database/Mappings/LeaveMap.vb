Option Strict On

Imports AccuPay.Entity
Imports FluentNHibernate.Mapping

Public Class LeaveMap
    Inherits ClassMap(Of Leave)

    Public Sub New()
        Table("employeeleave")

        Id(Function(x) x.RowID).GeneratedBy.Identity()
        Map(Function(x) x.OrganizationID)
        Map(Function(x) x.Created).Generated.Insert()
        Map(Function(x) x.CreatedBy)
        Map(Function(x) x.LastUpd).Generated.Always()
        Map(Function(x) x.LastUpdBy)

        Map(Function(x) x.EmployeeID)
        Map(Function(x) x.LeaveType)
        Map(Function(x) x.LeaveHours)
        Map(Function(x) x.StartTime).Column("LeaveStartTime")
        Map(Function(x) x.EndTime).Column("LeaveEndTime")
        Map(Function(x) x.StartDate).Column("LeaveStartDate")
        Map(Function(x) x.EndDate).Column("LeaveEndDate")
        Map(Function(x) x.Reason)
        Map(Function(x) x.Comments)
        Map(Function(x) x.Image)
        Map(Function(x) x.Status)
    End Sub

End Class
