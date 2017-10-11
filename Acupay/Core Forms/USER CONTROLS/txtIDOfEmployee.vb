
Public Class txtIDOfEmployee

    Dim n_Exists As Boolean = False

    'Public Event TextChanging(sender As Object, e As TextChangingEventArgs)


    Public ReadOnly Property Exists As Boolean

        Get

            Return n_Exists

            'Dim ee As New TextChangingEventArgs("")

            'RaiseEvent TextChanging(Me, ee)

            'If ee.Cancel = False Then

            '    MyBase.Text = ""

            'End If

        End Get

    End Property

End Class

Public Class TextChangingEventArgs

    Inherits System.ComponentModel.CancelEventArgs

    Private p_sNewValue As String

    Public Sub New()

        p_sNewValue = String.Empty

    End Sub

    Public Sub New(sNewValue As String)

        p_sNewValue = sNewValue

    End Sub

    Public Property NewValue As String

        Get

            Return p_sNewValue

        End Get

        Set(value As String)

            p_sNewValue = value

        End Set

    End Property

End Class