Public Class CustomDatePicker

    Inherits DateTimePicker

    Sub New()
        CustomDatePicker()
    End Sub

    Sub CustomDatePicker()
        MyBase.Value = VBDateTimePickerValueFormat(Now) 'VBDateTimePickerValueFormat
        MyBase.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        MyBase.Format = DateTimePickerFormat.Custom
    End Sub

    'Overloads Property Value As Object

    '    Get
    '        Return MyBase.Value
    '    End Get

    '    Set(value As Object)
    '        MyBase.Value = VBDateTimePickerValueFormat(CDate(value))
    '    End Set

    'End Property

    Protected Overrides Sub OnValueChanged(eventargs As EventArgs)

        Try
            MyBase.Tag = MYSQLDateFormat(MyBase.Value)
        Catch ex As Exception
            MyBase.Tag = Nothing

        End Try

        MyBase.OnValueChanged(eventargs)

    End Sub



End Class