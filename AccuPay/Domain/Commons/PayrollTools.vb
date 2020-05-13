Option Strict On

Public Class PayrollTools

    Public Shared Sub UpdateLoanSchedule(paypRowID As Integer)

        Dim param_array = New Object() {orgztnID, paypRowID, z_User}

        Static strquery_recompute_13monthpay As String =
                        "call recompute_thirteenthmonthpay(?organizid, ?payprowid, ?userrowid);"

        Dim n_ExecSQLProcedure = New SQL(strquery_recompute_13monthpay, param_array)
        n_ExecSQLProcedure.ExecuteQuery()
    End Sub

    Public Shared Function GetOrganizationAddress() As String

        Dim str_quer_address As String =
            String.Concat("SELECT CONCAT_WS(', '",
                          ", IF(LENGTH(TRIM(ad.StreetAddress1)) = 0, NULL, ad.StreetAddress1)",
                          ", IF(LENGTH(TRIM(ad.StreetAddress2)) = 0, NULL, ad.StreetAddress2)",
                          ", IF(LENGTH(TRIM(ad.Barangay)) = 0, NULL, ad.Barangay)",
                          ", IF(LENGTH(TRIM(ad.CityTown)) = 0, NULL, ad.CityTown)",
                          ", IF(LENGTH(TRIM(ad.Country)) = 0, NULL, ad.Country)",
                          ", IF(LENGTH(TRIM(ad.State)) = 0, NULL, ad.State)",
                          ") `Result`",
                          " FROM organization og",
                          " LEFT JOIN address ad ON ad.RowID = og.PrimaryAddressID",
                          " WHERE og.RowID = ", orgztnID, ";")

        Return Convert.ToString(New SQL(str_quer_address).GetFoundRow)

    End Function

End Class