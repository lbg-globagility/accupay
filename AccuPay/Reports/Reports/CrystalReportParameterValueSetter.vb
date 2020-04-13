Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class CrystalReportParameterValueSetter
    Private ReadOnly _reportDocument As ReportClass

    Public Sub New(reportDocument As ReportClass)
        _reportDocument = reportDocument
    End Sub

    Public Sub SetParameter(parameterName As String, parameterValue As Object)
        Try
            Dim parameterFields = _reportDocument.DataDefinition.ParameterFields
            Dim crParameterFieldDefinition = parameterFields.Item(parameterName)
            Dim crParameterValues = crParameterFieldDefinition.CurrentValues
            Dim crParameterDiscreteValue As New ParameterDiscreteValue

            'Dim parameterFieldNames = parameterFields.OfType(Of ParameterFieldDefinition).Select(Function(p) p.ParameterFieldName).ToList()

            crParameterDiscreteValue.Value = parameterValue

            'crParameterValues.Clear()
            crParameterValues.Add(crParameterDiscreteValue)
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub SetParameters(parameters As Dictionary(Of String, Object))
        For Each parameter In parameters
            SetParameter(parameter.Key, parameter.Value)
        Next
    End Sub
End Class
