Imports MySql.Data.MySqlClient

Module mdlStoredProcedure

    'Dim n_DataBaseConnection As New DataBaseConnection

    Public connectionString As String = n_DataBaseConnection.GetStringMySQLConnectionString

    Public connection As MySqlConnection = New MySqlConnection(connectionString)

    Public Function sp_employeeshiftentry(ByVal Created As DateTime,
                                      ByVal CreatedBy As Integer,
                                      ByVal LastUpD As DateTime,
                                      ByVal OrganizationID As Integer,
                                      ByVal lastupdby As Integer,
                                      ByVal Effectivefrom As Date,
                                      ByVal Effectiveto As Date,
                                      ByVal EmployeeID As Integer,
                                      ByVal ShiftID As Integer,
                                      ByVal nightshift As Boolean,
                                      Optional restday As String = Nothing)

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("sp_employeeshiftentry", connection)
        With SQL_command
            Try
                .Connection.Open()

                .Parameters.AddWithValue("I_Created", Created)
                .Parameters.AddWithValue("I_Createdby", CreatedBy)
                .Parameters.AddWithValue("I_lastupd", LastUpD)
                .Parameters.AddWithValue("I_OrganizationID", OrganizationID)
                .Parameters.AddWithValue("I_lastupdby", lastupdby)
                .Parameters.AddWithValue("I_EffectiveFrom", Effectivefrom)
                .Parameters.AddWithValue("I_EffectiveTo", Effectiveto)
                .Parameters.AddWithValue("I_ShiftID", If(ShiftID = 0, DBNull.Value, ShiftID))
                .Parameters.AddWithValue("I_EmployeeID", EmployeeID)
                .Parameters.AddWithValue("I_NightShift", nightshift)
                .Parameters.AddWithValue("I_RestDay", If(restday = Nothing, 0, restday))

                .CommandType = CommandType.StoredProcedure
                F_return = (.ExecuteNonQuery > 0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Finally
                connection.Close()
            End Try
        End With
        Return F_return
    End Function

    Public Function sp_shift(ByVal Created As DateTime,
                                      ByVal CreatedBy As Integer,
                                      ByVal LastUpD As DateTime,
                                      ByVal OrganizationID As Integer,
                                      ByVal lastupdby As Integer,
                                      ByVal timefrom As DateTime,
                                      ByVal timeto As DateTime)

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("sp_Shift", connection)
        With SQL_command
            Try
                .Connection.Open()

                .Parameters.AddWithValue("I_Created", Created)
                .Parameters.AddWithValue("I_Createdby", CreatedBy)
                .Parameters.AddWithValue("I_lastupd", LastUpD)
                .Parameters.AddWithValue("I_OrganizationID", OrganizationID)
                .Parameters.AddWithValue("I_lastupdby", lastupdby)
                .Parameters.AddWithValue("I_TimeFrom", Format(CDate(timefrom), "HH:mm"))
                .Parameters.AddWithValue("I_TimeTo", Format(CDate(timeto), "HH:mm"))

                .CommandType = CommandType.StoredProcedure
                F_return = (.ExecuteNonQuery > 0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Finally
                connection.Close()
            End Try
        End With
        Return F_return
    End Function

End Module