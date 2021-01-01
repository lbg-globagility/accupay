Imports AccuPay.Core
Imports AccuPay.Core.Entities
Imports MySql.Data.MySqlClient

Namespace Routes

    Public Class TripTicketRepository
        Private context As PayrollContext

        Public Sub New(context As PayrollContext)
            Me.context = context
        End Sub

        Public Function Find(tripTicketID As Integer?) As TripTicket
            Return context.TripTickets.Single(Function(x) x.RowID = tripTicketID)
        End Function

        'Public Function Find(tripTicketID As Integer?) As TripTicket
        '    Dim tripTicket As TripTicket = Nothing

        '    Using connection As New MySqlConnection(connectionString),
        '        command As New MySqlCommand("", connection)

        '        command.Parameters.AddWithValue("@RowID", tripTicketID)

        '        connection.Open()

        '        Dim reader = command.ExecuteReader()
        '        While reader.Read()
        '            tripTicket = New TripTicket() With {
        '                .RowID = reader.GetValue(Of Integer?)("RowID"),
        '                .OrganizationID = reader.GetValue(Of Integer?)("OrganizationID"),
        '                .Created = reader.GetValue(Of DateTime)("Created"),
        '                .CreatedBy = reader.GetValue(Of Integer?)("CreatedBy"),
        '                .LastUpd = reader.GetValue(Of DateTime)("LastUpd"),
        '                .LastUpdBy = reader.GetValue(Of Integer?)("LastUpdBy")
        '            }
        '        End While
        '    End Using

        '    Return tripTicket
        'End Function

        Public Sub Save(tripTicket As TripTicket)
            Using connection As New MySqlConnection(connectionString),
                command As New MySqlCommand("", connection)

                With command.Parameters
                    .AddWithValue("@RowID", tripTicket.RowID)
                    .AddWithValue("@OrganizationID", tripTicket.OrganizationID)
                    .AddWithValue("@CreatedBy", tripTicket.CreatedBy)
                    .AddWithValue("LastUpdBy", tripTicket.LastUpdBy)
                    .AddWithValue("@VehicledID", tripTicket.VehicleID)
                    .AddWithValue("@RouteID", tripTicket.RouteID)
                    .AddWithValue("@TicketNo", tripTicket.TicketNo)
                    .AddWithValue("@TripDate", tripTicket.Date)
                    .AddWithValue("@StartAt", tripTicket.TimeFrom)
                    .AddWithValue("@EndAt", tripTicket.TimeTo)
                    .AddWithValue("@DispatchAt", tripTicket.TimeDispatched)
                    .AddWithValue("@Guide", tripTicket.Guide)
                End With
            End Using
        End Sub

    End Class

End Namespace