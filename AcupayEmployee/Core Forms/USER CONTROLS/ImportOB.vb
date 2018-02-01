
Public Class ImportOB

    Dim dt As New DataSet

    Dim commanding_form As New Form

    Sub New(excel_file_contents As DataSet,
            form_caller As Form)

        dt = excel_file_contents
        commanding_form = form_caller

    End Sub

    Sub DoImport()
        'og_id
        ',user_rowid
        ',emp_num
        ',allowance_name
        ',start_date
        ',start_time
        ',end_date
        ',end_time

        Dim str_query_import_loan As String =
            String.Concat("CALL IMPORT_OB(",
                          "?og_id",
                          ", ?user_rowid",
                          ", ?emp_num",
                          ", ?allowance_name",
                          ", ?start_date",
                          ", ?start_time",
                          ", ?end_date",
                          ", ?end_time);")

        For Each dtTbl As DataTable In dt.Tables

            For Each drow As DataRow In dtTbl.Rows

                Dim param_values =
                    New Object() {orgztnID,
                                 z_User,
                                  drow(0),
                                  drow(1),
                                  drow(2),
                                  drow(3),
                                  drow(4),
                                  drow(5)}

                Dim sql As New SQL(str_query_import_loan,
                                   param_values)

                sql.ExecuteQuery()

                If sql.HasError Then
                    MsgBox(sql.ErrorMessage)
                End If

            Next
        Next

    End Sub

End Class