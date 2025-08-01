﻿Option Strict On

Imports System.IO

Namespace Global.AccuPay.Desktop.Helpers

    Public Class SaveFileDialogHelper

        Public Shared Function BrowseFile(
                                defaultFileName As String,
                                Optional defaultExtension As String = Nothing,
                                Optional filter As String = Nothing) As SaveFileDialogHelperOutPut

            Dim saveFileDialog = New SaveFileDialog With {
                .RestoreDirectory = True,
                .FileName = defaultFileName
            }

            If String.IsNullOrWhiteSpace(defaultExtension) = False Then

                saveFileDialog.DefaultExt = defaultExtension
                saveFileDialog.AddExtension = True

            End If

            If String.IsNullOrWhiteSpace(filter) = False Then

                saveFileDialog.Filter = filter

            End If

            Dim fileName = String.Empty
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                fileName = saveFileDialog.FileName
            Else
                Return SaveFileDialogHelperOutPut.Failed()
            End If

            Dim newFile = New FileInfo(fileName)

            If newFile.Exists Then
                newFile.Delete()
                newFile = New FileInfo(fileName)
            End If

            Return SaveFileDialogHelperOutPut.Success(newFile)

        End Function

        Public Class SaveFileDialogHelperOutPut

            Property IsSuccess As Boolean
            Property FileInfo As FileInfo

            Public Shared Function Success(fileInfo As FileInfo) As SaveFileDialogHelperOutPut

                Return New SaveFileDialogHelperOutPut(True, fileInfo)

            End Function

            Public Shared Function Failed() As SaveFileDialogHelperOutPut

                Return New SaveFileDialogHelperOutPut(False, Nothing)

            End Function

            Private Sub New(isSuccess As Boolean, fileInfo As FileInfo)

                Me.IsSuccess = isSuccess
                Me.FileInfo = fileInfo

            End Sub

        End Class

    End Class

End Namespace