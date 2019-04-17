Option Strict On

Imports System.Configuration
Imports System.IO
Imports System.Xml.Linq
Imports System.Collections.Specialized

Public Class FeatureListChecker

    ''' <summary>
    ''' The default file name of the feature list.
    ''' </summary>
    Private Const DefaultFileName = "./features.xml"

    ''' <summary>
    ''' The list of optional features that AccuPay has and its corresponding correct password.
    ''' </summary>
    Private _featureList As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
        {"MassOvertime", "1189C349-DDA2-4654-8E7F-DC5FC62513C3"},
        {"JobLevel", "1189C349-DDA2-4654-8E7F-DC5FC62513C1"},
        {"AdditionalVacationLeaveType", "29abbfc8-4645-4153-9a9f-84794fad672f"}
    }

    ''' <summary>
    ''' The list of features that is enabled for a client.
    ''' </summary>
    Private _availableFeatures As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    Private Shared ReadOnly _instance As New Lazy(Of FeatureListChecker)(
        Function()
            Return New FeatureListChecker(DefaultFileName)
        End Function,
        System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Public Shared ReadOnly Property Instance As FeatureListChecker
        Get
            Return _instance.Value
        End Get
    End Property

    Public Sub New(stream As Stream)
        Initialize(stream)
    End Sub

    Public Sub New(filename As String)
        Try
            Dim stream = New MemoryStream(File.ReadAllBytes(filename))
            Initialize(stream)
        Catch ex As FileNotFoundException
            ' Nothing to do here as it's normal to have no feature list file.
        End Try
    End Sub

    Private Sub Initialize(stream As Stream)
        Dim document = XDocument.Load(stream)

        Dim features =
            From e In document.Root.Elements("feature")
            Select e

        For Each feature In features
            Dim name = feature.Attribute("name").Value
            Dim password = feature.Attribute("password").Value

            _availableFeatures.Add(name, password)
        Next
    End Sub

    Public Function HasAccess(feature As Feature) As Boolean
        Dim featureName = feature.ToString()

        If _availableFeatures.ContainsKey(featureName) Then
            Return String.Equals(_availableFeatures(featureName), _featureList(featureName))
        End If

        Return False
    End Function

End Class

Public Enum Feature
    MassOvertime
    JobLevel
    AdditionalVacationLeaveType
End Enum
