
Public Interface IMySQLCommander

    Property ParameterValueCollection() As Object()

    Property CommandQuery As String

    Property VariableNameOfReturningValue As String

    Property CommandTimingOut As Integer

End Interface