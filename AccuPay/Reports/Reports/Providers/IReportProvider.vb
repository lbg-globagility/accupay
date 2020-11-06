Imports System.Threading.Tasks

Public Interface IReportProvider

    Property Name As String
    Property IsHidden As Boolean

    Sub Run()

End Interface
