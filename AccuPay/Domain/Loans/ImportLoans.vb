Public Class ImportLoans

    Private Data_Set As DataSet = Nothing

    Private ref_frm As Form = Nothing

    Sub New(ByVal dt As DataSet, ByVal MainWindow As Form)

        Data_Set = dt

        ref_frm = MainWindow

    End Sub

    Const MySQLProcedureName As String = "INSUPD_employeeloanschedule"

    Sub StartProcess()

        Dim success_prompt As String = String.Empty

        Dim n_ExecuteQuery =
            New ExecuteQuery("SET group_concat_max_len = 2048;" &
                             "SELECT GROUP_CONCAT(ii.PARAMETER_NAME)" &
                             " FROM information_schema.PARAMETERS ii" &
                             " WHERE ii.SPECIFIC_NAME = '" & MySQLProcedureName & "'" &
                             " AND ii.`SPECIFIC_SCHEMA`='" & sys_db & "'" &
                             " AND ii.PARAMETER_NAME IS NOT NULL;", 999999)

        Dim paramName = n_ExecuteQuery.Result

        Dim paramNames = Split(paramName, ",")
        Dim i As Integer = 0

        Try
            For Each dtTbl As DataTable In Data_Set.Tables

                For Each drow As DataRow In dtTbl.Rows
                    success_prompt = String.Empty
                    Dim n_ReadSQLFunctionV2 As _
                        New ReadSQLFunctionV2(MySQLProcedureName,
                                                "returnvalue", paramNames,
                                            DBNull.Value,
                                            orgztnID,
                                            z_User,
                                            drow(0),
                                            drow(2),
                                            MYSQLDateFormat(CDate(drow(3))),
                                            ValNoComma(drow(4)),
                                            drow(7),
                                            ValNoComma(drow(5)),
                                            ValNoComma(drow(6)),
                                            "In Progress",
                                            DBNull.Value,
                                            0,
                                            ValNoComma(drow(8)),
                                            ValNoComma(drow(8)),
                                            "",
                                            DBNull.Value,
                                            drow(1))
                    'DBNull.Value,
                    'orgztnID,
                    'z_User,
                    'drow(0),
                    'drow(1),
                    'MYSQLDateFormat(CDate(drow(6))),
                    'ValNoComma(drow(3)),
                    'drow(7),
                    'ValNoComma(drow(4)),
                    'ValNoComma(drow(9)),
                    '"In Progress",
                    'DBNull.Value,
                    '0,
                    'ValNoComma(drow(8)),
                    'ValNoComma(drow(8)),
                    '"",
                    'DBNull.Value,
                    'drow(2))
                    i += 1
                    'n_ReadSQLFunctionV2.HasError And
                    'If n_ReadSQLFunctionV2.ErrorMessage.Length > 0 Then
                    If n_ReadSQLFunctionV2.HasError Then
                        MsgBox("Error at row index " & i & vbNewLine & n_ReadSQLFunctionV2.ErrorMessage)
                        Continue For
                    End If
                    '`els_RowID` INT(11)
                    ', `els_OrganizID` INT(11)
                    ', `els_UserRowID` INT(11)
                    ', `els_EmpNumber` VARCHAR(50)
                    ', `els_LoanNum` VARCHAR(50)
                    ', `els_DateFrom` DATE
                    ', `els_TotLoanAmt` DECIMAL(11,6)
                    ', `els_DeductSched` VARCHAR(50)
                    ', `els_TotBalLeft` DECIMAL(11,6)
                    ', `els_DeductAmt` DECIMAL(11,6)
                    ', `els_Status` VARCHAR(50)
                    ', `els_LoanTypeID` INT(11)
                    ', `els_DeductPerc` DECIMAL(11,6)
                    ', `els_NoOfPayPer` INT(11)
                    ', `els_LoanPayPerLeft` INT(11)
                    ', `els_Comments` VARCHAR(2000)
                    ', `els_BonusID` INT(11)
                    ', `els_LoanName` VARCHAR(50))
                Next
                success_prompt = "Done importing employee loans."
                Exit For
            Next
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, "Class - ImportLoans"))
        Finally
            Data_Set.Dispose()
        End Try

        If success_prompt.Length > 0 Then
            MsgBox(success_prompt, MsgBoxStyle.Information)
        End If

    End Sub

End Class