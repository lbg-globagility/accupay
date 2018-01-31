Imports Microsoft.Win32

Public Class DataGridViewTimeColumn

    Inherits DataGridViewTextBoxColumn

    Sub New()

        Me.CellTemplate =
            New DataGridViewTimeCell

    End Sub

    Sub DataGridViewTimeColumn()

    End Sub

End Class

Public Class DataGridViewTimeCell

    Inherits DataGridViewTextBoxCell

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineShortTime As String = RegKey.GetValue("sShortTime").ToString

    Dim machineLongTime As String = RegKey.GetValue("sTimeFormat").ToString

    Dim ValueBefore As Object = Nothing

    Protected Overrides Sub OnEnter(rowIndex As Integer, throughMouseClick As Boolean)

        ValueBefore = MyBase.Value

        MyBase.OnEnter(rowIndex, throughMouseClick)

    End Sub

    Protected Overrides Sub OnLeave(rowIndex As Integer, throughMouseClick As Boolean)

        Dim thisValue = MyBase.Value

        If thisValue Is Nothing Then

            MyBase.ErrorText = Nothing

        ElseIf ValueBefore <> MyBase.Value Then

            Dim dateobj As Object = Trim(thisValue).Replace(" ", ":")

            Dim dateobj_len = dateobj.ToString.Length

            Dim ampm As String = Nothing

            Try

                If dateobj.ToString.Contains("A") Or _
                    dateobj.ToString.Contains("P") Or _
                    dateobj.ToString.Contains("M") Then

                    ampm = " " & StrReverse(getStrBetween(StrReverse(dateobj.ToString), "", ":"))

                    dateobj_len -= ampm.Length

                    dateobj = dateobj.ToString.Replace(":", " ")

                    dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len)) 'dateobj.ToString.Substring(0, 4)

                    dateobj = dateobj.ToString.Replace(" ", ":")

                End If
                '    dateobj = getStrBetween(dateobj.ToString, "", " ")
                '    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("hh:mm")
                '    dgvempleave.Item(colName, rowIndx).Value = valtime.ToLongTimeString
                'Else

                Dim modified_format = machineLongTime 'If(dateobj_len = 5, "h:mm", machineLongTime)

                Dim valtime As DateTime = DateTime.Parse(dateobj).ToString(modified_format)

                If ampm = Nothing Then

                    MyBase.Value = valtime.ToLongTimeString

                Else
                    
                    'MyBase.Value = Trim(valtime.ToLongTimeString.Substring(0, (dateobj_len - 1))) & ampm
                    MyBase.Value = Trim(valtime.ToLongTimeString.Substring(0, dateobj_len)) & ampm

                End If
                'End If
                'valtime = DateTime.Parse(e.FormattedValue)
                'valtime = valtime.ToLongTimeString
                'Format(valtime, "hh:mm tt")

                MyBase.ErrorText = Nothing

                MyBase.Tag = valtime.ToString("HH:mm:ss")

            Catch ex As Exception

                Try

                    dateobj = dateobj.ToString.Replace(":", " ")

                    dateobj = Trim(dateobj.ToString.Substring(0, dateobj_len))

                    dateobj = dateobj.ToString.Replace(" ", ":")

                    Dim valtime As DateTime = DateTime.Parse(dateobj).ToString("HH:mm:ss")
                    'valtime = DateTime.Parse(e.FormattedValue)
                    'valtime = valtime.ToLongTimeString
                    MyBase.Value = valtime.ToLongTimeString
                    'Format(valtime, "hh:mm tt")

                    MyBase.ErrorText = Nothing

                    MyBase.Tag = valtime.ToString("HH:mm:ss")

                Catch exx As Exception

                    MyBase.ErrorText = "     Invalid" & vbNewLine &
                        "     time" & vbNewLine &
                        "     value" & vbNewLine

                    MyBase.Tag = Nothing

                End Try

            End Try

        End If

        MyBase.OnLeave(rowIndex, throughMouseClick)

    End Sub

End Class