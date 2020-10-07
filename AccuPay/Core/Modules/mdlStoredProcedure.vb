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

    Public Function sp_list(ByVal DisplayValue As String,
                                           ByVal LIC As String,
                                           ByVal Type As String,
                                           ByVal ParentLIC As String,
                                           ByVal Active As String,
                                           ByVal Description As String,
                                          ByVal Created As DateTime,
                                          ByVal CreatedBy As String,
                                          ByVal LastUpD As DateTime,
                                          ByVal orderby As Integer,
                                          ByVal lastupdby As String) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_ListValue", connection)
        With SQL_command
            Try
                .Connection.Open()
                .Parameters.AddWithValue("I_DisplayValue", DisplayValue)
                .Parameters.AddWithValue("I_LIC", LIC)
                .Parameters.AddWithValue("I_Type", Type)
                .Parameters.AddWithValue("I_ParentLIC", ParentLIC)
                .Parameters.AddWithValue("I_Active", Active)
                .Parameters.AddWithValue("I_Description", Description)
                .Parameters.AddWithValue("I_Created", Created)
                .Parameters.AddWithValue("I_Createdby", CreatedBy)
                .Parameters.AddWithValue("I_lastupd", LastUpD)
                .Parameters.AddWithValue("I_orderby", orderby)
                .Parameters.AddWithValue("I_lastupdby", lastupdby)
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

    Public Function SP_Organization(ByVal Name As String,
                                     ByVal PrimaryAddressID As Integer,
                                     ByVal PrimaryContactID As Integer,
                                     ByVal MainPhone As String,
                                     ByVal FaxNumber As String,
                                     ByVal EmailAddress As String,
                                     ByVal AltEmailAddress As String,
                                     ByVal AltPhone As String,
                                     ByVal URL As String,
                                     ByVal Created As DateTime,
                                     ByVal CreatedBy As Integer,
                                     ByVal LastUpd As DateTime,
                                     ByVal LastUpdBy As Integer,
                                     ByVal TINNumber As String,
                                     ByVal Tradename As String,
                                     ByVal orgType As String,
                                     Optional I_VacationLeaveDays As Object = Nothing,
                                     Optional I_SickLeaveDays As Object = Nothing,
                                     Optional I_MaternityLeaveDays As Object = Nothing,
                                     Optional I_OthersLeaveDays As Object = Nothing,
                                     Optional I_NightDifferentialTimeFrom As Object = Nothing,
                                     Optional I_NightDifferentialTimeTo As Object = Nothing,
                                     Optional I_NightShiftTimeFrom As Object = Nothing,
                                     Optional I_NightShiftTimeTo As Object = Nothing,
                                     Optional I_PayFrequencyID As Object = Nothing,
                                     Optional I_PhilhealthDeductionSchedule As Object = Nothing,
                                     Optional I_SSSDeductionSchedule As Object = Nothing,
                                     Optional I_PagIbigDeductionSchedule As Object = Nothing,
                                     Optional I_WorkDaysPerYear As Object = Nothing,
                                     Optional I_RDOCode As Object = Nothing,
                                     Optional I_ZIPCode As Object = Nothing,
                                     Optional WithholdingDeductionSchedule As Object = Nothing,
                                     Optional I_IsAgency As Boolean = False) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_Organization", connection)
        With SQL_command
            Try

                .Connection.Open()
                .Parameters.AddWithValue("I_CreatedBy", CreatedBy)
                .Parameters.AddWithValue("I_LastUpdBy", LastUpdBy)
                .Parameters.AddWithValue("I_Created", Created)
                .Parameters.AddWithValue("I_LastUpd", LastUpd)
                .Parameters.AddWithValue("I_Name", Name)
                .Parameters.AddWithValue("I_Tradename", Tradename)
                .Parameters.AddWithValue("I_PrimaryAddressID", If(PrimaryAddressID = Nothing, DBNull.Value, PrimaryAddressID))
                .Parameters.AddWithValue("I_PrimaryContactID", If(PrimaryContactID = Nothing, DBNull.Value, PrimaryContactID))
                .Parameters.AddWithValue("I_MainPhone", MainPhone)
                .Parameters.AddWithValue("I_FaxNumber", FaxNumber)
                .Parameters.AddWithValue("I_EmailAddress", EmailAddress)
                .Parameters.AddWithValue("I_AltEmailAddress", AltEmailAddress)
                .Parameters.AddWithValue("I_AltPhone", AltPhone)
                .Parameters.AddWithValue("I_URL", URL)
                .Parameters.AddWithValue("I_TINNo", TINNumber) 'OrganizationType
                .Parameters.AddWithValue("I_OrganizationType", orgType) 'OrganizationType

                .Parameters.AddWithValue("I_VacationLeaveDays", If(I_VacationLeaveDays = Nothing, DBNull.Value, I_VacationLeaveDays))
                .Parameters.AddWithValue("I_SickLeaveDays", If(I_SickLeaveDays = Nothing, DBNull.Value, I_SickLeaveDays))
                .Parameters.AddWithValue("I_MaternityLeaveDays", If(I_MaternityLeaveDays = Nothing, DBNull.Value, I_MaternityLeaveDays))
                .Parameters.AddWithValue("I_OthersLeaveDays", If(I_OthersLeaveDays = Nothing, DBNull.Value, I_OthersLeaveDays))

                .Parameters.AddWithValue("I_NightDifferentialTimeFrom", If(I_NightDifferentialTimeFrom = Nothing, DBNull.Value, I_NightDifferentialTimeFrom))
                .Parameters.AddWithValue("I_NightDifferentialTimeTo", If(I_NightDifferentialTimeTo = Nothing, DBNull.Value, I_NightDifferentialTimeTo))
                .Parameters.AddWithValue("I_NightShiftTimeFrom", If(I_NightShiftTimeFrom = Nothing, DBNull.Value, I_NightShiftTimeFrom))
                .Parameters.AddWithValue("I_NightShiftTimeTo", If(I_NightShiftTimeTo = Nothing, DBNull.Value, I_NightShiftTimeTo))

                .Parameters.AddWithValue("I_PhilhealthDeductionSchedule", If(I_PhilhealthDeductionSchedule = Nothing, DBNull.Value, I_PhilhealthDeductionSchedule))
                .Parameters.AddWithValue("I_SSSDeductionSchedule", If(I_SSSDeductionSchedule = Nothing, DBNull.Value, I_SSSDeductionSchedule))
                .Parameters.AddWithValue("I_PagIbigDeductionSchedule", If(I_PagIbigDeductionSchedule = Nothing, DBNull.Value, I_PagIbigDeductionSchedule))
                .Parameters.AddWithValue("I_WithholdingDeductionSchedule", If(WithholdingDeductionSchedule = Nothing, DBNull.Value, WithholdingDeductionSchedule))

                .Parameters.AddWithValue("I_PayFrequencyID", If(I_PayFrequencyID = Nothing, DBNull.Value, I_PayFrequencyID))

                .Parameters.AddWithValue("I_WorkDaysPerYear", If(I_WorkDaysPerYear = Nothing, 0, Val(I_WorkDaysPerYear)))

                .Parameters.AddWithValue("I_RDOCode", I_RDOCode.ToString)

                .Parameters.AddWithValue("I_ZIPCode", I_ZIPCode.ToString)
                .Parameters.AddWithValue("I_IsAgency", I_IsAgency)

                .Parameters.AddWithValue("I_ClientId", Z_Client)

                .CommandType = CommandType.StoredProcedure
                F_return = (.ExecuteNonQuery > 0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error I_Organization Stored Procedure")
            Finally
                connection.Close()
            End Try
        End With
        Return F_return
    End Function

    Public Function SP_OrganizationUpdate(ByVal Name As String,
                                 ByVal PrimaryAddressID As Integer,
                                 ByVal PrimaryContactID As Object,
                                 ByVal MainPhone As String,
                                 ByVal FaxNumber As String,
                                 ByVal EmailAddress As String,
                                 ByVal AltEmailAddress As String,
                                 ByVal AltPhone As String,
                                 ByVal URL As String,
                                 ByVal LastUpd As DateTime,
                                 ByVal LastUpdBy As Integer,
                                 ByVal TINNumber As String,
                                 ByVal Tradename As String,
                                 ByVal orgType As String,
                                 ByVal RowID As Integer,
                                 Optional I_VacationLeaveDays As Object = Nothing,
                                 Optional I_SickLeaveDays As Object = Nothing,
                                 Optional I_MaternityLeaveDays As Object = Nothing,
                                 Optional I_OthersLeaveDays As Object = Nothing,
                                     Optional I_NightDifferentialTimeFrom As Object = Nothing,
                                     Optional I_NightDifferentialTimeTo As Object = Nothing,
                                     Optional I_NightShiftTimeFrom As Object = Nothing,
                                     Optional I_NightShiftTimeTo As Object = Nothing,
                                 Optional I_PayFrequencyID As Object = Nothing,
                                     Optional I_PhilhealthDeductionSchedule As Object = Nothing,
                                     Optional I_SSSDeductionSchedule As Object = Nothing,
                                     Optional I_PagIbigDeductionSchedule As Object = Nothing,
                                     Optional I_WorkDaysPerYear As Object = Nothing,
                                     Optional I_RDOCode As Object = Nothing,
                                     Optional I_ZIPCode As Object = Nothing,
                                     Optional WithholdingDeductionSchedule As Object = Nothing,
                                     Optional I_IsAgency As Boolean = False) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_OrganizationUpdate", connection)
        With SQL_command
            Try

                .Connection.Open()
                .Parameters.AddWithValue("I_LastUpdBy", LastUpdBy)
                .Parameters.AddWithValue("I_LastUpd", LastUpd)
                .Parameters.AddWithValue("I_Name", Name)
                .Parameters.AddWithValue("I_Tradename", Tradename)
                .Parameters.AddWithValue("I_PrimaryAddressID", If(PrimaryAddressID = Nothing, DBNull.Value, PrimaryAddressID))
                .Parameters.AddWithValue("I_PrimaryContactID", If(PrimaryContactID = Nothing, DBNull.Value, PrimaryContactID))
                .Parameters.AddWithValue("I_MainPhone", MainPhone)
                .Parameters.AddWithValue("I_FaxNumber", FaxNumber)
                .Parameters.AddWithValue("I_EmailAddress", EmailAddress)
                .Parameters.AddWithValue("I_AltEmailAddress", AltEmailAddress)
                .Parameters.AddWithValue("I_AltPhone", AltPhone)
                .Parameters.AddWithValue("I_URL", URL)
                .Parameters.AddWithValue("I_TINNo", TINNumber)
                .Parameters.AddWithValue("I_OrganizationType", orgType)
                .Parameters.AddWithValue("I_RowID", RowID)

                .Parameters.AddWithValue("I_VacationLeaveDays", If(I_VacationLeaveDays = Nothing, DBNull.Value, I_VacationLeaveDays))
                .Parameters.AddWithValue("I_SickLeaveDays", If(I_SickLeaveDays = Nothing, DBNull.Value, I_SickLeaveDays))
                .Parameters.AddWithValue("I_MaternityLeaveDays", If(I_MaternityLeaveDays = Nothing, DBNull.Value, I_MaternityLeaveDays))
                .Parameters.AddWithValue("I_OthersLeaveDays", If(I_OthersLeaveDays = Nothing, DBNull.Value, I_OthersLeaveDays))

                .Parameters.AddWithValue("I_NightDifferentialTimeFrom", If(I_NightDifferentialTimeFrom = Nothing, DBNull.Value, I_NightDifferentialTimeFrom))
                .Parameters.AddWithValue("I_NightDifferentialTimeTo", If(I_NightDifferentialTimeTo = Nothing, DBNull.Value, I_NightDifferentialTimeTo))
                .Parameters.AddWithValue("I_NightShiftTimeFrom", If(I_NightShiftTimeFrom = Nothing, DBNull.Value, I_NightShiftTimeFrom))
                .Parameters.AddWithValue("I_NightShiftTimeTo", If(I_NightShiftTimeTo = Nothing, DBNull.Value, I_NightShiftTimeTo))

                .Parameters.AddWithValue("I_PhilhealthDeductionSchedule", If(I_PhilhealthDeductionSchedule = Nothing, DBNull.Value, I_PhilhealthDeductionSchedule))
                .Parameters.AddWithValue("I_SSSDeductionSchedule", If(I_SSSDeductionSchedule = Nothing, DBNull.Value, I_SSSDeductionSchedule))
                .Parameters.AddWithValue("I_PagIbigDeductionSchedule", If(I_PagIbigDeductionSchedule = Nothing, DBNull.Value, I_PagIbigDeductionSchedule))
                .Parameters.AddWithValue("I_WithholdingDeductionSchedule", If(WithholdingDeductionSchedule = Nothing, DBNull.Value, WithholdingDeductionSchedule))

                .Parameters.AddWithValue("I_PayFrequencyID", If(I_PayFrequencyID = Nothing, DBNull.Value, I_PayFrequencyID))

                .Parameters.AddWithValue("I_WorkDaysPerYear", If(I_WorkDaysPerYear = Nothing, 0, Val(I_WorkDaysPerYear)))

                .Parameters.AddWithValue("I_RDOCode", I_RDOCode.ToString)

                .Parameters.AddWithValue("I_ZIPCode", I_ZIPCode.ToString)
                .Parameters.AddWithValue("I_IsAgency", I_IsAgency)

                .CommandType = CommandType.StoredProcedure
                F_return = (.ExecuteNonQuery > 0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error I_OrganizationUpdate Stored Procedure")
            Finally
                connection.Close()
            End Try
        End With
        Return F_return
    End Function

    Public Function SP_OrganizationWithImage(ByVal Name As String,
                                 ByVal PrimaryAddressID As Integer,
                                 ByVal PrimaryContactID As Integer,
                                 ByVal MainPhone As String,
                                 ByVal FaxNumber As String,
                                 ByVal EmailAddress As String,
                                 ByVal AltEmailAddress As String,
                                 ByVal AltPhone As String,
                                 ByVal URL As String,
                                 ByVal Created As DateTime,
                                 ByVal CreatedBy As Integer,
                                 ByVal LastUpd As DateTime,
                                 ByVal LastUpdBy As Integer,
                                 ByVal TINNumber As String,
                                 ByVal Tradename As String,
                                 ByVal orgType As String,
                                 ByVal image As Object,
                                 Optional I_VacationLeaveDays As Object = Nothing,
                                 Optional I_SickLeaveDays As Object = Nothing,
                                 Optional I_MaternityLeaveDays As Object = Nothing,
                                 Optional I_OthersLeaveDays As Object = Nothing,
                                     Optional I_NightDifferentialTimeFrom As Object = Nothing,
                                     Optional I_NightDifferentialTimeTo As Object = Nothing,
                                     Optional I_NightShiftTimeFrom As Object = Nothing,
                                     Optional I_NightShiftTimeTo As Object = Nothing,
                                 Optional I_PayFrequencyID As Object = Nothing,
                                     Optional I_PhilhealthDeductionSchedule As Object = Nothing,
                                     Optional I_SSSDeductionSchedule As Object = Nothing,
                                     Optional I_PagIbigDeductionSchedule As Object = Nothing,
                                     Optional I_WorkDaysPerYear As Object = Nothing,
                                     Optional I_RDOCode As Object = Nothing,
                                     Optional I_ZIPCode As Object = Nothing,
                                     Optional WithholdingDeductionSchedule As Object = Nothing,
                                             Optional I_IsAgency As Boolean = False) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_OrgWithImage", connection)
        With SQL_command
            Try

                .Connection.Open()
                .Parameters.AddWithValue("I_CreatedBy", CreatedBy)
                .Parameters.AddWithValue("I_LastUpdBy", LastUpdBy)
                .Parameters.AddWithValue("I_Created", Created)
                .Parameters.AddWithValue("I_LastUpd", LastUpd)
                .Parameters.AddWithValue("I_Name", Name)
                .Parameters.AddWithValue("I_Tradename", Tradename)
                .Parameters.AddWithValue("I_PrimaryAddressID", If(PrimaryAddressID = Nothing, DBNull.Value, PrimaryAddressID))
                .Parameters.AddWithValue("I_PrimaryContactID", If(PrimaryContactID = Nothing, DBNull.Value, PrimaryContactID))
                .Parameters.AddWithValue("I_MainPhone", MainPhone)
                .Parameters.AddWithValue("I_FaxNumber", FaxNumber)
                .Parameters.AddWithValue("I_EmailAddress", EmailAddress)
                .Parameters.AddWithValue("I_AltEmailAddress", AltEmailAddress)
                .Parameters.AddWithValue("I_AltPhone", AltPhone)
                .Parameters.AddWithValue("I_URL", URL)
                .Parameters.AddWithValue("I_TINNo", TINNumber) 'OrganizationType
                .Parameters.AddWithValue("I_OrganizationType", orgType) 'OrganizationType
                .Parameters.AddWithValue("I_Image", image) 'OrganizationType

                .Parameters.AddWithValue("I_VacationLeaveDays", If(I_VacationLeaveDays = Nothing, DBNull.Value, I_VacationLeaveDays))
                .Parameters.AddWithValue("I_SickLeaveDays", If(I_SickLeaveDays = Nothing, DBNull.Value, I_SickLeaveDays))
                .Parameters.AddWithValue("I_MaternityLeaveDays", If(I_MaternityLeaveDays = Nothing, DBNull.Value, I_MaternityLeaveDays))
                .Parameters.AddWithValue("I_OthersLeaveDays", If(I_OthersLeaveDays = Nothing, DBNull.Value, I_OthersLeaveDays))

                .Parameters.AddWithValue("I_NightDifferentialTimeFrom", If(I_NightDifferentialTimeFrom = Nothing, DBNull.Value, I_NightDifferentialTimeFrom))
                .Parameters.AddWithValue("I_NightDifferentialTimeTo", If(I_NightDifferentialTimeTo = Nothing, DBNull.Value, I_NightDifferentialTimeTo))
                .Parameters.AddWithValue("I_NightShiftTimeFrom", If(I_NightShiftTimeFrom = Nothing, DBNull.Value, I_NightShiftTimeFrom))
                .Parameters.AddWithValue("I_NightShiftTimeTo", If(I_NightShiftTimeTo = Nothing, DBNull.Value, I_NightShiftTimeTo))

                .Parameters.AddWithValue("I_PhilhealthDeductionSchedule", If(I_PhilhealthDeductionSchedule = Nothing, DBNull.Value, I_PhilhealthDeductionSchedule))
                .Parameters.AddWithValue("I_SSSDeductionSchedule", If(I_SSSDeductionSchedule = Nothing, DBNull.Value, I_SSSDeductionSchedule))
                .Parameters.AddWithValue("I_PagIbigDeductionSchedule", If(I_PagIbigDeductionSchedule = Nothing, DBNull.Value, I_PagIbigDeductionSchedule))
                .Parameters.AddWithValue("I_WithholdingDeductionSchedule", If(WithholdingDeductionSchedule = Nothing, DBNull.Value, WithholdingDeductionSchedule))

                .Parameters.AddWithValue("I_PayFrequencyID", If(I_PayFrequencyID = Nothing, DBNull.Value, I_PayFrequencyID))

                .Parameters.AddWithValue("I_WorkDaysPerYear", If(I_WorkDaysPerYear = Nothing, 0, Val(I_WorkDaysPerYear)))

                .Parameters.AddWithValue("I_RDOCode", I_RDOCode.ToString)

                .Parameters.AddWithValue("I_ZIPCode", I_ZIPCode.ToString)
                .Parameters.AddWithValue("I_IsAgency", I_IsAgency)

                .CommandType = CommandType.StoredProcedure
                F_return = (.ExecuteNonQuery > 0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error I_OrgWithImage Stored Procedure")
            Finally
                connection.Close()
            End Try
        End With
        Return F_return
    End Function

    Public Function SP_OrganizationWithImageUpdate(ByVal Name As String,
                                ByVal PrimaryAddressID As Integer,
                                ByVal PrimaryContactID As Object,
                                ByVal MainPhone As String,
                                ByVal FaxNumber As String,
                                ByVal EmailAddress As String,
                                ByVal AltEmailAddress As String,
                                ByVal AltPhone As String,
                                ByVal URL As String,
                                ByVal LastUpd As DateTime,
                                ByVal LastUpdBy As Integer,
                                ByVal TINNumber As String,
                                ByVal Tradename As String,
                                ByVal orgType As String,
                                ByVal image As Object,
                                ByVal RowID As Integer,
                                 Optional I_VacationLeaveDays As Object = Nothing,
                                 Optional I_SickLeaveDays As Object = Nothing,
                                 Optional I_MaternityLeaveDays As Object = Nothing,
                                 Optional I_OthersLeaveDays As Object = Nothing,
                                     Optional I_NightDifferentialTimeFrom As Object = Nothing,
                                     Optional I_NightDifferentialTimeTo As Object = Nothing,
                                     Optional I_NightShiftTimeFrom As Object = Nothing,
                                     Optional I_NightShiftTimeTo As Object = Nothing,
                                 Optional I_PayFrequencyID As Object = Nothing,
                                     Optional I_PhilhealthDeductionSchedule As Object = Nothing,
                                     Optional I_SSSDeductionSchedule As Object = Nothing,
                                     Optional I_PagIbigDeductionSchedule As Object = Nothing,
                                     Optional I_WorkDaysPerYear As Object = Nothing,
                                     Optional I_RDOCode As Object = Nothing,
                                     Optional I_ZIPCode As Object = Nothing,
                                     Optional WithholdingDeductionSchedule As Object = Nothing,
                                                   Optional I_IsAgency As Boolean = False) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_OrgWithImageUpdate", connection)
        With SQL_command
            Try

                .Connection.Open()
                .Parameters.AddWithValue("I_LastUpdBy", LastUpdBy)
                .Parameters.AddWithValue("I_LastUpd", LastUpd)
                .Parameters.AddWithValue("I_Name", Name)
                .Parameters.AddWithValue("I_Tradename", Tradename)
                .Parameters.AddWithValue("I_PrimaryAddressID", PrimaryAddressID)
                .Parameters.AddWithValue("I_PrimaryContactID", If(PrimaryContactID = Nothing, DBNull.Value, PrimaryContactID))
                .Parameters.AddWithValue("I_MainPhone", MainPhone)
                .Parameters.AddWithValue("I_FaxNumber", FaxNumber)
                .Parameters.AddWithValue("I_EmailAddress", EmailAddress)
                .Parameters.AddWithValue("I_AltEmailAddress", AltEmailAddress)
                .Parameters.AddWithValue("I_AltPhone", AltPhone)
                .Parameters.AddWithValue("I_URL", URL)
                .Parameters.AddWithValue("I_TINNo", TINNumber)
                .Parameters.AddWithValue("I_OrganizationType", orgType)
                .Parameters.AddWithValue("I_Image", image)
                .Parameters.AddWithValue("I_RowID", RowID)

                .Parameters.AddWithValue("I_VacationLeaveDays", If(I_VacationLeaveDays = Nothing, DBNull.Value, I_VacationLeaveDays))
                .Parameters.AddWithValue("I_SickLeaveDays", If(I_SickLeaveDays = Nothing, DBNull.Value, I_SickLeaveDays))
                .Parameters.AddWithValue("I_MaternityLeaveDays", If(I_MaternityLeaveDays = Nothing, DBNull.Value, I_MaternityLeaveDays))
                .Parameters.AddWithValue("I_OthersLeaveDays", If(I_OthersLeaveDays = Nothing, DBNull.Value, I_OthersLeaveDays))

                .Parameters.AddWithValue("I_NightDifferentialTimeFrom", If(I_NightDifferentialTimeFrom = Nothing, DBNull.Value, I_NightDifferentialTimeFrom))
                .Parameters.AddWithValue("I_NightDifferentialTimeTo", If(I_NightDifferentialTimeTo = Nothing, DBNull.Value, I_NightDifferentialTimeTo))
                .Parameters.AddWithValue("I_NightShiftTimeFrom", If(I_NightShiftTimeFrom = Nothing, DBNull.Value, I_NightShiftTimeFrom))
                .Parameters.AddWithValue("I_NightShiftTimeTo", If(I_NightShiftTimeTo = Nothing, DBNull.Value, I_NightShiftTimeTo))

                .Parameters.AddWithValue("I_PhilhealthDeductionSchedule", If(I_PhilhealthDeductionSchedule = Nothing, DBNull.Value, I_PhilhealthDeductionSchedule))
                .Parameters.AddWithValue("I_SSSDeductionSchedule", If(I_SSSDeductionSchedule = Nothing, DBNull.Value, I_SSSDeductionSchedule))
                .Parameters.AddWithValue("I_PagIbigDeductionSchedule", If(I_PagIbigDeductionSchedule = Nothing, DBNull.Value, I_PagIbigDeductionSchedule))
                .Parameters.AddWithValue("I_WithholdingDeductionSchedule", If(WithholdingDeductionSchedule = Nothing, DBNull.Value, WithholdingDeductionSchedule))

                .Parameters.AddWithValue("I_PayFrequencyID", If(I_PayFrequencyID = Nothing, DBNull.Value, I_PayFrequencyID))

                .Parameters.AddWithValue("I_WorkDaysPerYear", If(I_WorkDaysPerYear = Nothing, 0, Val(I_WorkDaysPerYear)))

                .Parameters.AddWithValue("I_RDOCode", I_RDOCode.ToString)

                .Parameters.AddWithValue("I_ZIPCode", I_ZIPCode.ToString)
                .Parameters.AddWithValue("I_IsAgency", I_IsAgency)

                .CommandType = CommandType.StoredProcedure
                F_return = (.ExecuteNonQuery > 0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Error I_OrgWithImageUpdate Stored Procedure")
            Finally
                connection.Close()
            End Try
        End With
        Return F_return
    End Function

    Public Function I_contact(ByVal Status As String,
          ByVal Created As DateTime,
          ByVal OrganizationID As Integer,
          ByVal MainPhone As String,
          ByVal LastName As String,
          ByVal FirstName As String,
          ByVal MiddleName As String,
          ByVal MobilePhone As String,
          ByVal WorkPhone As String,
          ByVal Gender As String,
          ByVal JobTitle As String,
          ByVal EmailAddress As String,
          ByVal AlternatePhone As String,
          ByVal FaxNumber As String,
          ByVal LastUpd As DateTime,
          ByVal CreatedBy As Integer,
          ByVal LastUpdBy As Integer,
          ByVal personaltitle As String,
          ByVal type As String,
          ByVal suffix As String,
          ByVal addrID As Integer,
          ByVal tinno As String) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_contact", connection)
        With SQL_command
            Try

                .Connection.Open()
                .Parameters.AddWithValue("I_Status", Status)
                .Parameters.AddWithValue("I_Created", Created)
                .Parameters.AddWithValue("I_OrganizationID", OrganizationID)
                .Parameters.AddWithValue("I_MainPhone", MainPhone)
                .Parameters.AddWithValue("I_LastName", LastName)
                .Parameters.AddWithValue("I_FirstName", FirstName)
                .Parameters.AddWithValue("I_MiddleName", MiddleName)
                .Parameters.AddWithValue("I_MobilePhone", MobilePhone)
                .Parameters.AddWithValue("I_WorkPhone", WorkPhone)
                .Parameters.AddWithValue("I_Gender", Gender)
                .Parameters.AddWithValue("I_JobTitle", JobTitle)
                .Parameters.AddWithValue("I_EmailAddress", EmailAddress)
                .Parameters.AddWithValue("I_AlternatePhone", AlternatePhone)
                .Parameters.AddWithValue("I_FaxNumber", FaxNumber)
                .Parameters.AddWithValue("I_LastUpd", LastUpd)
                .Parameters.AddWithValue("I_CreatedBy", CreatedBy)
                .Parameters.AddWithValue("I_LastUpdBy", LastUpdBy)
                .Parameters.AddWithValue("I_PersonalTitle", personaltitle)
                .Parameters.AddWithValue("I_Type", type)
                .Parameters.AddWithValue("I_Suffix", type)
                .Parameters.AddWithValue("I_AddressID", If(addrID = Nothing, DBNull.Value, addrID)) 'I_TINNumber
                .Parameters.AddWithValue("I_TINNumber", tinno) 'I_TINNumber
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

    Public Function I_contactUpdate(ByVal Status As String,
      ByVal MainPhone As String,
      ByVal LastName As String,
      ByVal FirstName As String,
      ByVal MiddleName As String,
      ByVal MobilePhone As String,
      ByVal WorkPhone As String,
      ByVal Gender As String,
      ByVal JobTitle As String,
      ByVal EmailAddress As String,
      ByVal AlternatePhone As String,
      ByVal FaxNumber As String,
      ByVal LastUpd As DateTime,
      ByVal LastUpdBy As Integer,
      ByVal personaltitle As String,
      ByVal type As String,
      ByVal suffix As String,
      ByVal addrID As Integer,
      ByVal tinno As String,
      ByVal RowID As Integer) As Boolean

        Dim F_return As Boolean = False
        Dim SQL_command As MySqlCommand =
                  New MySqlCommand("I_contactUpdate", connection)
        With SQL_command
            Try

                .Connection.Open()
                .Parameters.AddWithValue("I_Status", Status)
                .Parameters.AddWithValue("I_MainPhone", MainPhone)
                .Parameters.AddWithValue("I_LastName", LastName)
                .Parameters.AddWithValue("I_FirstName", FirstName)
                .Parameters.AddWithValue("I_MiddleName", MiddleName)
                .Parameters.AddWithValue("I_MobilePhone", MobilePhone)
                .Parameters.AddWithValue("I_WorkPhone", WorkPhone)
                .Parameters.AddWithValue("I_Gender", Gender)
                .Parameters.AddWithValue("I_JobTitle", JobTitle)
                .Parameters.AddWithValue("I_EmailAddress", EmailAddress)
                .Parameters.AddWithValue("I_AlternatePhone", AlternatePhone)
                .Parameters.AddWithValue("I_FaxNumber", FaxNumber)
                .Parameters.AddWithValue("I_LastUpd", LastUpd)
                .Parameters.AddWithValue("I_LastUpdBy", LastUpdBy)
                .Parameters.AddWithValue("I_PersonalTitle", personaltitle)
                .Parameters.AddWithValue("I_Type", type)
                .Parameters.AddWithValue("I_Suffix", type)
                .Parameters.AddWithValue("I_AddressID", addrID) 'I_TINNumber
                .Parameters.AddWithValue("I_TINNumber", tinno) 'I_TINNumber
                .Parameters.AddWithValue("I_RowID", If(RowID = Nothing, DBNull.Value, RowID)) 'I_TINNumber
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