Imports System.IO

Public Class CustomPictureBox

    Inherits PictureBox

    Public Event AfterImageChanged As EventHandler(Of EventArgs)

    Public Event BeforeImageChanged As EventHandler(Of EventArgs)

    Sub OnAfterImageChanged(ByVal e As EventArgs)

        RaiseEvent AfterImageChanged(Me, e)

    End Sub

    Sub OnBeforeImageChanged(ByVal e As EventArgs)

        RaiseEvent BeforeImageChanged(Me, e)

    End Sub

    Public Overridable Shadows Property Image() As Image

        Get

            Return MyBase.Image

        End Get

        Set(value As Image)

            OnBeforeImageChanged(EventArgs.Empty)

            If MyBase.Image IsNot value Then
                
                MyBase.Image = value

                MyBase.Tag = imgToByteArray(value)

            Else

                MyBase.Image = Nothing

                MyBase.Tag = Nothing

            End If

            OnAfterImageChanged(EventArgs.Empty)

        End Set

    End Property

    'convert image to bytearray
    Private Function imgToByteArray(ByVal img As Image) As Byte()
        Using mStream As New MemoryStream()
            img.Save(mStream, img.RawFormat)
            Return mStream.ToArray()
        End Using
    End Function

    'convert bytearray to image
    Private Function byteArrayToImage(ByVal byteArrayIn As Byte()) As Image
        Using mStream As New MemoryStream(byteArrayIn)
            Return Image.FromStream(mStream)
        End Using
    End Function

    'another easy way to convert image to bytearray
    Private Shared Function imgToByteConverter(ByVal inImg As Image) As Byte()
        Dim imgCon As New ImageConverter()
        Return DirectCast(imgCon.ConvertTo(inImg, GetType(Byte())), Byte())
    End Function

End Class