
Public Class SQL

#Region "Variables"

    Private mysql_cmd As New MySQLCommander(String.Empty)

    Dim has_error As Boolean = False

    Dim err_msg As String = String.Empty

    Private err As Exception

#End Region

#Region "Properties"

    ReadOnly Property GetFoundRow As Object
        Get
            Dim my_exec_cmd As New  _
                MySQLExecuteCommand(mysql_cmd)

            Return my_exec_cmd.GetFoundRow

            AssingError(my_exec_cmd)

        End Get

    End Property

    ReadOnly Property GetFoundRows As DataSet
        Get
            Dim my_exec_cmd As New  _
                MySQLExecuteCommand(mysql_cmd)

            Return my_exec_cmd.GetFoundRows

            AssingError(my_exec_cmd)

        End Get

    End Property

    ReadOnly Property ErrorException As Exception
        Get
            Return err
        End Get
    End Property

    ReadOnly Property HasError As Boolean
        Get
            Return has_error

        End Get

    End Property

    ReadOnly Property ErrorMessage As String
        Get
            Return err_msg

        End Get

    End Property

#End Region

#Region "Methods"

    Sub New(sql_command As String,
            ParamArray parram_vals() As Object)
        mysql_cmd = New MySQLCommander(sql_command)

        mysql_cmd.ParameterValueCollection = parram_vals

    End Sub

    Sub New()

    End Sub

    Sub ExecuteQuery()
        Dim my_exec_cmd As New  _
                MySQLExecuteCommand(mysql_cmd)

        Dim obj As Object = my_exec_cmd.GetFoundRow

        AssingError(my_exec_cmd)

    End Sub

    Private Sub AssingError(mysql_cmdr As MySQLExecuteCommand)
        err = mysql_cmdr.ErrorException
        has_error = mysql_cmdr.HasError
        err_msg = mysql_cmdr.ErrorMessage
    End Sub

#End Region

End Class