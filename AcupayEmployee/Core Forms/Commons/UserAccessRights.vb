
Public Enum AccessRightName
    HasReadOnly = 0
    HasUpdate = 1
    HasCreate = 2
    HasDelete = 3

End Enum

Public Class UserAccessRights

    Dim n_RowIDOfView = Nothing

    Sub New(RowIDOfView As Object)

        n_RowIDOfView = RowIDOfView

    End Sub

    Dim returnvalue = False

    Public Function ResultValue(arn As AccessRightName) As Boolean

        Select Case CInt(arn)

            Case 0 'ReadOnly

                Dim useraccess = position_view_table.Select("ViewID = " & n_RowIDOfView &
                                                            " AND ReadOnly='Y'")

                If useraccess.Count <> 0 Then
                    returnvalue = True

                End If

            Case 1 'Updates

                Dim useraccess = position_view_table.Select("ViewID = " & n_RowIDOfView &
                                                            " AND Updates='Y'")

                If useraccess.Count <> 0 Then
                    returnvalue = True

                End If

            Case 2 'Creates

                Dim useraccess = position_view_table.Select("ViewID = " & n_RowIDOfView &
                                                            " AND Creates='Y'")

                If useraccess.Count <> 0 Then
                    returnvalue = True

                End If

            Case 3 'Deleting

                Dim useraccess = position_view_table.Select("ViewID = " & n_RowIDOfView &
                                                            " AND Deleting='Y'")

                If useraccess.Count <> 0 Then
                    returnvalue = True

                End If

        End Select

        Return returnvalue

    End Function

End Class