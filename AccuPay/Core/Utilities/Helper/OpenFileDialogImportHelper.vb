Option Strict On

Namespace Global.AccuPay.Helpers

    Public Class OpenFileDialogImportHelper

        Public Shared Function BrowseFile() As BrowseFileOutPut

            Dim browsedFile = New OpenFileDialog With {
                .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"
            }

            If browsedFile.ShowDialog() = DialogResult.OK Then

                Return BrowseFileOutPut.Success(browsedFile.FileName)
            Else

                Return BrowseFileOutPut.Failed()

            End If

        End Function

        Public Class BrowseFileOutPut

            Property IsSuccess As Boolean
            Property FileName As String

            Public Shared Function Success(fileName As String) As BrowseFileOutPut

                Return New BrowseFileOutPut(True, fileName)

            End Function

            Public Shared Function Failed() As BrowseFileOutPut

                Return New BrowseFileOutPut(False, Nothing)

            End Function

            Private Sub New(isSuccess As Boolean, fileName As String)

                Me.IsSuccess = isSuccess
                Me.FileName = fileName

            End Sub

        End Class

    End Class

End Namespace
