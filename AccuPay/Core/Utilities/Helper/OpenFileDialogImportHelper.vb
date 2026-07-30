Option Strict On
Imports System.IO

Namespace Global.AccuPay.Desktop.Helpers

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

        Public Shared Function BrowseFile(filter As String, Optional maxMediumBlobBytes As Long = 16777215) As BrowseFileOutPut

            Using browsedFile = New OpenFileDialog()

                With browsedFile
                    .Filter = filter

                    If .ShowDialog() = DialogResult.OK Then

                        Dim fileInfo As New FileInfo(.FileName)

                        If fileInfo.Length > maxMediumBlobBytes Then
                            Dim fileSizeMB As Double = fileInfo.Length / 1024.0 / 1024.0

                            MessageBox.Show(
                                $"The selected file is too large.{vbCrLf}{vbCrLf}" &
                                $"File size: {fileSizeMB:F2} MB{vbCrLf}" &
                                $"Maximum allowed: 16.00 MB (16,777,215 bytes)",
                                "File Too Large",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)

                            Return BrowseFileOutPut.Failed()
                        End If

                        Return BrowseFileOutPut.Success(.FileName)

                    Else
                        Return BrowseFileOutPut.Failed()

                    End If

                End With

            End Using

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
