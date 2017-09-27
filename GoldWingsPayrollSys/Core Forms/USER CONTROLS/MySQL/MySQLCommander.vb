
Public Class MySQLCommander

#Region "Implementation"
    Implements IMySQLCommander

#End Region

#Region "Variables"

    Private param_value_collect() As Object

    Private cmd_query As String = String.Empty

    Private varname_of_returnval As String = String.Empty

    Private cmd_timeout As Integer = 0

#End Region

#Region "Properties"

    Property ParameterValueCollection() As Object() Implements IMySQLCommander.ParameterValueCollection

        Get
            Return param_value_collect

        End Get

        Set(value As Object())
            param_value_collect = value

        End Set

    End Property

    Property CommandQuery As String Implements IMySQLCommander.CommandQuery

        Get
            Return cmd_query

        End Get

        Set(value As String)
            cmd_query = value

        End Set

    End Property

    Property VariableNameOfReturningValue As String Implements IMySQLCommander.VariableNameOfReturningValue

        Get
            Return varname_of_returnval

        End Get

        Set(value As String)
            varname_of_returnval = value

        End Set

    End Property

    Property CommandTimingOut As Integer Implements IMySQLCommander.CommandTimingOut

        Get
            Return cmd_timeout

        End Get

        Set(value As Integer)
            cmd_timeout = value

        End Set

    End Property

    'ReadOnly Property GetFoundRow As Object
    '    Get
    '        Dim my_exec_cmd As New  _
    '            MySQLExecuteCommand(Me)

    '        Return my_exec_cmd.GetFoundRow

    '    End Get

    'End Property

    'ReadOnly Property GetFoundRows As DataSet
    '    Get
    '        Dim my_exec_cmd As New  _
    '            MySQLExecuteCommand(Me)

    '        Return my_exec_cmd.GetFoundRows

    '    End Get

    'End Property

#End Region

#Region "Methods"

    Sub New(command_query As String)

        If command_query.Trim.Length > 0 Then
            cmd_query = command_query

        End If

    End Sub

#End Region

#Region "Functions"

#End Region

End Class