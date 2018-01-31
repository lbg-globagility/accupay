Imports Microsoft.Win32

Public Class DataGridViewDateColumn

    Inherits DataGridViewTextBoxColumn

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineShortDate As String = RegKey.GetValue("sShortDate").ToString

    Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()

    'Dim n_MinimumDate As Date = "1900-01-01"

    'Property MinimumDate As Date

    '    Get
    '        Return n_MinimumDate
    '    End Get

    '    Set(value As Date)
    '        n_MinimumDate = value
    '    End Set

    'End Property

    Sub New()

        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        DataGridViewCellStyle1.Format = machineShortDate

        Me.CellTemplate =
            New DataGridViewDateCell

        'Dim fsdfsd As New DataGridViewDateCell

        'fsdfsd.Tag = Nothing

        Me.DefaultCellStyle = DataGridViewCellStyle1

    End Sub

    Sub DataGridViewDateColumn()

    End Sub

End Class

Public Class DataGridViewDateCell

    Inherits DataGridViewTextBoxCell

    Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)

    Dim machineShortDate As String = RegKey.GetValue("sShortDate").ToString 'sShortDate

    Dim machineLongTime As String = RegKey.GetValue("sTimeFormat").ToString

    Dim ValueBefore As Object = Nothing

    Dim n_MinimumDate As Date = "1900-01-01"

    Sub New() 'Optional Minimum_DATE As Date = Nothing)

        'If Minimum_DATE = Nothing Then
        'Else
        '    n_MinimumDate = Minimum_DATE
        'End If

        MyBase.ContextMenuStrip = New ContextMenuStrip

        MyBase.MaxInputLength = 11

    End Sub

    Dim n_value As Object = Nothing

    'Public Overloads Property Value As Object

    '    Get
    '        Return n_value
    '    End Get

    '    Set(value As Object)
    '        n_value = value

    '        Try

    '            MyBase.ErrorText = Nothing

    '            MyBase.Tag = MYSQLDateFormat(CDate(value))

    '            n_value = Format(CDate(value), machineShortDate)

    '        Catch ex As Exception

    '            MyBase.ErrorText = "     Invalid" & vbNewLine &
    '                "     date" & vbNewLine &
    '                "     value" & vbNewLine

    '            MyBase.Tag = MYSQLDateFormat(CDate(n_MinimumDate))

    '            n_value = Format(CDate(n_MinimumDate), machineShortDate)

    '        End Try

    '    End Set

    'End Property

    Property MinimumDate As Date

        Get
            Return n_MinimumDate
        End Get

        Set(value As Date)
            n_MinimumDate = value

        End Set

    End Property

    'Protected Overrides Function SetValue(rowIndex As Integer, value As Object) As Boolean

    '    Static once As SByte = 0

    '    If once = 0 Then

    '        once = 1

    '        value = n_MinimumDate

    '    End If

    '    Try

    '        MyBase.ErrorText = Nothing

    '        MyBase.Tag = MYSQLDateFormat(CDate(value))

    '        MyBase.Value = Format(CDate(value), machineShortDate)

    '    Catch ex As Exception

    '        MyBase.ErrorText = "     Invalid" & vbNewLine &
    '            "     date" & vbNewLine &
    '            "     value" & vbNewLine

    '        MyBase.Tag = MYSQLDateFormat(CDate(n_MinimumDate))

    '        MyBase.Value = Format(CDate(n_MinimumDate), machineShortDate)

    '    End Try

    '    Return MyBase.SetValue(rowIndex, value)

    'End Function

    Protected Overrides Sub OnEnter(rowIndex As Integer, throughMouseClick As Boolean)

        Try

            If MyBase.Value = Nothing Then
                ValueBefore = Nothing
            Else

                ValueBefore = Format(CDate(MyBase.Value), machineShortDate)

            End If

        Catch ex As Exception

            ValueBefore = n_MinimumDate

        End Try

        MyBase.OnEnter(rowIndex, throughMouseClick)

    End Sub

    Dim n_Tag As Object = Nothing

    Overloads Property Tag As Object

        Get
            Return n_Tag

        End Get

        Set(value As Object)
            n_Tag = value

        End Set

    End Property

    Protected Overrides Sub OnLeave(rowIndex As Integer, throughMouseClick As Boolean)

        Try

            Dim thisValue = MyBase.Value

            If thisValue Is Nothing Then

                MyBase.Tag = MYSQLDateFormat(CDate(n_MinimumDate))

                MyBase.Value = Nothing

                MyBase.ErrorText = Nothing

            ElseIf ValueBefore <> MyBase.Value Then

                Try

                    MyBase.ErrorText = Nothing

                    MyBase.Tag = MYSQLDateFormat(CDate(MyBase.Value))

                    MyBase.Value = Format(CDate(MyBase.Value), machineShortDate)

                Catch ex As Exception

                    MyBase.ErrorText = "     Invalid" & vbNewLine &
                        "     date" & vbNewLine &
                        "     value" & vbNewLine

                    MyBase.Tag = MYSQLDateFormat(CDate(n_MinimumDate))

                    MyBase.Value = Format(CDate(n_MinimumDate), machineShortDate)

                Finally

                    ValueBefore = MyBase.Value

                End Try

            End If

        Catch ex As Exception

            MyBase.ErrorText = "     Invalid" & vbNewLine &
                "     date" & vbNewLine &
                "     value" & vbNewLine

            MyBase.Tag = MYSQLDateFormat(CDate(n_MinimumDate))

            MyBase.Value = Format(CDate(n_MinimumDate), machineShortDate)

        Finally

            n_Tag = MyBase.Tag

        End Try

        MyBase.OnLeave(rowIndex, throughMouseClick)

    End Sub

End Class

'    With MachineLocalization.Columns
'        .Add("Locale")
'        .Add("LocaleName")
'        .Add("s1159")
'        .Add("s2359")
'        .Add("sCountry")
'        .Add("sCurrency")
'        .Add("sDate")
'        .Add("sDecimal")
'        .Add("sGrouping")
'        .Add("sLanguage")
'        .Add("sList")
'        .Add("sLongDate")
'        .Add("sMonDecimalSep")
'        .Add("sMonGrouping")
'        .Add("sMonThousandSep")
'        .Add("sNativeDigits")
'        .Add("sNegativeSign")
'        .Add("sPositiveSign")
'        .Add("sShortDate")
'        .Add("sThousand")
'        .Add("sTime")
'        .Add("sTimeFormat")
'        .Add("sShortTime")
'        .Add("sYearMonth")
'        .Add("iCalendarType")
'        .Add("iCountry")
'        .Add("iCurrDigits")
'        .Add("iCurrency")
'        .Add("iDate")
'        .Add("iDigits")
'        .Add("NumShape")
'        .Add("iFirstDayOfWeek")
'        .Add("iFirstWeekOfYear")
'        .Add("iLZero")
'        .Add("iMeasure")
'        .Add("iNegCurr")
'        .Add("iNegNumber")
'        .Add("iPaperSize")
'        .Add("iTime")
'        .Add("iTimePrefix")
'        .Add("iTLZero")
